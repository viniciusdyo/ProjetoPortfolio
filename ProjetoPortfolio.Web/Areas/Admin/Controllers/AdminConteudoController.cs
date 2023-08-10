using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                var categoriasResponse = await _httpClient.GetAsync($"{ENDPOINT}/CategoriaConteudo/Categorias");

                if (categoriasResponse.IsSuccessStatusCode)
                {
                    var readCategoriasResult = await categoriasResponse.Content.ReadAsStringAsync();

                    var categorias = JsonConvert.DeserializeObject<List<CategoriaConteudo>>(readCategoriasResult);

                    if (categorias == null || categorias.Count == 0)
                        throw new Exception("Categorias não encontradas");

                    ViewBag.Categorias = categorias;

                    if (responseTask.IsSuccessStatusCode)
                    {
                        var result = responseTask.Content;
                        var readResult = await result.ReadAsStringAsync();
                        var conteudos = JsonConvert.DeserializeObject<List<Conteudo>>(readResult);


                        if (conteudos == null || conteudos.Count == 0)
                            throw new Exception("Nenhum conteudo encontrado.");

                        List<ConteudoViewModel> conteudoView = new();

                        foreach (var item in conteudos)
                        {
                            CategoriaConteudo categoria = item.CategoriaConteudoModel;

                            var i = new ConteudoViewModel()
                            {
                                Conteudo = new Conteudo()
                                {
                                    Id = item.Id,
                                    Nome = item.Nome,
                                    Titulo = item.Titulo,
                                    ConteudoValor = item.ConteudoValor,
                                    CategoriaConteudoId = item.CategoriaConteudoId
                                },
                                CategoriaConteudo = categoria,
                                Categorias = categorias
                            };
                            conteudoView.Add(i);
                        }
                        return View(conteudoView);
                    }
                }
                return View(new List<ConteudoViewModel>());
            }
            catch (Exception e)
            {
                ViewData["Erro"] = e;
                return RedirectToAction("Index", "Admin");
            }
        }

        public async Task<IActionResult> Adicionar(ConteudoViewModel conteudoView)
        {
            try
            {
                if (conteudoView.Conteudo == null)
                    throw new Exception("Conteúdo inválido");

                Conteudo conteudoModel = new()
                {
                    Id = Guid.Empty,
                    Nome = conteudoView.Conteudo.Nome,
                    Titulo = conteudoView.Conteudo.Titulo,
                    ConteudoValor = conteudoView.Conteudo.ConteudoValor,
                    CategoriaConteudoId = conteudoView.Conteudo.CategoriaConteudoId,
                    CategoriaConteudoModel = new CategoriaConteudo()
                    {
                        CategoriaConteudoId = conteudoView.Conteudo.CategoriaConteudoId,
                        Descricao = string.Empty,
                        Nome = string.Empty
                    },
                };

                if (conteudoModel == null)
                    throw new Exception("Conteúdo inválido");

                var responseTask = await _httpClient.PostAsJsonAsync($"{ENDPOINT}/Conteudo/CadastrarConteudo", conteudoModel);

                if (responseTask.IsSuccessStatusCode)
                {
                    RedirectToAction("Index");
                }
                throw new Exception("Erro no servidor");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", e);
            }
        }

        public async Task<IActionResult> Editar(ConteudoViewModel conteudoView)
        {
            try
            {

                if (conteudoView.Conteudo == null) throw new Exception("Conteúdo inválido");
                Conteudo conteudoModel = new()
                {
                    Id = conteudoView.Conteudo.Id,
                    Nome = conteudoView.Conteudo.Nome,
                    Titulo = conteudoView.Conteudo.Titulo,
                    ConteudoValor = conteudoView.Conteudo.ConteudoValor,
                    CategoriaConteudoId = conteudoView.Conteudo.CategoriaConteudoId,
                    CategoriaConteudoModel = new CategoriaConteudo()
                    {
                        CategoriaConteudoId = conteudoView.Conteudo.CategoriaConteudoId,
                        Descricao = string.Empty,
                        Nome = string.Empty
                    },
                };
                var conteudoResponse = await _httpClient.PutAsJsonAsync($"{ENDPOINT}/Conteudo/Editar/{conteudoModel.Id}", conteudoModel);

                if (conteudoResponse.IsSuccessStatusCode)
                    RedirectToAction("Index");


                throw new Exception("Erro no servidor");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", e);
            }
        }
    }
}
