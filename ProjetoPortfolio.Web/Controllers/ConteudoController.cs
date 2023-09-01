using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.Web.Domain;
using ProjetoPortfolio.Web.Models;
using ProjetoPortfolio.Web.Models.Response;

namespace ProjetoPortfolio.Web.Controllers
{
    public class ConteudoController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListaConteudoHome()
        {
            try
            {
                var request = new Request<ConteudoModel>();
                var response = await request.Listar("Conteudo/Conteudos");

                if (response == null)
                {
                    throw new Exception("Erro no servidor");
                }

                if (response.Errors.Any())
                {
                    throw new Exception(response.Errors.FirstOrDefault());
                }

                var conteudosHome = response.Results.Where(x => x.CategoriaConteudoModel.Nome == "home").ToList();

                if(conteudosHome != null)
                    return new JsonResult(conteudosHome);

                throw new Exception("Nenhum conteúdo encontrado");
                
            }
            catch (Exception e)
            {
                return new JsonResult(e);
            }
        }
    }
}
