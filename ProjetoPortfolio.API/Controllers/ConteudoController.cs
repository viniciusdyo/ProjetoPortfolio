using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConteudoController : ControllerBase
    {
        private readonly IConteudoRepository _conteudoRepository;
        private readonly ICategoriaConteudoRepository _categoriaRepository;

        public ConteudoController(IConteudoRepository conteudoRepository, ICategoriaConteudoRepository categoriaRepository)
        {
            _conteudoRepository = conteudoRepository;
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet("Conteudos")]
        public async Task<ActionResult<List<ConteudoModel>>> Listar()
        {
            try
            {
                List<ConteudoModel> conteudos = await _conteudoRepository.Listar();
                if (conteudos.Count == 0 || conteudos == null)
                    throw new Exception("Não há conteudos a serem listados");

                foreach(var item in conteudos)
                {
                    item.CategoriaConteudoModel = await _categoriaRepository.BuscarPorId(item.CategoriaConteudoId);

                    if(item.CategoriaConteudoModel == null)
                        throw new Exception("Não há categoria");
                }

                

                return Ok(conteudos);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConteudoModel>> BuscarPorId(Guid id)
        {
            try
            {
                ConteudoModel conteudo = await _conteudoRepository.BuscarPorId(id);

                if (conteudo == null) throw new Exception("Conteudo nao encontrado");
                return Ok(conteudo);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost("Adicionar")]
        public async Task<ActionResult<ConteudoModel>> Adicionar(ConteudoModel conteudo)
        {
            try
            {
                if (conteudo == null) throw new Exception("Conteudo inválido");

                if (conteudo.CategoriaConteudoId == null) throw new Exception("Categoria não encontrada");

                CategoriaConteudoModel categoria = await _categoriaRepository.BuscarPorId(conteudo.CategoriaConteudoId);
                ConteudoModel conteudoRequest = new ConteudoModel()
                {
                    Id = Guid.NewGuid(),
                    CategoriaConteudoId = conteudo.CategoriaConteudoId,
                    CategoriaConteudoModel = categoria,
                    Nome = conteudo.Nome,
                    Conteudo = conteudo.Conteudo,
                    Titulo= conteudo.Titulo,
                };

                ConteudoModel conteudoResponse = await _conteudoRepository.Adicionar(conteudoRequest);

                if (conteudoResponse == null) throw new Exception("Erro no servidor");

                return Ok(conteudoResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Editar")]
        public async Task<ActionResult<ConteudoModel>> Editar(ConteudoModel conteudo)
        {
            try
            {
                if (conteudo == null) throw new Exception("Conteúdo inválido");

                ConteudoModel conteudoResponse = await _conteudoRepository.Atualizar(conteudo);

                if (conteudoResponse == null) throw new Exception("Erro ao atualizar");

                return Ok(conteudoResponse);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Remover")]
        public async Task<ActionResult<ConteudoModel>> Remover(Guid id)
        {
            try
            {
                if (Guid.Empty == id || id == null) throw new Exception("Id incorreto");

                ConteudoModel conteudo = await _conteudoRepository.Excluir(id);

                if (conteudo == null) throw new Exception("Erro ao excluir");

                return Ok(conteudo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
