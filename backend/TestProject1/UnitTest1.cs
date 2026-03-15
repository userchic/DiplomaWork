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
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;

namespace TestProject1
{
    public class UnitTest1
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
            var options = new DbContextOptionsBuilder<MathBattlesDbContext>()
            .UseInMemoryDatabase("testDB")
            .Options;
            using (var context = new MathBattlesDbContext(options))
            {
                // Arrange: Seed data into the context
                IJureRepository jureRep = new JureRepository(context);
                HomeController controller = new HomeController(jureRep);
                LoginRequestRecord request = new LoginRequestRecord() 
                {
                    Login=login,
                    Password=password
                };

                // Act: Perform the action to be tested
                JsonResult res = (JsonResult)controller.Login(request);

                // Assert: Verify the result
                JsonResult expectedResult = controller.Json(new { success = 0, message = "Не найден пользователь с этим логином" });
                Assert.Equal(JsonSerializer.Serialize(expectedResult.Value), JsonSerializer.Serialize(res.Value));
            }
        }
        [Theory]
        [InlineData("GabRus", "1234")]
        [InlineData("NWA", "1234")]
        public void Login_RealUsersAndCorrectPassword_SuccessfulLogin(string login, string password)
        {
            var options = new DbContextOptionsBuilder<MathBattlesDbContext>()
            .UseInMemoryDatabase("testDB")
            .Options;
            using (var context = new MathBattlesDbContext(options))
            {
                // Arrange: Seed data into the context
                IJureRepository jureRep = new JureRepository(context);
                HomeController controller = new HomeController(jureRep);
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
            }
        }
        [Theory]
        [InlineData("GabRus", "password")]
        [InlineData("GabRus", "12345")]
        [InlineData("NWA", "12345")]
        public void Login_WrongPassword_SuccessfulLogin(string login, string password)
        {
            var options = new DbContextOptionsBuilder<MathBattlesDbContext>()
            .UseInMemoryDatabase("testDB")
            .Options;
            using (var context = new MathBattlesDbContext(options))
            {
                // Arrange: Seed data into the context
                IJureRepository jureRep = new JureRepository(context);
                HomeController controller = new HomeController(jureRep);
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
            }
        }
        [Theory]
        [InlineData(["суперЛогин","суперПароль"])]
        [InlineData(["login","password"])]
        public void LoginRequestValidation_ValidParameters_SuccessfulValidation(string login,string password)
        {
            LoginRequestRecord request = new LoginRequestRecord()
            {
                Login = password,
                Password = login
            };
            LoginRequestRecord partialRequest1 = new LoginRequestRecord()
            {
                Password = login
            };
            LoginRequestRecord partialRequest2 = new LoginRequestRecord()
            {
                Login = password
            };
            IList<ValidationResult> results1 = ValidationTesting.ValidateModel(request);
            IList<ValidationResult> results2 = ValidationTesting.ValidateModel(partialRequest1);
            IList<ValidationResult> results3 = ValidationTesting.ValidateModel(partialRequest2);

            Assert.Empty(results1);
            Assert.NotEmpty(results2);
            Assert.NotEmpty(results3);
        }
        [Theory]
        [InlineData(["", "суперПароль"])]
        [InlineData(["login", ""])]
        [InlineData(["", ""])]
        public void LoginRequestValidation_InvalidParameters_SuccessfulValidation(string login, string password)
        {
            LoginRequestRecord request = new LoginRequestRecord()
            {
                Login = password,
                Password = login
            };
            LoginRequestRecord partialRequest1 = new LoginRequestRecord()
            {
                Password = login
            };
            LoginRequestRecord partialRequest2 = new LoginRequestRecord()
            {
                Login = password
            };
            IList<ValidationResult> results1 = ValidationTesting.ValidateModel(request);
            IList<ValidationResult> results2 = ValidationTesting.ValidateModel(partialRequest1);
            IList<ValidationResult> results3 = ValidationTesting.ValidateModel(partialRequest2);

            Assert.NotEmpty(results1);
            Assert.NotEmpty(results2);
            Assert.NotEmpty(results3);
        }
        [Theory]
        [InlineData(["Руслан","Габзалилов","Маратович","GabRus","1234"])]
        [InlineData(["Николас", "Вольфгангович", "Amodeus", "NWA", "1234"])]
        public void Registry_RealUser_ExistingUserMessage(string name,string surname,string? fatname,string login, string password)
        {
            var options = new DbContextOptionsBuilder<MathBattlesDbContext>()
            .UseInMemoryDatabase("testDB")
            .Options;
            using (var context = new MathBattlesDbContext(options))
            {
                // Arrange: Seed data into the context
                IJureRepository jureRep = new JureRepository(context);
                HomeController controller = new HomeController(jureRep);
                controller.ControllerContext = new ControllerContext();
                controller.ControllerContext.HttpContext = new DefaultHttpContext();
                controller.HttpContext.RequestServices = new TestServiceProvider();
                RegistryRequestRecord request = new RegistryRequestRecord()
                {
                    Login = login,
                    Password = password,
                    Name=name,
                    Surname=surname,
                    Fatname=fatname
                };

                // Act: Perform the action to be tested
                JsonResult res = (JsonResult)controller.Registry(request);

                // Assert: Verify the result
                JsonResult expectedResult = controller.Json(new { success = 0, message = "Введенный логин уже занят, выберите другой" });
                Assert.Equal(JsonSerializer.Serialize(expectedResult.Value), JsonSerializer.Serialize(res.Value));
            }
        }
        [Theory]
        [InlineData(["Тима", "Димонович", "CPU", "CPU1", "1222131e3e234"])]
        [InlineData(["Волопас", "Лионович", "Error", "Volopas", "13rr32fe234"])]
        public void Registry_NewUser_SuccesfulRegistration(string name, string surname, string? fatname, string login, string password)
        {
            var options = new DbContextOptionsBuilder<MathBattlesDbContext>()
            .UseInMemoryDatabase("testDB")
            .Options;
            using (var context = new MathBattlesDbContext(options))
            {
                // Arrange: Seed data into the context
                IJureRepository jureRep = new JureRepository(context);
                HomeController controller = new HomeController(jureRep);
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
            }
        }
        [Theory]
        [InlineData(["Тима", "Димонович", "CPU", "CPU1", "dwe23rfe23edw2"])]
        [InlineData(["Велес", "workinprogress", "CPU", "CPU2", "dwaferfew23123232"])]
        [InlineData(["Лара", "DI", "Dependency", "CPU3", "1222e32f3e234"])]
        public void RegistryValidation_CorrectData_SuccesfulValidation(string name, string surname, string? fatname, string login, string password)
        {
            var options = new DbContextOptionsBuilder<MathBattlesDbContext>()
            .UseInMemoryDatabase("testDB")
            .Options;
            using (var context = new MathBattlesDbContext(options))
            {
                // Arrange: Seed data into the context
                IJureRepository jureRep = new JureRepository(context);
                HomeController controller = new HomeController(jureRep);
                RegistryRequestRecord request = new RegistryRequestRecord()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname
                };

                //Act
                IList<ValidationResult> results = ValidationTesting.ValidateModel(request);

                // Assert: Verify the result

                Assert.Empty(results);
            }
        }
        [Theory]
        [InlineData(["", "Димонович", "CPU", "CPU1", "dwe23rfe23edw2"])]
        [InlineData(["Велес", "", "CPU", "CPU2", "dwaferfew23123232"])]
        [InlineData(["Лара", "DI", "Dependency", "", "1222e32f3e234"])]
        [InlineData(["", "Димонович", "CPU", "CPU1", "dwe23rfe23edw2"])]
        [InlineData(["Велес", "", "CPU", "CPU2", ""])]
        [InlineData(["", "", "", "", ""])]
        public void RegistryValidation_IncorrectData_FailuredValidation(string name, string surname, string? fatname, string login, string password)
        {
            var options = new DbContextOptionsBuilder<MathBattlesDbContext>()
            .UseInMemoryDatabase("testDB")
            .Options;
            using (var context = new MathBattlesDbContext(options))
            {
                // Arrange: Seed data into the context
                IJureRepository jureRep = new JureRepository(context);
                HomeController controller = new HomeController(jureRep);
                RegistryRequestRecord request = new RegistryRequestRecord()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    Fatname = fatname
                };

                // Act: Perform the action to be tested
                IList<ValidationResult> results = ValidationTesting.ValidateModel(request);

                // Assert: Verify the result

                Assert.NotEmpty(results);
            }
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
    static class ValidationTesting
    {
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}