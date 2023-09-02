using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.Web.Domain;
using ProjetoPortfolio.Web.Models;
using ProjetoPortfolio.Web.Models.ViewModels;

namespace ProjetoPortfolio.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminProjetoController : Controller
    {

        public async Task<IActionResult> Index()
        {
            try
            {

                return View();
            }
            catch (Exception e)
            {
                ViewData["Error"] = e;
                return View();
            }

        }


        [HttpPost]
        public async Task<ProjetoViewModel> Editar([FromBody] ProjetoModel projeto)
        {
            try
            {
                if (projeto != null)
                {
                    var request = new Request<ProjetoModel>();
                    var response = await request.Editar("Projeto/Atualizar", projeto);

                    if (response.Errors.Any())
                        throw new Exception(response.Errors.First());

                    var projetoResponse = response.Results.FirstOrDefault();
                    if (projetoResponse == null)
                        throw new Exception("Erro no servidor.");

                    List<ProjetoModel> result = new();
                    result.Add(projetoResponse);
                    return new ProjetoViewModel()
                    {
                        Projetos = result,
                        Erros = new List<string>()
                    };
                }
                throw new Exception("Projeto inválido");
            }
            catch (Exception e)
            {
                return new ProjetoViewModel()
                {
                    Erros = { e.Message }
                };
            }
        }

        public async Task<ProjetoViewModel> Listar()
        {
            try
            {
                var request = new Request<ProjetoModel>();
                var response = await request.Listar("Projeto/Projetos");
                if (response.Errors.Any())
                    throw new Exception(response.Errors.First());

                if (response.Results.Any())
                    return new ProjetoViewModel()
                    {
                        Projetos = response.Results.ToList()
                    };

                throw new Exception("Nenhum projeto encontrado!");
            }
            catch (Exception e)
            {
                var erros = new ProjetoViewModel()
                {
                    Projetos = new List<ProjetoModel>(),
                    Erros = new List<string> { e.Message }
                };
                return erros;
            }
        }

        [HttpPost]
        public async Task<ProjetoViewModel> Excluir([FromBody] ProjetoModel projeto)
        {
            try
            {
                if (projeto == null)
                    throw new Exception("Projeto inválido");
                if (projeto.Id == Guid.Empty)
                    throw new Exception("Projeto inválido");

                var request = new Request<ProjetoModel>();
                var response = await request.Excluir("Projeto/Apagar", projeto.Id, true);

                if (response.Errors.Any())
                    throw new Exception(response.Errors.First());


                return new ProjetoViewModel()
                {
                    Projetos = new List<ProjetoModel>(),
                    Erros = new List<string>(), 
                };
            }
            catch (Exception e)
            {
                return new ProjetoViewModel()
                {
                    Erros = { e.Message }
                };
            }
        }
        [HttpPost]
        public async Task<ProjetoViewModel> Adicionar([FromBody] ProjetoModel projeto)
        {
            try
            {
                if (projeto == null)
                    throw new Exception("Projeto inválido");

                ProjetoModel projetoModel = new(projeto);
                projetoModel.Id = Guid.Empty;

                var request = new Request<ProjetoModel>();
                var response = await request.Cadastrar("Projeto/Cadastrar", projetoModel);

                if (response.Errors.Any())
                    throw new Exception(response.Errors.First());

                return new ProjetoViewModel()
                {
                    Projetos = new List<ProjetoModel>(),
                    Erros = new List<string>(),
                };
            }
            catch (Exception e)
            {
                return new ProjetoViewModel()
                {
                    Projetos = new List<ProjetoModel>(),
                    Erros = new List<string>()
                    {
                        e.Message
                    },
                };
            }
        }
    }

}
