using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NuGet.Protocol;
using ProjetoPortfolio.Entities.Enums;
using ProjetoPortfolio.Web.Domain;
using ProjetoPortfolio.Web.Models;
using ProjetoPortfolio.Web.Models.ViewModels;

namespace ProjetoPortfolio.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminConteudoController : Controller
    {

        public async Task<IActionResult> Index()
        {
            try
            {
                var request = new Request<ConteudoModel>();
                var response = await request.Listar("Conteudo/Conteudos");

                if (response.Errors.Count() == 0)
                {
                    var conteudos = new List<ConteudoModel>(response.Results);

                    var requestCategoria = new Request<CategoriaConteudo>();
                    var responseCategorias = await requestCategoria.Listar("CategoriaConteudo/Categorias");

                    if (responseCategorias.Errors.Count() > 0)
                        throw new Exception("Categorias não encontradas");

                    var categorias = new List<CategoriaConteudo>(responseCategorias.Results);

                    ViewBag.Categorias = categorias;

                    if (conteudos == null || conteudos.Count == 0)
                        throw new Exception("Nenhum conteudo encontrado.");

                    List<ConteudoViewModel> conteudoView = new();

                    foreach (var item in conteudos)
                    {
                        CategoriaConteudo categoria = item.CategoriaConteudoModel;

                        var i = new ConteudoViewModel()
                        {
                            Conteudo = new ConteudoModel()
                            {
                                Id = item.Id,
                                Nome = item.Nome,
                                Titulo = item.Titulo,
                                Conteudo = item.Conteudo,
                                CategoriaId = item.CategoriaId
                            },
                            CategoriaConteudo = categoria,
                            Categorias = categorias
                        };
                        conteudoView.Add(i);
                    }
                    return View(conteudoView);
                }

                return View(new List<ConteudoViewModel>());
            }
            catch (Exception e)
            {
                ViewData["Erro"] = e;
                return RedirectToAction("Index", "Admin");
            }
        }

        public async Task<IActionResult> Listar()
        {
            try
            {
                var request = new Request<ConteudoModel>();
                var response = await request.Listar("Conteudo/Conteudos");
                if (response.Errors.Count() == 0)
                {
                    return new JsonResult(response.Results);
                }

                throw new Exception(response.Errors[0]);
            }
            catch (Exception e)
            {
                ViewData["Erro"] = e;
                return RedirectToAction("Index", "Admin");
            }
        }

        public async Task<IActionResult> ListarConteudoAdmin()
        {
            try
            {
                var requestConteudo = new Request<ConteudoModel>();
                var responseConteudo = await requestConteudo.Listar("Conteudo/Conteudos");
                if (responseConteudo.Errors.Count() == 0)
                {
                    var requestCategoria = new Request<CategoriaConteudo>();
                    var responseCategoria = await requestCategoria.Listar("CategoriaConteudo/Categorias");

                    if (responseCategoria.Errors.Count() == 0)
                    {
                        var conteudo = responseConteudo.Results.FirstOrDefault();

                        if (conteudo == null) throw new Exception("Conteúdo não encontrado");

                        var conteudoViewModel = new ConteudoViewModel
                        {
                            Categorias = new List<CategoriaConteudo>(responseCategoria.Results),
                            Conteudos = new List<ConteudoModel>(responseConteudo.Results)
                        };

                        return new JsonResult(conteudoViewModel);
                    }
                    throw new Exception(responseCategoria.Errors.FirstOrDefault());
                }
                throw new Exception(responseConteudo.Errors.FirstOrDefault());
            }
            catch (Exception e)
            {
                return new JsonResult(e);
            }
        }

        public async Task<IActionResult> Adicionar(ConteudoViewModel conteudoView)
        {
            try
            {
                if (conteudoView.Conteudo == null)
                    throw new Exception("Conteúdo inválido");

                ConteudoModel conteudoModel = new()
                {
                    Id = Guid.Empty,
                    Nome = conteudoView.Conteudo.Nome,
                    Titulo = conteudoView.Conteudo.Titulo,
                    Conteudo = conteudoView.Conteudo.Conteudo,
                    CategoriaId = conteudoView.Conteudo.CategoriaId,
                    AtivosConteudo = conteudoView.Conteudo.AtivosConteudo,
                    CategoriaConteudoModel = new CategoriaConteudo()
                    {
                        CategoriaId = conteudoView.Conteudo.CategoriaId,
                        Descricao = string.Empty,
                        Nome = string.Empty
                    },
                };

                var content = new ConteudoAtivosViewModel()
                {
                    Conteudo = conteudoModel,
                    Ativos = new List<AtivoConteudo> { new AtivoConteudo
                    {
                        Nome = "eae",
                        Descricao = "eaeae",
                        Valor = "eaeaeae",
                        TipoAtivo = TipoAtivo.Link,
                    } }
                };



                if (conteudoModel == null)
                    throw new Exception("Conteúdo inválido");

                var request = new Request<ConteudoAtivosViewModel>();
                var response = await request.Cadastrar("Conteudo/CadastrarConteudo", content);

                if (response.Errors.Count == 0)
                    return RedirectToAction("Index");

                throw new Exception(response.Errors.FirstOrDefault()?.ToString());
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
                ConteudoModel conteudoModel = new()
                {
                    Id = conteudoView.Conteudo.Id,
                    Nome = conteudoView.Conteudo.Nome,
                    Titulo = conteudoView.Conteudo.Titulo,
                    Conteudo = conteudoView.Conteudo.Conteudo,
                    CategoriaId = conteudoView.Conteudo.CategoriaId,
                    CategoriaConteudoModel = new CategoriaConteudo()
                    {
                        CategoriaId = conteudoView.Conteudo.CategoriaId,
                        Descricao = string.Empty,
                        Nome = string.Empty
                    },
                };



                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                return RedirectToAction("Index", e);
            }
        }
    }
}
