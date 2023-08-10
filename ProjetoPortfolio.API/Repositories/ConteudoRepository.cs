using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Repositories
{

    public class ConteudoRepository : IConteudoRepository
    {
        private readonly PortfolioDbContext _dbContext;
        private readonly ICategoriaConteudoRepository _categoriaConteudoRepository;
        public ConteudoRepository(PortfolioDbContext dbContext, ICategoriaConteudoRepository categoriaConteudoRepository)
        {
            _dbContext = dbContext;
            _categoriaConteudoRepository = categoriaConteudoRepository;
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

            ConteudoModel conteudoRequest = new ConteudoModel();
            conteudoRequest.Titulo = conteudo.Titulo;
            conteudoRequest.Nome = conteudo.Nome;
            conteudoRequest.Conteudo = conteudo.Conteudo;
            conteudoRequest.CategoriaConteudoId = conteudo.CategoriaConteudoId;
            await _dbContext.Conteudo.AddAsync(conteudo);
            await _dbContext.SaveChangesAsync();


            return conteudo;


            throw new Exception();
        }

        public async Task<ConteudoModel> Atualizar(ConteudoModel conteudoRequest)
        {
            if (conteudoRequest == null)
                return null;

            var conteudo = await BuscarPorId(conteudoRequest.Id);
            conteudo.Titulo = conteudoRequest.Titulo;
            conteudo.Nome= conteudoRequest.Nome;
            conteudo.Conteudo = conteudoRequest.Conteudo;
            conteudo.CategoriaConteudoId = conteudoRequest.CategoriaConteudoId;



            if (conteudo != null)
            {
                _dbContext.Conteudo.Update(conteudo);
                await _dbContext.SaveChangesAsync();
                return conteudo;
            }

            return null;
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
