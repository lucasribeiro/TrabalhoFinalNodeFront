using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreTutorial.Models.Domain;
using NuGet.Common;

namespace MvcCoreTutorial.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;

        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
           

            if (ModelState.IsValid)
            {
                try
                {
                    var token = await Service.Service.Login(login);
                    HttpContext.Session.SetString("token", token.token);

                    return RedirectToAction("DisplayAparelho", "Aparelho");
                }
                catch (Exception ex)
                {
                    TempData["msg"] = "Email/Senha inválidos!";
                }

            }

            return View();
        }

    }
}
