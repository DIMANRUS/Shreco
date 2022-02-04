using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Shreco.Helpers {
    class HttpHelper : IDisposable {
        readonly HttpClient _httpClient = new HttpClient() {
            BaseAddress = new Uri("https://shreco.dimanrus.ru/")
        };
        public async Task<T> GetRequest<T>(string url) {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserDataStore.UserToken);
            string jsonResponse = await _httpClient.GetStringAsync(url);
            return JsonSerializer.Deserialize<T>(jsonResponse);
        }
        public async Task<HttpResponseMessage> PostRequest<T>(string url, T model) {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserDataStore.UserToken);
            string json = JsonSerializer.Serialize(model);
            return await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public void Dispose() {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}