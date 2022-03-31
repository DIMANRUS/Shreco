namespace Shreco.Helpers;

internal class HttpHelper : IDisposable {
    private readonly HttpClient _httpClient = new() {
#if DEBUG
        BaseAddress = new Uri("http://10.0.0.19:5000")
#else
        BaseAddress = new Uri("https://shreco.dimanrus.ru/")
#endif
    };
    public async Task<HttpResponseMessage> GetRequest(string url, string token = "")
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token == "" ? await UserDataStore.Get(DatasNames.Token) : token);
        return await _httpClient.GetAsync(url);
    }
    public async Task<HttpResponseMessage> PostRequest<T>(string url, T model, string token = "")
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token == "" ? await UserDataStore.Get(DatasNames.Token) : token);
        string json = JsonSerializer.Serialize(model);
        return await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
    }
    
    public async Task<HttpResponseMessage> DeleteRequest(string url,string token = ""){
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token == "" ? await UserDataStore.Get(DatasNames.Token) : token);
        return await _httpClient.DeleteAsync(url);
    }
    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}