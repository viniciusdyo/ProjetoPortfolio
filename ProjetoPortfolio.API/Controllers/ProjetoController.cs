using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models;
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

        [HttpGet]
        public async Task<ActionResult<List<ProjetoModel>>> ListarTodosProjetos()
        { 
            try
            {
                List<ProjetoModel> projetos = await _projetoRepository.BuscarTodosProjetos();
                return Ok(projetos);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjetoModel>> BuscarPorId(Guid id)
        {
            try
            {
                ProjetoModel projeto = await _projetoRepository.BuscarPorId(id);
                return Ok(projeto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<ProjetoModel>> Cadastrar([FromBody] ProjetoModel projeto)
        {
            try
            {
                Guid id = new Guid();
                projeto.Id = id;
                ProjetoModel projetoAdicionado = await _projetoRepository.Adicionar(projeto);
                return Ok(projeto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]

        public async Task<ActionResult<ProjetoModel>> Atualizar([FromBody] ProjetoModel projetoModel, Guid id)
        {
            try
            {
                projetoModel.Id = id;
                ProjetoModel projeto = await _projetoRepository.Atualizar(projetoModel, id);
                return Ok(projeto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Apagar/")]

        public async Task<ActionResult<ProjetoModel>> Apagar([FromBody] ProjetoModel projetoModel, Guid id)
        {
            try
            {
                bool excluido = await _projetoRepository.Apagar(id);
                return Ok(excluido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
