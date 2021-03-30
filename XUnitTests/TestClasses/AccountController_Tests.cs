using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

using static XUnitTests.SeedInMemoryDatabase;


// double init of database (cause 2 test clasees created)

namespace XUnitTests
{
    public class AccountController_Tests : IClassFixture<TestClassBase>
    {
        [Theory]
        [MemberData(nameof(TestCaseSource.RegisterDtoData), MemberType = typeof(TestCaseSource))]
        public void Register_Test(string name, string password, UserDto expectedResult) 
        {
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(obj => obj.Map<AppUser>(It.IsAny<RegisterDto>()))
                .Returns(new AppUser());

            var mockTokenService = new Mock<ITokenService>();
            mockTokenService.Setup(obj => obj.CreateToken(It.IsAny<AppUser>()))
                .ReturnsAsync("token");

            var mockUserManager = GetUserManagerMock<AppUser>();
            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            mockUserManager.Setup(x => x.FindByNameAsync(name))
                        .Returns(Task.FromResult(new AppUser { UserName = name, PasswordHash = password }));

            var context = new DataContext(GetOptions());

            mockUserManager.Setup(x => x.Users)
                .Returns(context.Users);

            mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "Member"))
                .Returns(Task.FromResult(IdentityResult.Success));

            var controller = new AccountController(mockUserManager.Object, null, mockMapper.Object, mockTokenService.Object);

            var res = controller.Register(new RegisterDto { UserName = name, Password = password }).Result;

            Assert.NotNull(res);
        }

        Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser<int>
        {
            return new Mock<UserManager<TIDentityUser>>(
                    new Mock<IUserStore<TIDentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<TIDentityUser>>().Object,
                    new IUserValidator<TIDentityUser>[0],
                    new IPasswordValidator<TIDentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<TIDentityUser>>>().Object);
        }
    }
}
