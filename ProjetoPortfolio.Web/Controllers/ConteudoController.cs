using Microsoft.AspNetCore.Mvc;

namespace ProjetoPortfolio.Web.Controllers
{
    public class ConteudoController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
