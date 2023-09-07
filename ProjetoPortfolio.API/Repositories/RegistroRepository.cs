using Microsoft.AspNetCore.Identity;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Repositories
{
    public class RegistroRepository : IRegistroRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public RegistroRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AutenticacaoResult> Registrar(UsuarioRegistroRequestDto request)
        {
            try
            {

                if (request == null) 
                    throw new ArgumentNullException(nameof(request));

                var usuario = await _userManager.FindByEmailAsync(request.Email);
                var userName = await _userManager.FindByNameAsync(request.UserName);

                if (usuario != null && userName != null)
                    throw new Exception("Esse endereço de e-mail já está em uso.");
                

                var novo_usuario = new IdentityUser()
                {
                    Email = request.Email,
                    UserName = request.UserName
                };

                var esta_criado = await _userManager.CreateAsync(novo_usuario, request.Password);

                if (esta_criado.Succeeded)
                {
                    return new AutenticacaoResult()
                    {
                        Result = true,
                    };
                }


                throw new Exception("Erro no servidor");
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
