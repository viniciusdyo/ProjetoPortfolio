using Microsoft.AspNetCore.Identity;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Repositories
{
    public class RegistroRepository : IRegistroRepository
    {
        private readonly PortfolioDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public RegistroRepository(PortfolioDbContext context, UserManager<IdentityUser> userManager)
        {
            _context= context;
            _userManager= userManager;  
        }

        public async Task<AutenticacaoResult> Registrar(UsuarioRegistroRequestDto request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));

            var usuario = await _userManager.FindByEmailAsync(request.Email);
            var userName = await _userManager.FindByNameAsync(request.UserName);

            if(usuario != null && userName != null)
            {
                return new AutenticacaoResult()
                { 
                    Result= false,
                    Errors= new List<string>()
                    {
                        "Esse endereço de e-mail já está em uso."
                    }
                };
            }

            var novo_usuario = new IdentityUser()
            {
                Email = request.Email,
                UserName = request.UserName
            };

            var esta_criado = await _userManager.CreateAsync(novo_usuario, request.Password);

            if(esta_criado.Succeeded)
            {
                return new AutenticacaoResult()
                {
                    Result = true,
                };
            }

            return new AutenticacaoResult()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "Erro no servidor."
                }
            };

        }
    }
}
