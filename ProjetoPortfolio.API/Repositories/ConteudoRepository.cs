using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Repositories
{

    public class ConteudoRepository : IConteudoRepository
    {
        private readonly PortfolioDbContext _dbContext;
        public ConteudoRepository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ConteudoModel> BuscarPorId(Guid id)
        {
            var conteudo = await _dbContext.Conteudo.FirstOrDefaultAsync(x => x.Id == id);

            if (conteudo == null)
                return null;

            return conteudo;

        }

        public async Task<List<ConteudoModel>> Listar()
        {
            List<ConteudoModel> conteudos = await _dbContext.Conteudo.ToListAsync();
            if (conteudos.Count == 0 || conteudos == null)
                return null;

            return conteudos;
        }

        public async Task<ConteudoModel> Adicionar(ConteudoModel conteudo)
        {
            if (conteudo == null)
                return null;

            await _dbContext.Conteudo.AddAsync(conteudo);
            await _dbContext.SaveChangesAsync();
            return conteudo;
        }

        public async Task<ConteudoModel> Atualizar(ConteudoModel conteudo)
        {
            if (conteudo == null)
                return null;

            ConteudoModel conteudoRequest = await BuscarPorId(conteudo.Id);
            conteudoRequest.Titulo = conteudo.Titulo;
            conteudoRequest.Conteudo = conteudo.Conteudo;
            conteudoRequest.CategoriaConteudoModel = conteudo.CategoriaConteudoModel;
            conteudoRequest.CategoriaConteudoId = conteudo.CategoriaConteudoId;
            conteudoRequest.Nome = conteudo.Nome;

            _dbContext.Update(conteudoRequest);
            await _dbContext.SaveChangesAsync();

            return conteudoRequest;
        }

        public async Task<ConteudoModel> Excluir(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            ConteudoModel conteudo = await BuscarPorId(id);
            _dbContext.Conteudo.Remove(conteudo);
            await _dbContext.SaveChangesAsync();
            return conteudo;
        }
    }
}
