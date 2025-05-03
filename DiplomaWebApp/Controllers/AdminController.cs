using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaWebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IGameRepository gameRep;
        public IActionResult Games(IGameRepository gamerep)
        {

            return View();
        }
    }
}
