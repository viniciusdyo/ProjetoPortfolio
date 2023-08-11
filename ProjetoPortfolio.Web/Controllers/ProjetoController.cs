using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.Web.Domain;
using ProjetoPortfolio.Web.Models;
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
    }
}
