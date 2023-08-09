using System.Text.RegularExpressions;

namespace ProjetoPortfolio.Web.Models
{
    public class ConteudoModel
    {
        public Guid Id { get; set; }
        public string Conteudo { get; set; }
        public string Titulo { get; set; }
        public string Nome { get; set; }
        public Guid CategoriaConteudoId { get; set; }
        public CategoriaConteudoModel CategoriaConteudoModel { get; set; }

        public string NomeNormalizado { get => NormalizarNome(Nome); set { } }

        private string NormalizarNome(string nome)
        {
            if (nome.Length > 0)
            {
                string n = nome.Trim().Replace(" ", "").ToLower();
                Regex.Replace(n, "[^0-9a-zA-Z]+", "");
                return n;
            }
            return nome;
        }
    }
}
