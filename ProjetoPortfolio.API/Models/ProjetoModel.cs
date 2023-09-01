using ProjetoPortfolio.API.Models.DTOs.Response;
using ProjetoPortfolio.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ProjetoPortfolio.API.Models
{
    public class ProjetoModel
    {
        public Guid Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? UrlImagem { get; set; }
        public string? UrlRedirecionar { get; set; }
        public StatusProjeto Status { get; set; }
        public bool Excluido { get; set; }

        [NotMapped]
        public string TituloNormalizado { get { return NormalizarTitulo(Titulo); } set { } }

        public static string NormalizarTitulo(string titulo)
        {
            string t = titulo.Trim().Replace(" ", "").ToLower();
            Regex.Replace(t, "[^0-9a-zA-Z]+", "");
            return t;

        }
        public ProjetoModel()
        {

        }

        public ProjetoModel(ProjetoResponse projetoResponse)
        {
            Id= projetoResponse.Id;
            Titulo= projetoResponse.Titulo;
            Descricao= projetoResponse.Descricao;
            UrlImagem= projetoResponse.UrlImagem;
            UrlRedirecionar = projetoResponse.UrlRedirecionar;
            Status = projetoResponse.Status;
            Excluido = projetoResponse.Excluido;
        }
    }
}
