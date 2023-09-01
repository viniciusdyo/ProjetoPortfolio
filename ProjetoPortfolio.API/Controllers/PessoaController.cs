using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models.DTOs.Response;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaRepository _pessoaRepository;
        public PessoaController(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }

        [HttpGet("Pessoas")]
        public async Task<ActionResult> Listar()
        {
            try
            {
                var pessoas = await _pessoaRepository.Listar();

                if (pessoas == null) throw new Exception("Erro no servidor");
                if (pessoas.Errors.Count() > 0) throw new Exception(pessoas.Errors.FirstOrDefault());

                return Ok(pessoas);
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var pessoa = await _pessoaRepository.BuscarPorId(id);

                if (pessoa == null) throw new Exception("Erro no servidor");
                if (pessoa.Errors.Count() > 0) throw new Exception(pessoa.Errors.FirstOrDefault());

                return Ok(pessoa);
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }

        [HttpPut("Editar/{id}")]

        public async Task<ActionResult> Atualizar(Guid id, [FromBody] PessoaResponse pessoa)
        {
            try
            {
                var atualizarPessoa = await _pessoaRepository.Atualizar(id, pessoa);
                if (atualizarPessoa == null) throw new Exception("Erro no servidor");
                if (atualizarPessoa.Errors.Count() > 0) throw new Exception(atualizarPessoa.Errors.FirstOrDefault());

                return Ok(atualizarPessoa);
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }

        [HttpPost("Cadastrar")]

        public async Task<ActionResult> Cadastrar(PessoaResponse pessoa)
        {
            try
            {
                var cadastrarPessoa = await _pessoaRepository.Cadastrar(pessoa);
                if (cadastrarPessoa == null) throw new Exception("Erro no servidor");
                if (cadastrarPessoa.Errors.Count() > 0) throw new Exception(cadastrarPessoa.Errors.FirstOrDefault());

                return Ok();
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }

        [HttpDelete("Excluir/{id}")]

        public async Task<ActionResult> Excluir(Guid id)
        {
            try
            {
                var excluirPessoa = await _pessoaRepository.Remover(id);
                if (excluirPessoa == null) throw new Exception("Erro no servidor");
                if (excluirPessoa.Errors.Count() > 0) throw new Exception(excluirPessoa.Errors.FirstOrDefault());

                return Ok();
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }
    }
}
