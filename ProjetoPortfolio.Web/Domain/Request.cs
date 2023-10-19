using Newtonsoft.Json;
using ProjetoPortfolio.Web.Controllers;
using ProjetoPortfolio.Web.Models;
using ProjetoPortfolio.Web.Models.Response;
using System.Net.Http;

namespace ProjetoPortfolio.Web.Domain
{
    public class Request<T>
    {
        private readonly HttpClient _httpClient = null;
        private readonly string ENDPOINT = "https://api.viniciusdyonisio.targetbr.biz/api";
        public Request()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ENDPOINT);

        }

        public async Task<PorfolioResponse<T>> Listar(string requestUri)
        {
            try
            {
                var request = await _httpClient.GetAsync($"{ENDPOINT}/{requestUri}");
                var readTask = await request.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<PorfolioResponse<T>>(readTask);

                if (response == null)
                    throw new Exception("Erro no servidor.");

                return response;
            }
            catch (Exception e)
            {
                var erro = new PorfolioResponse<T>();
                erro.Errors = new() { e.Message };
                return erro;
            }
        }

        public async Task<PorfolioResponse<T>> Cadastrar(string requestUri, T conteudo)
        {
            try
            {
                var request = await _httpClient.PostAsJsonAsync($"{ENDPOINT}/{requestUri}", conteudo);
                var readTask = await request.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<PorfolioResponse<T>>(readTask);

                if (response == null)
                    throw new Exception("Erro no servidor.");

                return response;
            }
            catch (Exception e)
            {
                var erro = new PorfolioResponse<T>();
                erro.Errors = new() { e.Message };
                return erro;
            }
        }

        public async Task<PorfolioResponse<T>> Editar(string requestUri, T conteudo)
        {
            try
            {
                var request = await _httpClient.PutAsJsonAsync($"{ENDPOINT}/{requestUri}", conteudo);
                var readTask = await request.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<PorfolioResponse<T>>(readTask);

                if (response == null) throw new Exception("Erro no servidor");

                return response;
            }
            catch (Exception e)
            {
                var erro = new PorfolioResponse<T>();
                erro.Errors = new() { e.Message };
                return erro;
            }
        }

        public async Task<PorfolioResponse<T>> Excluir(string requestUri, Guid id, bool logic)
        {
            try
            {
                var request = logic ? await _httpClient.PutAsJsonAsync($"{ENDPOINT}/{requestUri}", id) : await _httpClient.DeleteAsync($"{ENDPOINT}/{requestUri}/{id}");
                var readTask = await request.Content.ReadAsStringAsync();

                //var readTask = await request.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<PorfolioResponse<T>>(readTask);

                if (response == null) throw new Exception("Erro no servidor");

                return response;
            }
            catch (Exception e)
            {
                var erro = new PorfolioResponse<T>();
                erro.Errors = new() { e.Message };
                return erro;
            }
        }
    }
}
