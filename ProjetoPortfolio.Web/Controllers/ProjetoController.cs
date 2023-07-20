using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPortfolio.Web.Models;
using System.Text.Json.Serialization;

namespace ProjetoPortfolio.Web.Controllers
{
    public class ProjetoController : Controller
    {
        private readonly string ENDPOINT = "https://localhost:44318/api";
        private readonly HttpClient httpClient = null;
        public ProjetoController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ENDPOINT);
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                List<ProjetoViewModel> projetos = null;
                var responseTask = httpClient.GetAsync($"{ENDPOINT}/Projeto");
                var result = responseTask.Result;
                var readTask = await result.Content.ReadAsStringAsync();
                projetos = JsonConvert.DeserializeObject<List<ProjetoViewModel>>(readTask);
                Console.WriteLine(projetos);
                return View(projetos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


        }
    }
}
