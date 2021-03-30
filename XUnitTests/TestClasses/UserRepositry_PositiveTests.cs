using API.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;
using static XUnitTests.SeedInMemoryDatabase;

namespace XUnitTests
{
    public class UserRepositry_PositiveTests : IClassFixture<TestClassBase>
    {
        
        [Theory]
        [MemberData(nameof(TestCaseSource.Ids), MemberType = typeof(TestCaseSource))]
        public async Task GetUserById_Test(int id)
        {
            using (var context = new DataContext(GetOptions()))
            {
                var mapper = new Mock<IMapper>();

                var userRepo = new UserRepository(context, mapper.Object);

                var user = await userRepo.GetUserByIdAsync(id);

                Assert.NotNull(user);
                Assert.Equal(id, user.Id);

                Assert.Null(user.LikedBy);
            }

        }

        [Theory]
        [MemberData(nameof(TestCaseSource.Usernames), MemberType = typeof(TestCaseSource))]
        public async Task GetUserByUserName_Test(string username)
        {
            using (var context = new DataContext(GetOptions()))
            {
                var mapper = new Mock<IMapper>();

                var userRepo = new UserRepository(context, mapper.Object);
                var l = await context.Users.ToListAsync();

                var user = await userRepo.GetUserByUserNameAsync(username);


                var ph = await context.Photos.ToListAsync();

                Assert.NotNull(user);
                Assert.Equal(username, user.UserName);

                Assert.NotNull(user.Photo);
            }
        }

        [Theory]
        [MemberData(nameof(TestCaseSource.PhotoIds), MemberType = typeof(TestCaseSource))]
        public async Task GetUserByPhotoId_Test(int id, string expectedUsername, int expectedPhotoCount)
        {
            var mapper = new Mock<IMapper>();

            using(var context = new DataContext(GetOptions()))
            {
                var userRepo = new UserRepository(context, mapper.Object);

                var user = await userRepo.getUserByPhotoId(id);

                Assert.NotNull(user);
                Assert.Equal(expectedUsername, user.UserName);

                Assert.NotNull(user.Photo);
                Assert.Equal(expectedPhotoCount, user.Photo.Count);
            }
            
        }

        //[Fact]
        //public async Task MinorCheck()
        //{
        //    using (var context = new DataContext(GetOptions()))
        //    {
        //        var users = await context.Users.ToListAsync();

        //        foreach(var user in users)
        //        {
        //            Assert.NotNull(user);
        //            Assert.Equal("lol", user.PasswordHash);
        //        }
        //    }
        //}
    }
}
