using DiplomaWebApp.Abstractions;
using DiplomaWebApp.Models;
using DiplomaWebApp.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;
using DiplomaWebApp.DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiplomaWebApp.Records;

namespace DiplomaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IJureRepository jureRep;

        public HomeController(ILogger<HomeController> logger,
            IJureRepository jurerep,MathBattlesDbContext context)
        {
            jureRep = jurerep;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpGet]
        public IActionResult Registry()
        {
            return View("Registry");
        }
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            if (isEmptyLoginPassword(login, password))
                return View("Login");
            Jure jure = jureRep.GetJure(login);
            if (jure == null)
            {
                ViewBag.Message = "Не найден пользователь с этим логином";
                return View("Login");
            }
            else
            {
                if (jure.Password == password)
                {
                    ExecuteLogin(login);
                    ViewBag.Message = "Вы успешно вошли в систему";
                    return View("Success");
                }
                else
                {
                    ViewBag.Message = "Неправильный пароль";
                    return View("Login");
                }
            }
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return View("Login");
        }
        private void ExecuteLogin(string login)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType,login)
                    };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(claimsPrincipal);
        }
        private bool isEmptyLoginPassword(string login, string password)
        {
            if (login.IsNullOrEmpty())
            {
                ViewBag.Message = "Логин должен быть не пустой строкой";
                return true;
            }
            if (password.IsNullOrEmpty())
            {
                ViewBag.Message = "Пароль должен быть не пустой строкой";
                return true;
            }
            return false;
        }
        [HttpPost]
        public IActionResult Registry(JureRecord jure)
        {
            if (!ModelState.IsValid)
            {
                return ShowFirstError("Registry");
            }
            Jure dbJure = jureRep.GetJure(jure.Login);
            if (dbJure != null)
            {
                ViewBag.Message = "Введенный логин уже занят, выберите другой";
                return View("Registry");
            }
            else
            {
                Jure newJure = new Jure()
                {
                    Name = jure.Name,
                    Surname = jure.Surname,
                    Fatname = jure.Fatname,
                    Login = jure.Login,
                    Password = jure.Password
                };
                jureRep.AddJure(newJure);
                jureRep.Save();
                ViewBag.Message = "Вы успешно вошли в систему";
                ExecuteLogin(jure.Login);
                return View("Success");

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private IActionResult ViewWithMessage(string message, string viewName)
        {
            ViewBag.Message = message;
            return View(viewName);
        }
        private IActionResult ShowFirstError(string viewName)
        {
            return ViewWithMessage(ModelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage, viewName);
        }
        private IActionResult JsonFirstError()
        {
            return Json(new { success = 0, message = ModelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage });
        }
    }
}
