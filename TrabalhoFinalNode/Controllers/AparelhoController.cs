using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcCoreTutorial.Models.Domain;
using TrabalhoFinalNode.Models.Domain;

namespace MvcCoreTutorial.Controllers
{
    public class AparelhoController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AparelhoController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        public IActionResult Index()
        {
            ViewBag.greeting = "Hello world";
            ViewData["greeting2"] = "I am using ViewData";
            //ViewBag and ViewData can send data only from ControllerToView

            //Tempdata can send data from one controller method to another controller method
            TempData["greeting3"] = "Its TempData msg";
            return View();
        }

        //it is get method
        public IActionResult AddAparelho()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult AddAparelho(Aparelho aparelho)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                var token = HttpContext.Session.GetString("token");

                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Login");
                }

                aparelho.id = null;

                var retorno = Service.Service.AddAparelho(aparelho, token).Result;

                TempData["msg"] = "Adicionado com Sucesso!";
                return RedirectToAction("AddAparelho");

            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return View();
            }

        }

        public IActionResult DisplayAparelho()
        {
            var token = HttpContext.Session.GetString("token");

            if (String.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }
            
            var aparelhos = Service.Service.GetAllAparelhos(token).Result;

            return View(aparelhos);
           
            
        }

        public IActionResult EditAparelho(int id)
        {
            var token = HttpContext.Session.GetString("token");

            if (String.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            var aparelho = Service.Service.GetAparelhoById(id, token).Result;
            return View(aparelho);
        }

        [HttpPost]
        public IActionResult EditAparelho(Aparelho aparelho)
        {
            var token = HttpContext.Session.GetString("token");
            if (String.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            if (!ModelState.IsValid)
            {                
                return View();
            }
            try
            {

                var retorno = Service.Service.UpdateAparelhoById(aparelho, token).Result;
                return RedirectToAction("DisplayAparelho");

            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return View();
            }

        }

        public IActionResult DeleteAparelho(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("token");

                if (String.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Login");
                }

                var aparelho = Service.Service.DeleteAparelhoById(id, token).Result;
                return RedirectToAction("DisplayAparelho");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return View();

            }
            
            
        }
    }
}
