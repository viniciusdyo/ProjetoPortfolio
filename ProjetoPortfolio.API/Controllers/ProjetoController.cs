using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs.Response;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly IProjetoRepository _projetoRepository;

        public ProjetoController(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        [HttpGet("Projetos")]
        public async Task<ActionResult> ListarTodosProjetos()
        {
            try
            {
                PorfolioResponse<ProjetoResponse> portfolioResponse = await _projetoRepository.BuscarTodosProjetos();

                if (portfolioResponse == null)
                    throw new Exception("Response inválida");


                if (!portfolioResponse.Results.Any())
                    throw new Exception("Nenhum projeto encontrado");


                if (portfolioResponse.Errors.Any())
                    throw new Exception(portfolioResponse.Errors.First());

                    return Ok(portfolioResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            try
            {
                PorfolioResponse<ProjetoResponse> portfolioResponse = await _projetoRepository.BuscarPorId(id);
                if (portfolioResponse == null)
                    throw new Exception("Response inválida");


                if (!portfolioResponse.Results.Any())
                    throw new Exception("Nenhum projeto encontrado");


                if (portfolioResponse.Errors.Any())
                    throw new Exception(portfolioResponse.Errors.First());

                if (portfolioResponse.Results.First() == null)
                    throw new Exception("Erro no servidor");

                return Ok(portfolioResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("Cadastrar")]
        public async Task<ActionResult> Cadastrar([FromBody] ProjetoModel projeto )
        {
            try
            {
                if (projeto == null)
                    throw new Exception("Projeto inválido");

                Guid id = new Guid();
                projeto.Id = id;
                projeto.Excluido = false;

                PorfolioResponse<ProjetoResponse> projetoAdicionado = await _projetoRepository.Adicionar(projeto);
                if (projetoAdicionado == null)
                    throw new Exception("Erro no servidor");

                if (projetoAdicionado.Errors.Any())
                    throw new Exception(projetoAdicionado.Errors.First());

                
                return Ok(projeto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Atualizar")]

        public async Task<ActionResult> Atualizar([FromBody] ProjetoModel projetoModel)
        {
            try
            {
                if (projetoModel != null && projetoModel.Id != Guid.Empty)
                {

                    PorfolioResponse<ProjetoResponse> portfolioResponse = await _projetoRepository.Atualizar(projetoModel, projetoModel.Id);
                    if (portfolioResponse == null)
                        throw new Exception("Erro no servidor.");

                    if(portfolioResponse.Errors.Any())
                        throw new Exception(portfolioResponse.Errors.First());

                    return Ok(portfolioResponse);
                }
                throw new Exception("Erro no servidor");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Apagar")]

        public async Task<ActionResult> Apagar([FromBody] Guid id)
        {
            try
            {
                PorfolioResponse<ProjetoResponse> portfolioResponse = await _projetoRepository.Apagar(id);
                if (portfolioResponse == null)
                    throw new Exception("Erro no servidor.");

                if (portfolioResponse.Errors.Any())
                    throw new Exception(portfolioResponse.Errors.First());

                return Ok(portfolioResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
