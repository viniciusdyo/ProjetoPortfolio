using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.Web.Domain;
using ProjetoPortfolio.Web.Models;
using ProjetoPortfolio.Web.Models.Response;

namespace ProjetoPortfolio.Web.Controllers
{
    public class ContatosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnviarEmail([FromBody] EmailMensagemModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Model");

                Request<EmailMensagemModel> request = new();
                var response = await request.Cadastrar("Email/EnviarEmail", model);

                if (response == null) 
                    throw new ArgumentNullException(nameof(response));

                if (response.Errors.Any())
                    throw new Exception(response.Errors.First());

                if(!response.Results.Any())
                    throw new ArgumentNullException($"{nameof(response)} results");

                return RedirectToAction("Index", response.Results);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", ex.Message);
            }

        }
    }
}
