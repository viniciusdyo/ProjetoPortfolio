using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
using ProjetoPortfolio.API.Models.DTOs.Response;
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

        public async Task<PorfolioResponse<ConteudoResponse>> BuscarPorId(Guid id)
        {
            var erros = new PorfolioResponse<ConteudoResponse>();
            if (Guid.Empty == id)
            {
                erros.Errors = new()
                {
                    "Id inválido"
                };

                return erros;
            }

            var conteudoResponse = await _dbContext.Conteudo.Select(x => new ConteudoResponse
            {
                Id = x.Id,
                Nome = x.Nome,
                Titulo = x.Titulo,
                Conteudo = x.Conteudo,
                CategoriaConteudoModel = x.CategoriaConteudoModel,
                CategoriaId = x.CategoriaId,
                AtivosConteudo = x.AtivoConteudoModels.Select(e => new AtivoResponse
                {
                    AtivoId = e.AtivoId,
                    Descricao = e.Descricao,
                    ConteudoModelId = e.ConteudoModelId,
                    NomeAtivo = e.NomeAtivo,
                    TipoAtivo = e.TipoAtivo,
                    Valor = e.Valor,
                })
            }).Where(x => x.Id == id).ToListAsync();

            if (conteudoResponse == null)
                return new PorfolioResponse<ConteudoResponse>()
                {
                    Errors =
                    {
                        "Conteúdo vazio"
                    }
                };

            return new PorfolioResponse<ConteudoResponse>(conteudoResponse);

        }

        public async Task<PorfolioResponse<ConteudoResponse>> Listar()
        {
            PorfolioResponse<ConteudoResponse> erros = new();
            try
            {
                var conteudoResponse = await _dbContext.Conteudo.Select(x => new ConteudoResponse
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Titulo = x.Titulo,
                    Conteudo = x.Conteudo,
                    CategoriaConteudoModel = x.CategoriaConteudoModel,
                    CategoriaId = x.CategoriaId,
                    AtivosConteudo = x.AtivoConteudoModels.Where(a => a.ConteudoModelId == x.Id).Select(e => new AtivoResponse
                    {
                        AtivoId = e.AtivoId,
                        Descricao = e.Descricao,
                        ConteudoModelId = e.ConteudoModelId,
                        NomeAtivo = e.NomeAtivo,
                        TipoAtivo = e.TipoAtivo,
                        Valor = e.Valor,
                    })
                }).ToListAsync();

                if (conteudoResponse == null)
                {
                    throw new Exception("Nenhum conteúdo encontrado");
                }

                return new PorfolioResponse<ConteudoResponse>(conteudoResponse);
            }
            catch (Exception ex)
            {
                erros.Errors = new() { ex.Message };
                return erros;
            }
        }

        public async Task<PorfolioResponse<ConteudoResponse>> Adicionar(ConteudoDto conteudo, List<AtivoConteudoDto>? ativos)
        {
            PorfolioResponse<ConteudoResponse> erros = new();
            try
            {

                if (conteudo == null)
                {
                    throw new Exception("Campo conteúdo inválido");
                }

                var categoria = await _dbContext.CategoriaConteudo.FirstOrDefaultAsync(x => x.CategoriaId == conteudo.CategoriaId);

                if (categoria == null)
                {
                    throw new Exception("Campo categoria inválido");
                }


                ConteudoModel conteudoResponse = new()
                {
                    Id = conteudo.Id,
                    Nome = conteudo.Nome,
                    Titulo = conteudo.Titulo,
                    Conteudo = conteudo.Conteudo,
                    CategoriaId = conteudo.CategoriaId,
                };
                if (ativos != null)
                {

                    List<AtivoConteudoModel> ativosResponse = new();
                    foreach (var item in ativos)
                    {
                        var ativo = new AtivoConteudoModel
                        {
                            AtivoId = Guid.NewGuid(),
                            NomeAtivo = item.NomeAtivo,
                            Valor = item.Valor,
                            Descricao = item.Descricao,
                            TipoAtivo = item.TipoAtivo,
                            ConteudoModelId = conteudoResponse.Id,
                        };
                        ativosResponse.Add(ativo);
                    }

                    if (ativosResponse.Count > 0)
                        conteudoResponse.AtivoConteudoModels = ativosResponse;
                }

                if (conteudoResponse == null)
                {
                    throw new Exception("Categoria não encontrada");
                }

                await _dbContext.Conteudo.AddAsync(conteudoResponse);
                await _dbContext.SaveChangesAsync();

                var list = new List<ConteudoResponse>()
                {
                    new ConteudoResponse()
                    {
                        Id= conteudoResponse.Id,
                        Nome= conteudoResponse.Nome,
                    }
                };
                var response = new PorfolioResponse<ConteudoResponse>(list);
                return response;

            }
            catch (Exception ex)
            {
                erros.Errors = new()
                {
                    ex.Message
                };
                return erros;
            }
        }

        public async Task<PorfolioResponse<ConteudoResponse>> Atualizar(ConteudoDto conteudoRequest, List<AtivoConteudoDto>? ativos)
        {
            PorfolioResponse<ConteudoResponse> erros = new();
            try
            {
                if (conteudoRequest == null)
                    throw new Exception("Conteúdo inválido");

                var conteudoPorId = await BuscarPorId(conteudoRequest.Id);

                if (conteudoPorId == null)
                    throw new Exception("Erro no servidor");

                if (conteudoPorId.Errors.Count == 0)
                {
                    var conteudoResponse = conteudoPorId.Results.FirstOrDefault();
                    if (conteudoResponse != null)
                    {
                        if (conteudoResponse.Id == conteudoRequest.Id)
                        {
                            List<AtivoConteudoModel> ativosList = new();

                            List<AtivoConteudoModel> ativoResponseList = new();
                            if (conteudoResponse.AtivosConteudo.Any())
                            {
                                foreach (var i in conteudoResponse.AtivosConteudo)
                                {
                                    var ativoConsultaResponse = await _dbContext.Ativos.FirstOrDefaultAsync(x => x.AtivoId == i.AtivoId);
                                    if (ativoConsultaResponse != null)
                                        ativoResponseList.Add(ativoConsultaResponse);
                                }
                            }

                            if (ativoResponseList.Any())
                            {
                                foreach (var a in ativoResponseList)
                                {
                                    var ativoConsultaDto = false;
                                    if (ativos != null && ativos.Any())
                                    {
                                        ativoConsultaDto = ativos.Select(x => x.AtivoId == a.AtivoId).FirstOrDefault();
                                    }

                                    if (!ativoConsultaDto)
                                    {
                                        _dbContext.Ativos.Remove(a);
                                        await _dbContext.SaveChangesAsync();
                                    }
                                }
                            }

                            if (ativos != null && ativos.Any())
                            {
                                foreach (var item in ativos)
                                {
                                    var ativoModel = _dbContext.Ativos.FirstOrDefault(x => x.AtivoId == item.AtivoId);

                                    if (ativoModel != null)
                                    {
                                        ativoModel.AtivoId = ativoModel.AtivoId;
                                        ativoModel.NomeAtivo = item.NomeAtivo;
                                        ativoModel.TipoAtivo = item.TipoAtivo;
                                        ativoModel.Valor = item.Valor;
                                        ativoModel.ConteudoModelId = conteudoResponse.Id;
                                        ativoModel.Descricao = item.Descricao;
                                    }
                                    else
                                    {
                                        ativoModel = new AtivoConteudoModel
                                        {
                                            AtivoId = Guid.NewGuid(),
                                            Valor = item.Valor,
                                            NomeAtivo = item.NomeAtivo,
                                            TipoAtivo = item.TipoAtivo,
                                            ConteudoModelId = conteudoResponse.Id,
                                            Descricao = item.Descricao
                                        };
                                        _dbContext.Ativos.Add(ativoModel);
                                        await _dbContext.SaveChangesAsync();
                                    }
                                    ativosList.Add(ativoModel);
                                }
                            }

                            ConteudoModel conteudo = new()
                            {
                                Id = conteudoRequest.Id,
                                Nome = conteudoRequest.Nome,
                                Titulo = conteudoRequest.Titulo,
                                Conteudo = conteudoRequest.Conteudo,
                                CategoriaId = conteudoRequest.CategoriaId,
                                AtivoConteudoModels = ativosList,
                            };

                            _dbContext.Conteudo.Update(conteudo);
                            await _dbContext.SaveChangesAsync();

                            var listResponse = new List<ConteudoResponse>()
                            {
                                new ConteudoResponse()
                                {
                                    Id = conteudo.Id,
                                    Nome = conteudo.Nome,
                                }
                            };
                            return new PorfolioResponse<ConteudoResponse>(listResponse);
                        }
                    }
                }
                throw new Exception("Erro no servidor");
            }
            catch (Exception ex)
            {
                erros.Errors = new() { ex.Message };
                return erros;
            }
        }

        public async Task<PorfolioResponse<ConteudoResponse>> Excluir(Guid id)
        {
            PorfolioResponse<ConteudoResponse> erros = new();
            try
            {
                if (id == Guid.Empty)
                    throw new Exception("Id vazia");

                var consulta = await BuscarPorId(id);

                if (consulta == null || consulta.Results.Count() == 0 || consulta.Errors.Count() > 0)
                    throw new Exception("Conteúdo não encontrado");

                var conteudo = consulta.Results.FirstOrDefault();
                if (conteudo == null) throw new Exception("Erro no servidor");

                _dbContext.Conteudo.Remove(new ConteudoModel()
                {
                    Id = conteudo.Id,
                    Nome = conteudo.Nome,
                    Conteudo = conteudo.Conteudo,
                    CategoriaId = conteudo.CategoriaId,
                    Titulo = conteudo.Titulo
                });
                await _dbContext.SaveChangesAsync();
                return new PorfolioResponse<ConteudoResponse>();
            }
            catch (Exception ex)
            {
                erros.Errors = new() {
                    ex.Message,
                };
                return erros;
            }

        }
    }
}
