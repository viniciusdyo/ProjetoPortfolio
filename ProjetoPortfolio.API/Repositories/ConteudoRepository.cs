using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Data;
using ProjetoPortfolio.API.Models;
using ProjetoPortfolio.API.Models.DTOs;
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

            var ativos = new List<AtivoConteudoModel>();
            var categoria = await _dbContext.CategoriaConteudo.FirstOrDefaultAsync(x => x.CategoriaId == conteudo.CategoriaId);

            if (categoria == null)
                categoria = new CategoriaConteudoModel();

            foreach (var item in conteudo.AtivoConteudoModels)
            {
                var ativo = await _dbContext.Ativos.FirstOrDefaultAsync(x => x.AtivoId == item.AtivoId);
                if (ativo != null)
                    ativos.Add(ativo);
            }
            var conteudoResponse = new ConteudoModel()
            {
                Id = conteudo.Id,
                Nome = conteudo.Nome,
                Titulo = conteudo.Titulo,
                Conteudo = conteudo.Conteudo,
                CategoriaId = conteudo.CategoriaId,
                CategoriaConteudoModel = categoria,
                AtivoConteudoModels = ativos,

            };

            if (conteudoResponse == null)
                return null;

            return conteudoResponse;

        }

        public async Task<List<ConteudoModel>> Listar()
        {
            List<ConteudoModel> conteudosResponse = await _dbContext.Conteudo.ToListAsync();
            if (conteudosResponse.Count == 0 || conteudosResponse == null)
                return null;

            return conteudosResponse;
        }

        public async Task<bool> Adicionar(ConteudoDto conteudo, List<AtivoConteudoDto> ativos)
        {
            if (conteudo == null)
                return false;

            var categoria = await _dbContext.CategoriaConteudo.FirstOrDefaultAsync(x => x.CategoriaId == conteudo.CategoriaId);
            if (categoria == null)
                return false;

            ConteudoModel conteudoResponse = new()
            {
                Id = conteudo.Id,
                Nome = conteudo.Nome,
                Titulo = conteudo.Titulo,
                Conteudo = conteudo.Conteudo,
                CategoriaId = conteudo.CategoriaId,
            };

            List<AtivoConteudoModel> ativosResponse = new();
            foreach (var item in ativos)
            {
                var ativo = new AtivoConteudoModel
                {
                    AtivoId = item.AtivoId,
                    Nome = item.Nome,
                    Valor = item.Valor,
                    Descricao = item.Descricao,
                    TipoAtivo = item.TipoAtivo,
                    ConteudoModelId = item.ConteudoModelId,
                };
                ativosResponse.Add(ativo);
            }

            if (ativosResponse.Count > 0)
                conteudoResponse.AtivoConteudoModels = ativosResponse;

            if (conteudoResponse == null)
                return false;
            try
            {
                await _dbContext.Conteudo.AddAsync(conteudoResponse);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public async Task<ConteudoModel> Atualizar(ConteudoDto conteudoRequest, List<AtivoConteudoDto> ativos)
        {
            if (conteudoRequest == null)
                return null;

            var conteudo = await BuscarPorId(conteudoRequest.Id);
            conteudo.Titulo = conteudoRequest.Titulo;
            conteudo.Nome = conteudoRequest.Nome;
            conteudo.Conteudo = conteudoRequest.Conteudo;
            conteudo.CategoriaId = conteudoRequest.CategoriaId;



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
