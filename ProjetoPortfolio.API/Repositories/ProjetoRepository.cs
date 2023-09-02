using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs.Response;
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
        public async Task<PorfolioResponse<ProjetoResponse>> BuscarPorId(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {

                    ProjetoModel? projetoModel = await _dbContext.Projetos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                    if (projetoModel == null)
                    {
                        throw new Exception("Projeto não encontrado");
                    }

                    ProjetoResponse projeto = new(projetoModel); ;


                    PorfolioResponse<ProjetoResponse> response = new()
                    {
                        Results = new List<ProjetoResponse>() { projeto },
                        Errors = new List<string>()
                    };

                    return response;
                }
                throw new Exception("Id inválido");
            }
            catch (Exception e)
            {
                PorfolioResponse<ProjetoResponse> response = new() { Errors = { e.Message } };
                return response;
            }
        }

        public async Task<PorfolioResponse<ProjetoResponse>> BuscarTodosProjetos()
        {
            try
            {
                List<ProjetoModel> projetosModel = await _dbContext.Projetos.Where(x => !x.Excluido).ToListAsync();

                if (projetosModel == null || !projetosModel.Any())
                    throw new Exception("Nenhum projeto encontrado");

                List<ProjetoResponse> projetos = new();

                foreach (var projetoModel in projetosModel)
                {
                    ProjetoResponse projeto = new(projetoModel);
                    projetos.Add(projeto);
                }

                if (projetos.Any())
                {
                    return new PorfolioResponse<ProjetoResponse>()
                    {
                        Results = projetos,
                        Errors = new List<string>()
                    };
                }

                throw new Exception("Erro no servidor");

            }
            catch (Exception e)
            {

                return new PorfolioResponse<ProjetoResponse> { Errors = { e.Message } };
            }
        }

        public async Task<PorfolioResponse<ProjetoResponse>> Adicionar(ProjetoModel projeto)
        {
            try
            {
                if (projeto == null) throw new Exception("Erro no servidor");
                await _dbContext.Projetos.AddAsync(projeto);
                await _dbContext.SaveChangesAsync();
                ProjetoResponse projetoResponse = new(projeto);
                return new PorfolioResponse<ProjetoResponse>()
                {
                    Results = new List<ProjetoResponse>() { projetoResponse },
                    Errors = new List<string>()
                };

            }
            catch (Exception e)
            {

                return new PorfolioResponse<ProjetoResponse>()
                {
                    Errors = { e.Message }
                };
            }

        }

        public async Task<PorfolioResponse<ProjetoResponse>> Atualizar(ProjetoModel projetoModel)
        {
            try
            {
                if (projetoModel != null)
                {
                    _dbContext.Projetos.Update(projetoModel);
                    await _dbContext.SaveChangesAsync();
                    ProjetoResponse projeto = new(projetoModel);


                    return new PorfolioResponse<ProjetoResponse>()
                    {
                        Results = new List<ProjetoResponse>() { projeto },
                        Errors = new List<string>()
                    };

                }

                throw new Exception("Erro no servidor.");

            }
            catch (Exception e)
            {
                return new PorfolioResponse<ProjetoResponse>()
                {
                    Errors = { e.Message }
                };
            }
        }

        public async Task<PorfolioResponse<ProjetoResponse>> Apagar(Guid id)
        {
            try
            {
               ProjetoModel projetoModel = await _dbContext.Projetos.FirstOrDefaultAsync(x => x.Id == id);

                if (projetoModel == null)
                {
                    throw new Exception($"Projeto não encontrado.");
                }

                if (projetoModel.Excluido)
                    throw new Exception("Projeto já excluído");

                projetoModel.Excluido = true;

                _dbContext.Projetos.Update(projetoModel);
                await _dbContext.SaveChangesAsync();

                return new PorfolioResponse<ProjetoResponse>()
                {
                    Errors = new List<string>()
                };
            }
            catch (Exception e)
            {
                return new PorfolioResponse<ProjetoResponse>()
                {
                    Errors = { e.Message }
                };
            }
        }

    }
}
