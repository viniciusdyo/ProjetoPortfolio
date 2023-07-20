namespace ProjetoPortfolio.API.Models
{
    public class ProjetoModel
    {
        public Guid Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? UrlImagem { get; set; }
        public string? UrlRedirecionar { get; set; }
        public bool Status { get; set; }
        public bool Excluido { get; set; }
    }
}
