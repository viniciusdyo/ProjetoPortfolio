﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaConteudoController : ControllerBase
    {
        private readonly ICategoriaConteudoRepository _categoriaConteudoRepository;
        public CategoriaConteudoController(ICategoriaConteudoRepository categoriaConteudoRepository)
        {
            _categoriaConteudoRepository = categoriaConteudoRepository;
        }

        [HttpGet("Categorias")]
        public async Task<ActionResult<List<CategoriaConteudoModel>>> Listar()
        {
            try
            {
                List<CategoriaConteudoModel> categorias = await _categoriaConteudoRepository.Listar();

                if (categorias.Count == 0 || categorias == null) throw new Exception("Nenhuma categoria encontrada.");

                return Ok(categorias);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut("Adicionar")]

        public async Task<ActionResult<CategoriaConteudoModel>> Adicionar(CategoriaConteudoModel categoria)
        {
            try
            {
                if (categoria == null) throw new Exception("Valor de categoria inválido");

                categoria.CategoriaConteudoId = Guid.NewGuid();
                CategoriaConteudoModel categoriaResponse = await _categoriaConteudoRepository.Adicionar(categoria);

                if (categoriaResponse == null) throw new Exception("Falha ao cadastrar.");

                return Ok(categoriaResponse);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut("Editar")]
        public async Task<ActionResult<CategoriaConteudoModel>> Editar(CategoriaConteudoModel categoria)
        {
            try
            {
                if (categoria == null) throw new Exception("Valor de categoria inválido.");

                CategoriaConteudoModel categoriaResponse = await _categoriaConteudoRepository.Atualizar(categoria);

                if (categoriaResponse == null) throw new Exception("Falha ao editar.");

                return Ok(categoriaResponse);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Excluir")]

        public async Task<ActionResult<CategoriaConteudoModel>> Excluir(Guid id)
        {
            try
            {
                if (Guid.Empty == id || id == null) throw new Exception("Id inválido");

                CategoriaConteudoModel categoriaResponse = await _categoriaConteudoRepository.Excluir(id);

                if (categoriaResponse == null) throw new Exception("Erro ao excluir");

                return Ok(categoriaResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
