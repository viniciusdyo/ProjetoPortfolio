namespace ProjetoPortfolio.API.Models.DTOs
{
    public class ConteudoDto
    {
        public Guid Id { get; set; }
        public string Conteudo { get; set; }
        public string Titulo { get; set; }
        public string Nome { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
