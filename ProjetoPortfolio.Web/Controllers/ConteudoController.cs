using Microsoft.AspNetCore.Mvc;

namespace ProjetoPortfolio.Web.Controllers
{
    public class ConteudoController : Controller
    {
        private IHttpClientBuilder httpClient = null;

        public IActionResult Index()
        {
            return View();
        }
    }
}
