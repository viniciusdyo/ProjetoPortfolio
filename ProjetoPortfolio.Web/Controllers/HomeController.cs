using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPortfolio.Web.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ProjetoPortfolio.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient = null;
        private readonly string ENDPOINT = "https://localhost:44318/api";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var conteudoResponse = await _httpClient.GetAsync($"{ENDPOINT}/Conteudo/Conteudos");
                if(conteudoResponse.IsSuccessStatusCode)
                {
                    var readConteudoTask = await conteudoResponse.Content.ReadAsStringAsync();
                    var conteudos = JsonConvert.DeserializeObject<List<ConteudoModel>>(readConteudoTask).Where(x => x.CategoriaConteudoModel.Nome == "Home");
                    return View(conteudos);
                }
                return View();

            }
            catch (Exception ex)
            {
                return View(ex);
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