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
using System.Diagnostics;
using System.Security.Claims;

namespace DiplomaWebApp.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    [EnableCors()]
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
        [HttpPost]
        
        public IActionResult Login([FromBody]LoginRecord input)
        {
            string login=input.Login;
            string password=input.Password;
            if (isEmptyLoginPassword(login, password))
                return Json(new { success = 0, message = "�� ������ ������ ��� �����" });
            Jure jure = jureRep.GetJure(login);
            if (jure == null)
            {
                return Json(new { success = 0, message = "�� ������ ������������ � ���� �������" });
            }
            else
            {
                if (jure.Password == password)
                {
                    ExecuteLogin(login);
                    return Json(new { success = 1, message = "�� ������� ����� � �������" });

                }
                else
                {
                    return Json(new { success = 0, message = "������������ ������" });
                }
            }
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Json(new { success = 1, message = "�� ������� ����� �� �������" });
        }
        

        [HttpPost]
        public IActionResult Registry([FromBody]JureRecord jure)
        {
            if (!ModelState.IsValid)
            {
                return JsonFirstError();
            }
            Jure dbJure = jureRep.GetJure(jure.Login);
            if (dbJure != null)
            {
                return Json(new {success=0,message= "��������� ����� ��� �����, �������� ������" });
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
                ViewBag.Message = "�� ������� ����� � �������";
                ExecuteLogin(jure.Login);
                return Json(new { success = 1, message ="�� ������� ������������������ � ����� � �������"});

            }
        }
        [NonAction]
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
        [NonAction]
        private bool isEmptyLoginPassword(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
            {
                ViewBag.Message = "����� ������ ���� �� ������ �������";
                return true;
            }
            if (string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "������ ������ ���� �� ������ �������";
                return true;
            }
            return false;
        }
        [NonAction]
        private IActionResult JsonFirstError()
        {
            return Json(new { success = 0, message = ModelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage });
        }
    }
}
