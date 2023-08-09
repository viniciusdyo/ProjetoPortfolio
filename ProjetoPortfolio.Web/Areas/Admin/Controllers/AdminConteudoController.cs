using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPortfolio.Web.Models;
using ProjetoPortfolio.Web.Models.ViewModels;

namespace ProjetoPortfolio.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminConteudoController : Controller
    {
        private readonly string ENDPOINT = "https://localhost:44318/api";
        private readonly HttpClient _httpClient = null;
        public AdminConteudoController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ENDPOINT);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var responseTask = await _httpClient.GetAsync($"{ENDPOINT}/Conteudo/Conteudos");
                if(responseTask.IsSuccessStatusCode)
                {
                    var result = responseTask.Content;
                    var readResult = await result.ReadAsStringAsync();
                    var conteudos = JsonConvert.DeserializeObject<List<ConteudoModel>>(readResult);
                   
                    if (conteudos == null || conteudos.Count == 0)
                        throw new Exception("Nenhum conteudo encontrado.");

                    List<ConteudoViewModel> conteudoView = new List<ConteudoViewModel>();
                    foreach(var item in conteudos)
                    {
                        CategoriaConteudoModel categoria = item.CategoriaConteudoModel;

                        var i = new ConteudoViewModel()
                        {
                            Conteudo = new ConteudoModel()
                            {
                                Id = item.Id,
                                Nome = item.Nome,
                                Titulo = item.Titulo,
                                Conteudo = item.Conteudo,
                                CategoriaConteudoId = item.CategoriaConteudoId,
                                CategoriaConteudoModel = item.CategoriaConteudoModel
                            },
                            CategoriaConteudo = categoria
                        };
                        conteudoView.Add(i);
                    }

                    if(conteudoView == null || conteudoView.Count == 0)
                        throw new Exception("Nenhum conteudo encontrado.");

                    return View(conteudoView);
                }
                throw new Exception("Nenhum conteudo encontrado.");
            }
            catch (Exception e)
            {
                ViewData["Erro"] = e;
                return View();
            }
        }
    }
}
