using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        public PhotoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Photo> GetPhotoById(int photoId)
        {
            return await _context.Photos
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(p => p.Id == photoId);
        }

        public async Task<IEnumerable<PhotoApprovalDto>> GetUnapprovedPhotos()
        {
            return await _context.Photos
                .IgnoreQueryFilters()
                .Where(p => !p.IsApproved)
                .Select(p => new PhotoApprovalDto{
                    Id = p.Id,
                    Url = p.Url,
                    Username = p.AppUser.UserName,
                    IsApproved = p.IsApproved
                }).ToListAsync();

        }

        public void RemovePhoto(Photo photo)
        {
            _context.Remove(photo);
        }
    }
}