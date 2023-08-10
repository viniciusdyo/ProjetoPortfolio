using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Repositories
{
    public class CategoriaConteudoRepository : ICategoriaConteudoRepository
    {
        private readonly PortfolioDbContext _dbContext;
        private readonly IConteudoRepository _conteudoRepository;
        public CategoriaConteudoRepository(PortfolioDbContext dbContext, IConteudoRepository conteudoRepository)
        {
            _dbContext = dbContext;
            _conteudoRepository = conteudoRepository;
        }
        public async Task<List<CategoriaConteudoModel>> Listar()
        {
            List<CategoriaConteudoModel> categorias = await _dbContext.CategoriaConteudo.ToListAsync();
            if (categorias.Count == 0)
                return null;

            return categorias;
        }
        public async Task<CategoriaConteudoModel> BuscarPorId(Guid id)
        {
            if (Guid.Empty == id || id == null)
                return null;

            var categoriaResponse = await _dbContext.CategoriaConteudo.Include(x => x.ConteudoModels).FirstOrDefaultAsync(x => x.CategoriaId == id);

            if (categoriaResponse == null)
                return null;

            CategoriaConteudoModel categoria = new()
            {
                CategoriaId = categoriaResponse.CategoriaId,
                Nome = categoriaResponse.Nome,
                Descricao= categoriaResponse.Descricao,
                ConteudoModels= categoriaResponse.ConteudoModels,
            };

            if (categoria == null)
                return null;

            return categoria;
        }
        public async Task<CategoriaConteudoModel> Adicionar(CategoriaConteudoDto categoria)
        {
            if (categoria == null)
                return null;

            CategoriaConteudoModel categoriaModel = new()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao
            };

            await _dbContext.CategoriaConteudo.AddAsync(categoriaModel);
            await _dbContext.SaveChangesAsync();

            return categoriaModel;
        }


        public async Task<CategoriaConteudoModel> Excluir(Guid id)
        {
            if (Guid.Empty == id || id == null)
                return null;
            CategoriaConteudoModel categoria = await BuscarPorId(id);
            if (categoria == null)
                return null;

            _dbContext.CategoriaConteudo.Remove(categoria);
            _dbContext.SaveChanges();

            return categoria;
        }

        public async Task<CategoriaConteudoModel> Atualizar(CategoriaConteudoDto categoria)
        {
            if (categoria == null)
                return null;

            CategoriaConteudoModel categoriaResponse = await BuscarPorId(categoria.CategoriaId);

            if (categoriaResponse == null)
                return null;

            categoriaResponse.Nome = categoria.Nome;
            categoriaResponse.Descricao= categoria.Descricao;

            _dbContext.CategoriaConteudo.Update(categoriaResponse);
            await _dbContext.SaveChangesAsync();

            return categoriaResponse;
        }
    }
}
