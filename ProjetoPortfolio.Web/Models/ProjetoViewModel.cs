namespace ProjetoPortfolio.Web.Models
{
    public class ProjetoViewModel
    {
        public Guid Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public bool Status { get; set; }
        public bool Excluido { get; set; }
    }
}
