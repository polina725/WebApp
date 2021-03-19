using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext, IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public async Task<MemberDto> GetMemberAsync(string name, bool isCurrentUser)
        {
            var query = _dataContext.Users.Where(user => user.UserName == name)
                            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                            .AsQueryable();
            if (isCurrentUser)
                query =query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _dataContext.Users.AsQueryable();
            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), 
                userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<AppUser> getUserByPhotoId(int photoId)
        {
            return await _dataContext.Users
                .Include(p => p.Photo)
                .IgnoreQueryFilters()
                .Where(user => user.Photo.Any(p => p.Id == photoId))
                .FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByUserNameAsync(string name)
        {
            return await _dataContext.Users.Include(p => p.Photo).SingleOrDefaultAsync(user => user.UserName == name);
        }

        public async Task<string> GetUserGender(string username)
        {
            return await _dataContext.Users
                .Where(u =>u.UserName == username)
                .Select(u => u.Gender)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _dataContext.Users.Include(p => p.Photo).ToListAsync();
        }
        
        public void Update(AppUser user)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}