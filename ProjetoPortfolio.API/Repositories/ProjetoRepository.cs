using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Repositories.Interfaces;
using System.Linq;

namespace ProjetoPortfolio.API.Repositories
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly PortfolioDbContext _dbContext;
        public ProjetoRepository(PortfolioDbContext dbContext)
        { 
            _dbContext = dbContext;
        }
        public async Task<ProjetoModel> BuscarPorId(Guid id)
        {
            ProjetoModel projeto = await _dbContext.Projetos.FirstOrDefaultAsync(x => x.Id == id);
            if(projeto == null)
            {
                throw new Exception("Projeto não encontrado");
            }

            return projeto;
        }

        public async Task<List<ProjetoModel>> BuscarTodosProjetos()
        {

            List<ProjetoModel> projetos = await _dbContext.Projetos.Where(x => !x.Excluido).ToListAsync();

            if (projetos.Count == 0 || projetos == null)
                return null;

            return projetos;
        }

        public async Task<ProjetoModel> Adicionar(ProjetoModel projeto)
        {
            await _dbContext.Projetos.AddAsync(projeto);
            await _dbContext.SaveChangesAsync();
            return projeto;  
        }

        public async Task<ProjetoModel> Atualizar(ProjetoModel projeto, Guid id)
        {
            ProjetoModel projetoPorId = await BuscarPorId(id);

            if(projetoPorId == null)
            {
                throw new Exception($"Projeto não encontrado.");
            }

            projetoPorId.Titulo = projeto.Titulo;
            projetoPorId.Descricao = projeto.Descricao;
            projetoPorId.UrlRedirecionar = projeto.UrlRedirecionar;
            projetoPorId.UrlImagem = projeto.UrlImagem;
            projetoPorId.Status = projeto.Status;
            projetoPorId.Excluido = projeto.Excluido;

            _dbContext.Projetos.Update(projetoPorId);
            await _dbContext.SaveChangesAsync();
            return projetoPorId;
        }

        public async Task<bool> Apagar(Guid id)
        {
            ProjetoModel projetoPorId = await BuscarPorId(id);

            if(projetoPorId == null)
            {
                throw new Exception($"Projeto não encontrado.");
            }
            projetoPorId.Excluido = true;

            ProjetoModel projeto = projetoPorId;

            _dbContext.Projetos.Update(projeto);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        
    }
}
