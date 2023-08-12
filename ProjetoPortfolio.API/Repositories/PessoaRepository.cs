using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs.Response;
using ProjetoPortfolio.API.Repositories.Interfaces;

namespace ProjetoPortfolio.API.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly PortfolioDbContext _dbContext;
        public PessoaRepository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PorfolioResponse<PessoaResponse>> Listar()
        {
            try
            {
                var pessoas = await _dbContext.Pessoas.Select(x => new PessoaResponse
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Sobrenome = x.Sobrenome,
                    Habilidades = x.Habilidades.Select(h => new HabilidadeResponse
                    {
                        Id = h.Id,
                        Descricao = h.Descricao,
                        Nivel = h.Nivel,
                        Nome = h.Nome,
                        PessoaId = h.PessoaId
                    })
                }).ToListAsync();

                if (pessoas != null && pessoas.Any())
                {
                    var response = new PorfolioResponse<PessoaResponse>(pessoas);
                    return response;
                }
                throw new Exception("Nenhuma pessoa encontrada");
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return erros;
            }
        }
        public async Task<PorfolioResponse<PessoaResponse>> BuscarPorId(Guid id)
        {
            try
            {
                var pessoa = await _dbContext.Pessoas.Select(x => new PessoaResponse
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Habilidades = x.Habilidades.Select(h => new HabilidadeResponse
                    {
                        Id = h.Id,
                        Descricao = h.Descricao,
                        Nivel = h.Nivel,
                        Nome = h.Nome,
                        PessoaId = x.Id,
                    })

                }).ToListAsync();

                if (pessoa != null && pessoa.Any())
                {
                    var response = new PorfolioResponse<PessoaResponse>(pessoa);
                    return response;
                }

                throw new Exception("Pessoa não encontrada");
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return erros;
            }
        }
        public async Task<PorfolioResponse<PessoaResponse>> Atualizar(Guid id, PessoaResponse pessoa)
        {
            try
            {
                if (Guid.Empty == id) throw new Exception("Id inválido");
                if (pessoa == null) throw new Exception("Dados inválidos");

                var buscarPessoa = await BuscarPorId(id);

                if (buscarPessoa == null) throw new Exception("Erro no servidor");

                if (buscarPessoa.Errors.Count() == 0)
                {
                    var pessoaResponse = buscarPessoa.Results.FirstOrDefault();
                    if (pessoaResponse != null)
                    {
                        var habilidades = new List<Habilidade>();
                        List<Habilidade> habilidadesResponse = new();

                        foreach (var item in pessoaResponse.Habilidades)
                        {
                            var habilidade = await _dbContext.Habilidades.FirstOrDefaultAsync(x => x.Id == item.Id);

                            if (habilidade != null)
                            {
                                habilidades.Add(habilidade);
                            }
                        };

                        foreach(var item in habilidades)
                        {
                            var habilidadeResponse = pessoa.Habilidades.Select(x => x.Id == item.Id).FirstOrDefault();
                            if (!habilidadeResponse)
                            {
                                _dbContext.Habilidades.Remove(item);
                                await _dbContext.SaveChangesAsync();
                            }
                        }

                        foreach(var item in pessoa.Habilidades)
                        {
                            var habilidade = await _dbContext.Habilidades.FirstOrDefaultAsync(x => x.Id == item.Id);

                            if(habilidade != null)
                            {
                                habilidade.Nome = item.Nome;
                                habilidade.Descricao = item.Descricao;
                                habilidade.Nivel= item.Nivel;
                                habilidade.PessoaId = id;
                            } else
                            {
                                habilidade = new Habilidade
                                {
                                    Id = Guid.NewGuid(),
                                    Nome = item.Nome,
                                    Descricao = item.Descricao,
                                    Nivel = item.Nivel,
                                    PessoaId = id,
                                };
                                await _dbContext.Habilidades.AddAsync(habilidade);
                                await _dbContext.SaveChangesAsync();
                            }
                            habilidadesResponse.Add(habilidade);
                        }

                        var pessoaAtualizada = _dbContext.Pessoas.Update(new PessoaPortfolio
                        {
                            Id = id,
                            Nome = pessoa.Nome,
                            Sobrenome = pessoa.Sobrenome,
                            Habilidades = habilidadesResponse
                        });

                        if (pessoaAtualizada != null)
                        {
                            await _dbContext.SaveChangesAsync();
                            return new PorfolioResponse<PessoaResponse>();
                        }
                    }
                }
                throw new Exception("Erro no servidor");
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return erros;
            }
        }
        public async Task<PorfolioResponse<PessoaResponse>> Cadastrar(PessoaResponse pessoa)
        {
            try
            {
                PessoaPortfolio pessoaModel = new()
                {
                    Id = Guid.NewGuid(),
                    Nome = pessoa.Nome,
                    Sobrenome = pessoa.Sobrenome,
                };

                List<Habilidade> habilidades = new();
                

                foreach (var item in pessoa.Habilidades)
                {
                    var habiidade = new Habilidade
                    {
                        Id = Guid.NewGuid(),
                        Nome = item.Nome,
                        Descricao = item.Descricao,
                        Nivel = item.Nivel,
                        PessoaId = pessoaModel.Id,
                    };
                    habilidades.Add(habiidade);
                }
                pessoaModel.Habilidades = habilidades;


                var pessoaCadastrada = await _dbContext.Pessoas.AddAsync(pessoaModel);


                if (pessoaCadastrada != null)
                {
                    await _dbContext.SaveChangesAsync();
                    return new PorfolioResponse<PessoaResponse>();
                }

                throw new Exception("Erro no servidor");
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return erros;
            }
        }
        public async Task<PorfolioResponse<PessoaResponse>> Remover(Guid id)
        {
            try
            {
                if (id == Guid.Empty) throw new Exception("Id inválido");

                var buscarPessoa = await _dbContext.Pessoas.FirstOrDefaultAsync(x => x.Id == id);

                if (buscarPessoa == null) throw new Exception("Pessoa não encontrada");
                _dbContext.Remove(buscarPessoa);
                await _dbContext.SaveChangesAsync();
                return new PorfolioResponse<PessoaResponse>();
            }
            catch (Exception e)
            {
                var erros = new PorfolioResponse<PessoaResponse>();
                erros.Errors = new() { e.Message };
                return erros;
            }
        }

    }
}
