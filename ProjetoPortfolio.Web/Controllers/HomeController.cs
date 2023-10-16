using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.Web.Domain;
using ProjetoPortfolio.Web.Models;
using ProjetoPortfolio.Web.Models.Response;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ProjetoPortfolio.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var request = new Request<ConteudoModel>();
                var response = await request.Listar("Conteudo/Conteudos");
                var result = response.Results;
                if (result != null)
                {
                    var conteudos = new List<ConteudoModel>(result);
                    return View(conteudos);
                }
                return View();
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public async Task<PorfolioResponse<ProjetoModel>> ListarProjetos()
        {
            try
            {
                var request = new Request<ProjetoModel>();
                var response = await request.Listar("Projeto/Projetos");
                if (response == null)
                    throw new Exception("Erro no servidor");
                var projetos = response.Results;
                
                PorfolioResponse<ProjetoModel> result = new()
                {
                    Results = projetos.Take(5),
                    Errors = response.Errors
                };
                
                return result;
            }
            catch (Exception ex)
            {

                return new PorfolioResponse<ProjetoModel>()
                {
                    Results = new List<ProjetoModel>(),
                    Errors = new List<string>()
                    {
                        ex.Message,
                    }
                };
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}