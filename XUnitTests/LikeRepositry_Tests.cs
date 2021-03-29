using API.Data;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;
using static XUnitTests.MockDatabase;

namespace XUnitTests
{
    public class LikeRepositry_Tests
    {
        [Fact]
        public async Task GetUserById_Test()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDataBase")
                .Options;
            using (var context = new DataContext(options))
            {
                SeedData(context);
            }

            using (var context = new DataContext(options))
            {
                var mapper = new Mock<IMapper>();

                var userRepo = new UserRepository(context, mapper.Object);

                var user = await userRepo.GetUserByIdAsync(2);

                Assert.NotNull(user);

                Assert.Equal(2, user.Id);
            }

        }
    }
}
