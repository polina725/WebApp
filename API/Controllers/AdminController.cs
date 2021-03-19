using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BasicController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        public AdminController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _photoService = photoService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            if (roles == null)
                return BadRequest("User must have at least 1 role");
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return NotFound("Couldn't find a user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var res = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!res.Succeeded)
                return BadRequest("Filed to add roles");

            res = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!res.Succeeded)
                return BadRequest("Failed to remve from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }


        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult<IEnumerable<PhotoApprovalDto>>> GetPhotosForModeration()
        {
            var photos = await _unitOfWork.PhotoRepository.GetUnapprovedPhotos();
            return Ok(photos);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<ActionResult> RejectPhoto(int photoId)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if(result.Result == "ok")
                    _unitOfWork.PhotoRepository.RemovePhoto(photo);
            }
            else
            {
                _unitOfWork.PhotoRepository.RemovePhoto(photo);
            }

            await _unitOfWork.Complete();

            return Ok();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<ActionResult> ApprovePhoto(int photoId)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if(photo == null)
                return NotFound("Could not find photo");

            photo.IsApproved = true;

            var user = await _unitOfWork.UserRepository.getUserByPhotoId(photoId);

            if(!user.Photo.Any(p => p.IsMain))
                photo.IsMain = true;

            await _unitOfWork.Complete();

            return Ok();
        }
    }
}