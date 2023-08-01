using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoPortfolio.Web.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

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

        public async Task<IActionResult> Criar(RegistroViewModel registro)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{ENDPOINT}/Autenticacao/Registrar", registro);

                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
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

        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await httpClient.PostAsJsonAsync($"{ENDPOINT}/Autenticacao", vm);
                    if (response.IsSuccessStatusCode)
                    {
                        var tokenModel = System.Text.Json.JsonSerializer.Deserialize<AutenticacaoResponseModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        JwtSecurityTokenHandler tokenHandler = new();

                        var token = tokenHandler.ReadJwtToken(tokenModel!.Token);

                        var claims = token.Claims.ToList();
                        claims.Add(new Claim("accessToken", tokenModel.Token));

                        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                        var authProps = new AuthenticationProperties()
                        {
                            ExpiresUtc = tokenModel.ExpireDate,
                            IsPersistent = true,
                        };

                        await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProps);

                        return RedirectToAction("Index", "Home");
                    }
                    throw new Exception("Erro no servidor");
                }
                throw new Exception("Erro no servidor");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
