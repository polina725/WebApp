using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);

         Task<bool> SaveAllAsync();

         Task<IEnumerable<AppUser>> GetUsersAsync();

         Task<AppUser> GetUserByIdAsync(int id);

         Task<AppUser> GetUserByUserNameAsync(string name);

         Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

        Task<MemberDto> GetMemberAsync(string name);
    }
}