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
        private readonly string ENDPOINT = "https://api.viniciusdyonisio.targetbr.biz/api";
        private readonly HttpClient httpClient;
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
                if (registro == null)
                    throw new Exception("Dados inválidos");

                if (registro.ConfirmPassword != registro.Password)
                    throw new Exception("Senha e senha de confirmação não são iguais.");

                var novoUsuario = new RegistroDto
                {
                    UserName = registro.UserName, Password = registro.Password, Email = registro.Email
                };
                
                var response = await httpClient.PostAsJsonAsync($"{ENDPOINT}/Autenticacao/Registrar", novoUsuario);

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
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

           return RedirectToAction("Index", "Home");
        }
    }
}
