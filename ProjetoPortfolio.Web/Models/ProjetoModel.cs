using ProjetoPortfolio.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ProjetoPortfolio.API.Models
{
    public class ProjetoModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? UrlImagem { get; set; }
        public string? UrlRedirecionar { get; set; }
        public StatusProjeto Status { get; set; }
        public bool Excluido { get; set; }

        [NotMapped]
        public string TituloNormalizado { get { return NormalizarTitulo(Titulo, Id); } set { } }

        public static string NormalizarTitulo(string titulo, Guid id)
        {

            string idString = id.ToString();

            if (titulo.Length > 0)
            {
                string t = titulo.Trim().Replace(" ", "").ToLower();
                Regex.Replace(t, "[^0-9a-zA-Z]+", "");
                string tn = t;
                if (tn.Length > 4)
                {
                    tn = t.Remove(3);
                }

                return tn + idString[0] + idString[1] + idString[2];

            }
            throw new Exception("titulo inválido");

        }

        public ProjetoModel()
        {

        }

        public ProjetoModel(ProjetoModel projeto)
        {
            Id = projeto.Id;
            Titulo = projeto.Titulo;
            Descricao = projeto.Descricao;
            UrlImagem = projeto.UrlImagem;
            UrlRedirecionar = projeto.UrlRedirecionar;
            Status = projeto.Status;
            Excluido = projeto.Excluido;
            TituloNormalizado = NormalizarTitulo(projeto.Titulo, projeto.Id);
        }
    }
}
