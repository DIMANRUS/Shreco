namespace Shreco.Helpers;

internal class HttpHelper : IDisposable {
    readonly HttpClient _httpClient = new() {
#if DEBUG
        BaseAddress = new Uri("http://192.168.253.1:5000")
#else
            BaseAddress = new Uri("https://shreco.dimanrus.ru/")
#endif
    };
    public async Task<T> GetJsonRequest<T>(string url, string token = "") {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token == "" ? UserDataStore.Token : token);
        string jsonResponse = await _httpClient.GetStringAsync(url);
        return JsonSerializer.Deserialize<T>(jsonResponse);
    }
    public async Task<HttpResponseMessage> GetRequest(string url, string token = "") {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token == "" ? UserDataStore.Token : token);
        return await _httpClient.GetAsync(url);
    }
    public async Task<HttpResponseMessage> PostRequest<T>(string url, T model, string token = "") {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token == "" ? UserDataStore.Token : token);
        string json = JsonSerializer.Serialize(model);
        return await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
    }

    public void Dispose() {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}