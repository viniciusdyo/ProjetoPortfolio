using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.Web.Domain;
using ProjetoPortfolio.Web.Models;
using ProjetoPortfolio.Web.Models.Response;
using System.Text.Json.Serialization;

namespace ProjetoPortfolio.Web.Controllers
{
    public class ProjetoController : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            try
            {
                var request = new Request<ProjetoModel>();
                var response = await request.Listar("Projeto");
                if (response == null)
                    throw new Exception("Erro no servidor");

                var projetos = response.Results;


                return View(projetos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PorfolioResponse<ProjetoModel>> Listar()
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
                    Results = projetos,
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
    }
}
