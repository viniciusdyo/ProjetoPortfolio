using Microsoft.AspNetCore.Identity;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Repositories.Interfaces;
using ProjetoPortfolio.API.Services.Interfaces;

namespace ProjetoPortfolio.API.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        public LoginRepository(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AutenticacaoResult> Login(UsuarioLoginRequestDto request)
        {

            try
            {

                IdentityUser user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    throw new Exception("Credenciais inválidas");
                }

                var verificaSenha = await _userManager.CheckPasswordAsync(user, request.Password);

                if (!verificaSenha)
                {
                    throw new Exception("Credenciais inválidas");
                }

                var result = await _tokenService.GenerateToken(user);

                return result;
            }
            catch (Exception e)
            {
                return new AutenticacaoResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                };
            }

        }
    }
}
