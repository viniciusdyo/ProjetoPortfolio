namespace ProjetoPortfolio.API.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool Usado   { get; set; }
        public bool Revogado { get; set; }
        public DateTime AdicionadoData { get; set; }
        public DateTime TempoExpiracao { get; set; }
    }
}
