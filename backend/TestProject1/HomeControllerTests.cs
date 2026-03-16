using DiplomaWebApp.Abstractions;
using DiplomaWebApp.Controllers;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Records;
using DiplomaWebApp.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mistral.SDK.Common;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;

namespace TestProject1
{
    public class HomeControllerTests
    {
        [Theory]
        [InlineData(["",""])]
        [InlineData(["","1234"])]
        [InlineData(["","0"])]
        [InlineData(["space"," "])]
        [InlineData(["user","2543"])]
        [InlineData(["login","7890"])]
        public void Login_NotCreatedUser_FailuredLogin(string login,string password)
        {
            ImplyControllerTest((context, controller) =>
            {
                LoginRequestRecord request = new LoginRequestRecord()
                {
                    Login = login,
                    Password = password
                };

                // Act: Perform the action to be tested
                JsonResult res = (JsonResult)controller.Login(request);

                // Assert: Verify the result
                JsonResult expectedResult = controller.Json(new { success = 0, message = "Не найден пользователь с этим логином" });
                Assert.Equal(JsonSerializer.Serialize(expectedResult.Value), JsonSerializer.Serialize(res.Value));
            });
        }
        [Theory]
        [InlineData("GabRus", "1234")]
        [InlineData("NWA", "1234")]
        public void Login_RealUsersAndCorrectPassword_SuccessfulLogin(string login, string password)
        {
            ImplyControllerTest((context, controller) =>
            {
                controller.ControllerContext = new ControllerContext();
                controller.ControllerContext.HttpContext = new DefaultHttpContext();
                controller.HttpContext.RequestServices = new TestServiceProvider();

                LoginRequestRecord request = new LoginRequestRecord()
                {
                    Login = login,
                    Password = password
                };

                // Act: Perform the action to be tested
                JsonResult res = (JsonResult)controller.Login(request);

                // Assert: Verify the result
                JsonResult expectedResult = controller.Json(new { success = 1, message = "Вы успешно вошли в систему" });
                Assert.Equal(JsonSerializer.Serialize(expectedResult.Value), JsonSerializer.Serialize(res.Value));
            });
        }
        [Theory]
        [InlineData("GabRus", "password")]
        [InlineData("GabRus", "12345")]
        [InlineData("NWA", "12345")]
        public void Login_WrongPassword_SuccessfulLogin(string login, string password)
        {
            ImplyControllerTest((context, controller) =>
            {
                controller.ControllerContext = new ControllerContext();
                controller.ControllerContext.HttpContext = new DefaultHttpContext();
                LoginRequestRecord request = new LoginRequestRecord()
                {
                    Login = login,
                    Password = password
                };

                // Act: Perform the action to be tested
                JsonResult res = (JsonResult)controller.Login(request);

                // Assert: Verify the result
                JsonResult expectedResult = controller.Json(new { success = 0, message = "Неправильный пароль" });

                Assert.Equal(JsonSerializer.Serialize(expectedResult.Value), JsonSerializer.Serialize(res.Value));
            });
        }


        [Theory]
        [InlineData(["Руслан","Габзалилов","Маратович","GabRus","1234"])]
        [InlineData(["Николас", "Вольфгангович", "Amodeus", "NWA", "1234"])]
        public void Registry_RealUser_ExistingUserMessage(string name,string surname,string? fatname,string login, string password)
        {
            ImplyControllerTest((context, controller) =>
            {
                controller.ControllerContext = new ControllerContext();
                controller.ControllerContext.HttpContext = new DefaultHttpContext();
                controller.HttpContext.RequestServices = new TestServiceProvider();
                RegistryRequestRecord request = new RegistryRequestRecord()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname
                };

                // Act: Perform the action to be tested
                JsonResult res = (JsonResult)controller.Registry(request);

                // Assert: Verify the result
                JsonResult expectedResult = controller.Json(new { success = 0, message = "Введенный логин уже занят, выберите другой" });
                Assert.Equal(JsonSerializer.Serialize(expectedResult.Value), JsonSerializer.Serialize(res.Value));
            });
        }
        [Theory]
        [InlineData(["Тима", "Димонович", "CPU", "CPU1", "1222131e3e234"])]
        [InlineData(["Волопас", "Лионович", "Error", "Volopas", "13rr32fe234"])]
        public void Registry_NewUser_SuccesfulRegistration(string name, string surname, string? fatname, string login, string password)
        {
            ImplyControllerTest((context, controller) =>
            {
                controller.ControllerContext = new ControllerContext();
                controller.ControllerContext.HttpContext = new DefaultHttpContext();
                controller.HttpContext.RequestServices = new TestServiceProvider();

                RegistryRequestRecord request = new RegistryRequestRecord()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname
                };

                // Act: Perform the action to be tested
                JsonResult res = (JsonResult)controller.Registry(request);

                // Assert: Verify the result
                JsonResult expectedResult = controller.Json(new { success = 1, message = "Вы успешно зарегистрировались и вошли в систему" }); ;
                Assert.Equal(JsonSerializer.Serialize(expectedResult.Value), JsonSerializer.Serialize(res.Value));
            });
        }

        
        private void ImplyControllerTest(Action<MathBattlesDbContext,HomeController> test)
        {
            var options = new DbContextOptionsBuilder<MathBattlesDbContext>()
                .UseInMemoryDatabase("testDB")
                .Options;
            using (var context = new MathBattlesDbContext(options))
            {
                // Arrange: Seed data into the context
                HomeController controller = InitializeHomeController(context);
                test(context, controller);
            }
        }
        public HomeController InitializeHomeController(MathBattlesDbContext context)
        {
            IJureRepository jureRep = new JureRepository(context);
            return new HomeController(jureRep);
        }
    }
    public class TestServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            return new TestAuthService();
        }
    }
    public class TestAuthService : IAuthenticationService
    {
        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
        {
            return Task.CompletedTask;
        }

        public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }

}