using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;
using DiplomaWebApp.Records;
using DiplomaWebApp.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using System.Diagnostics;
using System.Security.Claims;

namespace DiplomaWebApp.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    [EnableCors("AllowOneOrigin")]
    public class HomeController : Controller
    {
        public IJureRepository jureRep;
        Counter _loginCounter;
        Counter _registryCounter;
        public HomeController(IJureRepository jurerep)
        {
            jureRep = jurerep;
            _loginCounter = Metrics.CreateCounter("logins_total", "increments on login");
            _registryCounter = Metrics.CreateCounter("registries_total", "increments on registry");
        }
        [HttpPost]
        
        public IActionResult Login([FromBody]LoginRequestRecord input)
        {
            if (!ModelState.IsValid)
            {
                return JsonFirstError();
            }
            _loginCounter.Inc();
            _loginCounter.Publish();
            string login=input.Login;
            string password=input.Password;
            Jure jure = jureRep.GetJure(login);
            if (jure == null)
            {
                return Json(new { success = 0, message = "Не найден пользователь с этим логином" });
            }
            else
            {
                if (jure.Password == password)
                {
                    ExecuteLogin(login);
                    return Json(new { success = 1, message = "Вы успешно вошли в систему" });

                }
                else
                {
                    return Json(new { success = 0, message = "Неправильный пароль" });
                }
            }
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Json(new { success = 1, message = "Вы успешно вышли из системы" });
        }
        

        [HttpPost]
        public IActionResult Registry([FromBody]RegistryRequestRecord jure)
        {
            if (!ModelState.IsValid)
            {
                return JsonFirstError();
            }
            Jure dbJure = jureRep.GetJure(jure.Login);
            if (dbJure != null)
            {
                return Json(new {success=0,message= "Введенный логин уже занят, выберите другой" });
            }
            else
            {
                _registryCounter.Inc();
                _registryCounter.Publish();
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
                ExecuteLogin(jure.Login);
                return Json(new { success = 1, message = "Вы успешно зарегистрировались и вошли в систему" });
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
            HttpContext.SignInAsync(claimsPrincipal);
        }
        private IActionResult JsonFirstError()
        {
            return Json(new { success = 0, message = ModelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage });
        }
    }
}
