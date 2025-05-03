using DiplomaWebApp.Models;
using DiplomaWebApp.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;

namespace DiplomaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IJureRepository jureRep;

        public HomeController(ILogger<HomeController> logger,
            IJureRepository jurerep)
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
        public IActionResult Login(string login,string password)
        {
            if (isEmptyLoginPassword(login, password))
                return View("Login");
            Jure jure = jureRep.GetJure(login);
            if(jure == null)
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
        private void ExecuteLogin(string login)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType,login)
                    };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignOutAsync();
            HttpContext.SignInAsync(claimsPrincipal);
        }
        private bool isEmptyLoginPassword(string login,string password)
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
        public IActionResult Registry(string login,string password,string name,string surname,string fatname)
        {
            if (isEmptyLoginPassword(login, password))
                return View("Registry");
            if (name.IsNullOrEmpty())
            {
                ViewBag.Message = "Не введено имя пользователя";
                return View("Registry");
            }
            if (surname.IsNullOrEmpty())
            {
                ViewBag.Message = "Не введена фамилия пользователя";
                return View("Registry");
            }
            if (fatname.IsNullOrEmpty())
            {
                ViewBag.Message = "Не введено отчество пользователя";
                return View("Registry");
            }
            Jure jure=jureRep.GetJure(login);
            if(jure!=null)
            {
                ViewBag.Message = "Введенный логин уже занят, выберите другой";
                return View("Registry");
            }
            else
            {
                Jure newJure = new Jure()
                {
                    Name = name,
                    Surname = surname,
                    FatName = fatname,
                    Login = login,
                    Password = password
                };
                jureRep.AddJure(newJure);
                jureRep.Save();
                ViewBag.Message = "Вы успешно вошли в систему";
                ExecuteLogin(login);
                return View("Success");

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
