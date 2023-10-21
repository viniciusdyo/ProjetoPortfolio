using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.Web.Domain;
using ProjetoPortfolio.Web.Models.Response;

namespace ProjetoPortfolio.Web.Controllers
{
    public class SobreMimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
