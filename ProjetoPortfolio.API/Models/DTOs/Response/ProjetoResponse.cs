using ProjetoPortfolio.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoPortfolio.API.Models.DTOs.Response
{
    public class ProjetoResponse
    {
        public Guid Id { get; set; }
        [Required]
        public string? Titulo { get; set; }
        [Required]
        public string? Descricao { get; set; }
        public string? UrlImagem { get; set; }
        public string? UrlRedirecionar { get; set; }
        [Required]
        public StatusProjeto Status { get; set; }
        public bool Excluido { get; set; } = false;
        public string TituloNormalizado { get; set; }

        public ProjetoResponse(ProjetoModel projeto)
        {
            Id = projeto.Id;
            Titulo = projeto.Titulo;
            Descricao = projeto.Descricao;
            UrlImagem = projeto.UrlImagem;
            UrlRedirecionar= projeto.UrlRedirecionar;
            Status = projeto.Status;
            Excluido= projeto.Excluido;
            TituloNormalizado = projeto.TituloNormalizado;
            
        }
    }

    
}
