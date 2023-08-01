namespace ProjetoPortfolio.Web.Models
{
    public class AutenticacaoResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
