using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPortfolio.Web.Models;

namespace ProjetoPortfolio.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminProjetoController : Controller
    {
        private readonly string ENDPOINT = "https://localhost:44318/api";
        private readonly HttpClient httpClient = null;
        public AdminProjetoController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ENDPOINT);
        }
        public async Task<IActionResult> Index()
        {
            List<ProjetoViewModel> projetos = null;
            var responseTask = httpClient.GetAsync($"{ENDPOINT}/Projeto");
            if (responseTask.Result.IsSuccessStatusCode)
            {
                var result = responseTask.Result;
                var readTask = await result.Content.ReadAsStringAsync();
                projetos = JsonConvert.DeserializeObject<List<ProjetoViewModel>>(readTask);
                return View(projetos);
            }
            return View(projetos);

        }


        public async Task<IActionResult> Editar(ProjetoViewModel projeto)
        {
            ProjetoViewModel p = new ProjetoViewModel()
            {
                Id = projeto.Id,
                Titulo= projeto.Titulo,
                Descricao=projeto.Descricao,
                Status= projeto.Status,
                Excluido = projeto.Excluido,
                TituloNormalizado = projeto.TituloNormalizado,
            };
            var responseTask = await httpClient.PutAsJsonAsync($"{ENDPOINT}/Projeto/Atualizar", p);
            if (responseTask.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            } else
            {
                return RedirectToAction("Index");
            }
        }
    }

}
