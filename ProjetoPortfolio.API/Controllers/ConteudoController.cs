using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Models.DTOs.Response;
using ProjetoPortfolio.API.Models.ViewModels;
using ProjetoPortfolio.API.Repositories.Interfaces;
using System.Collections;

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
                if (conteudos == null || conteudos.Count == 0)
                    throw new Exception("Não há conteudos a serem listados");

                foreach (var item in conteudos)
                {
                    item.CategoriaConteudoModel = await _categoriaRepository.BuscarPorId(item.CategoriaId);

                    if (item.CategoriaConteudoModel == null)
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
        public async Task<ActionResult> BuscarPorId(Guid id)
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

        [HttpPost("CadastrarConteudo")]
        public async Task<ActionResult> Cadastrar([FromBody] ConteudoAtivosViewModel conteudo)
        {
            try
            {
                if (conteudo == null) throw new Exception("Valores inválidos");

                ConteudoDto conteudoRequest = new()
                {
                    Id = Guid.NewGuid(),
                    Nome = conteudo.Conteudo.Nome,
                    Conteudo = conteudo.Conteudo.Conteudo,
                    Titulo = conteudo.Conteudo.Titulo,
                    CategoriaId = conteudo.Conteudo.CategoriaId
                };

                List<AtivoConteudoDto> ativosRequest = new();

                if (conteudo.Ativos != null && conteudo.Ativos.Count > 0)
                {
                    foreach (var item in conteudo.Ativos)
                    {
                        var ativo = new AtivoConteudoDto()
                        {
                            AtivoId = Guid.NewGuid(),
                            Nome = item.Nome,
                            Descricao = item.Descricao,
                            TipoAtivo = item.TipoAtivo,
                            Valor = item.Valor,
                            ConteudoModelId = conteudoRequest.Id,
                        };
                        ativosRequest.Add(ativo);
                    }
                }

                var conteudoResponse = await _conteudoRepository.Adicionar(conteudoRequest, ativosRequest);

                if (conteudoResponse) throw new Exception("Erro no servidor");

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Editar/{id}")]
        public async Task<ActionResult<ConteudoModel>> Editar(Guid id, [FromBody] ConteudoAtivosViewModel conteudo)
        {
            try
            {
                if (conteudo == null) throw new Exception("Conteúdo inválido");

                if (Guid.Empty == id) throw new Exception("Id inválido");

                ConteudoModel conteudoResponse = await _conteudoRepository.Atualizar(conteudo.Conteudo, conteudo.Ativos);

                if (conteudoResponse == null) throw new Exception("Erro ao atualizar");

                return Ok(conteudoResponse);


            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Remover")]
        public async Task<ActionResult<ConteudoModel>> Remover([FromBody] Guid id)
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
