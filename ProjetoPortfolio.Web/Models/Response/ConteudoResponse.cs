namespace ProjetoPortfolio.Web.Models.Response
{
    public class ConteudoResponse
    {
        public Guid Id { get; set; }
        public string Conteudo { get; set; }
        public string Titulo { get; set; }
        public string Nome { get; set; }
        public Guid CategoriaId { get; set; }
        public object CategoriaConteudoModel { get; set; }
        public IEnumerable<AtivoResponse>? AtivosConteudo { get; set; } = null;
    }
}
