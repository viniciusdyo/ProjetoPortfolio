using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.Web.Models;
using System.Net;

namespace ProjetoPortfolio.Web.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly string ENDPOINT = "https://localhost:44318/api";
        private readonly HttpClient httpClient = null;
        public AutenticacaoController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ENDPOINT);
        }
        public IActionResult Registrar()
        {
            return View();
        }

        public async Task<IActionResult> Criar(RegistroViewModel request)
        {
            try
            {
                var result = await httpClient.PostAsJsonAsync($"{ENDPOINT}/Autenticacao/Registrar", request);

                if (result != null)
                {
                    if (result.StatusCode == HttpStatusCode.Created || result.StatusCode == HttpStatusCode.OK)
                    {

                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Autenticacao", "Registrar");

                }

                Console.WriteLine("result");
                return RedirectToAction("Autenticacao", "Registrar");

            }
            catch (Exception)
            {

                Console.WriteLine("result");
                return RedirectToAction("Autenticacao", "Registrar");
                
            }
        }
    }
}
