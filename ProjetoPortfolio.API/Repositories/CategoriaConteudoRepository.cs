using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Repositories
{
    public class CategoriaConteudoRepository : ICategoriaConteudoRepository
    {
        private readonly PortfolioDbContext _dbContext;
        public CategoriaConteudoRepository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
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

            CategoriaConteudoModel categoria = await _dbContext.CategoriaConteudo.FirstOrDefaultAsync(x => x.CategoriaConteudoId == id);

            if (categoria == null)
                return null;

            return categoria;
        }
        public async Task<CategoriaConteudoModel> Adicionar(CategoriaConteudoModel categoria)
        {
            if (categoria == null)
                return null;

            await _dbContext.CategoriaConteudo.AddAsync(categoria);
            await _dbContext.SaveChangesAsync();

            return categoria;
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

        public async Task<CategoriaConteudoModel> Atualizar(CategoriaConteudoModel categoria)
        {
            if (categoria == null)
                return null;

            CategoriaConteudoModel categoriaRequest = await BuscarPorId(categoria.CategoriaConteudoId);

            if (categoriaRequest == null)
                return null;

            categoriaRequest.Nome = categoria.Nome;
            categoriaRequest.Descricao= categoria.Descricao;

            _dbContext.CategoriaConteudo.Update(categoriaRequest);

            await _dbContext.SaveChangesAsync();

            return categoriaRequest;
        }
    }
}
