using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Repositories;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IRegistroRepository _registroRepository;
        private readonly ILoginRepository _loginRepository;
        public AutenticacaoController(IRegistroRepository registroRepository, ILoginRepository loginRepository)
        {
            _registroRepository = registroRepository;
            _loginRepository = loginRepository;
        }

        [HttpPost]
        [Route("Registrar")]

        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroRequestDto request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AutenticacaoResult result = await _registroRepository.Registrar(request);
                    if (result.Result)
                        return Ok(result);

                    return BadRequest(result);
                }
                return BadRequest(new AutenticacaoResult()
                {
                    Result = false,
                    Errors = new List<string>() { "Erro no servidor" }
                });
            }
            catch (Exception e)
            {
                return BadRequest(new AutenticacaoResult()
                {
                    Result = false,
                    Errors = new List<string> { e.Message }
                });
            }

        }
        [HttpPost]

        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequestDto request)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    AutenticacaoResult result = await _loginRepository.Login(request);
                    if (result.Result)
                        return Ok(result);

                    return BadRequest(result);
                }
                throw new Exception("Erro de servidor");
            }
            catch (Exception e)
            {
                return BadRequest(new AutenticacaoResult()
                {
                    Result = false,
                    Errors = new List<string> 
                    {
                        e.Message
                    }
                });
            }
        }
    }
}
