namespace ProjetoPortfolio.API.Models.DTOs.Response
{
    public class Response<T>
    {
        public Response(T results)
        {
            Results = results ?? throw new Exception("Erro de servidor");
        }
        public T Results { get; set; }
    }
}
