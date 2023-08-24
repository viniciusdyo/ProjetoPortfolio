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
        public async Task<ActionResult> Listar()
        {
            PorfolioResponse<ConteudoResponse> erros = new();
            try
            {
                PorfolioResponse<ConteudoResponse> conteudos = await _conteudoRepository.Listar();
                if (conteudos == null || conteudos.Results.Count() == 0)
                    throw new Exception("Não há conteudos a serem listados");

                if (conteudos.Errors.Any()) throw new Exception(conteudos.Errors.ToString());

                return Ok(conteudos);
            }
            catch (Exception e)
            {
                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> BuscarPorId(Guid id)
        {
            PorfolioResponse<ConteudoResponse> erros = new();
            try
            {
                PorfolioResponse<ConteudoResponse> conteudoResponse = await _conteudoRepository.BuscarPorId(id);

                if (conteudoResponse.Errors.Any()) throw new Exception(conteudoResponse.Errors.ToString());

                if (conteudoResponse.Results.Count() == 0) throw new Exception("Conteudo nao encontrado");

                return Ok(conteudoResponse);
            }
            catch (Exception e)
            {

                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }

        [HttpPost("CadastrarConteudo")]
        public async Task<ActionResult> Cadastrar([FromBody] ConteudoAtivosViewModel conteudo)
        {
            PorfolioResponse<ConteudoResponse> erros = new();
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
                PorfolioResponse<ConteudoResponse> conteudoResponse = new();
                if (conteudo.Ativos != null && conteudo.Ativos.Count > 0)
                {
                    foreach (var item in conteudo.Ativos)
                    {
                        var ativo = new AtivoConteudoDto()
                        {
                            AtivoId = Guid.NewGuid(),
                            NomeAtivo = item.NomeAtivo,
                            Descricao = item.Descricao,
                            TipoAtivo = item.TipoAtivo,
                            Valor = item.Valor,
                            ConteudoModelId = conteudoRequest.Id,
                        };
                        ativosRequest.Add(ativo);
                    }
                    conteudoResponse = await _conteudoRepository.Adicionar(conteudoRequest, ativosRequest);
                }
                else
                {
                    conteudoResponse = await _conteudoRepository.Adicionar(conteudoRequest);
                }

                if (conteudoResponse.Errors.Any()) throw new Exception(conteudoResponse.Errors.ToString());

                return Ok();
            }
            catch (Exception e)
            {
                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }

        [HttpPut("Editar")]
        public async Task<ActionResult> Editar([FromBody] ConteudoResponse conteudo)
        {
            PorfolioResponse<ConteudoResponse> erros = new();
            try
            {
                if (conteudo == null) throw new Exception("Conteúdo inválido");

                List<AtivoConteudoDto> ativos = new();
                if (conteudo.AtivosConteudo != null && conteudo.AtivosConteudo.Any())
                {
                    foreach (var item in conteudo.AtivosConteudo)
                    {
                        AtivoConteudoDto ativo = new()
                        {
                            AtivoId = item.AtivoId,
                            NomeAtivo = item.NomeAtivo,
                            Descricao = item.Descricao,
                            ConteudoModelId = item.ConteudoModelId,
                            TipoAtivo = item.TipoAtivo,
                            Valor = item.Valor
                        };
                        ativos.Add(ativo);
                    }
                }

                ConteudoDto conteudoDto = new()
                {
                    Id = conteudo.Id,
                    Nome = conteudo.Nome,
                    CategoriaId = conteudo.CategoriaId,
                    Conteudo = conteudo.Conteudo,
                    Titulo = conteudo.Titulo,
                };

                PorfolioResponse<ConteudoResponse> conteudoResponse = await _conteudoRepository.Atualizar(conteudoDto, ativos);

                if (conteudoResponse.Errors.Any()) throw new Exception(conteudoResponse.Errors.ToString());

                if (conteudoResponse == null) throw new Exception("Erro ao atualizar");



                return Ok(conteudoResponse);


            }
            catch (Exception e)
            {

                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }

        [HttpDelete("Remover/{id}")]
        public async Task<ActionResult<ConteudoModel>> Remover(Guid id)
        {
            PorfolioResponse<ConteudoResponse> erros = new();
            try
            {
                if (Guid.Empty == id) throw new Exception("Id incorreto");

                PorfolioResponse<ConteudoResponse> conteudoResponse = await _conteudoRepository.Excluir(id);

                if (conteudoResponse.Errors.Any()) throw new Exception(conteudoResponse.Errors.ToString());

                if (conteudoResponse == null) throw new Exception("Erro ao excluir");

                return Ok(conteudoResponse);
            }
            catch (Exception e)
            {
                erros.Errors = new() { e.Message };
                return BadRequest(erros);
            }
        }
    }
}
