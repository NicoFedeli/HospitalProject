using System.Text;
using Newtonsoft.Json;

namespace Hospital.Services
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string url);
        Task<T> PostAsync<T>(string url, object data);
        Task<T> PutAsync<T>(string url, object data);
        Task<T> DeleteAsync<T>(string url);
    }
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Implementazione dei metodi dell'interfaccia IApiService
        // Evito così di ripetere la chiamta HttpClient in ogni controller

        public Task<T> DeleteAsync<T>(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(response);
        }

        public async Task<T> PostAsync<T>(string url, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public Task<T> PutAsync<T>(string url, object data)
        {
            throw new NotImplementedException();
        }

        // idem per PutAsync e DeleteAsync

    }
}
