namespace Shreco.ViewModels;

internal class HomePageViewModel : BaseViewModel
{
    #region Private values
    private string _token;
    #endregion
    #region Initialization
    public HomePageViewModel()
    {
        OnAppearing = new AsyncCommand(async () =>
        {
            _token = await UserDataStore.Get(DatasNames.Token);
            Name = TokenHelper.GetName(_token);
            UserRole = bool.Parse(TokenHelper.GetRole(_token));
            await LoadData();
            CurrentLayoutState = LayoutState.None;
        });
        ExitCommand = new AsyncCommand(async () =>
        {
            UserDataStore.Clear();
            await Application.Current.MainPage.Navigation.PushAsync(new AuthPage());
        });
        OpenSettingsPageCommand = new AsyncCommand(async () =>
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage()));
        AddDistributorCommand = new AsyncCommand(async () =>
        {
            HttpResponseMessage httpResponseMessage = null;
            try
            {
                CurrentLayoutState = LayoutState.Loading;
                string percentString = await Application.Current.MainPage.DisplayPromptAsync("Новый распространитель",
                    "Введите процент, который получит распространитель от заказа", "Далее", "Отмена", "1-99", 2,
                    Keyboard.Numeric);
                string percentClientString = await Application.Current.MainPage.DisplayPromptAsync(
                    "Новый распространитель", "Введите процент скидки, которую получит клиент", "Создать Qr", "Отмена",
                    "1-99", 2, Keyboard.Numeric);
                if (percentString != null && percentClientString != null)
                {
                    int percent = int.Parse(percentString.Replace('-', ' '));
                    int percentClient = int.Parse(percentString.Replace('-', ' '));
                    using HttpHelper httpHelper = new();
                    string userId = TokenHelper.GetNameIdentifer(await UserDataStore.Get(DatasNames.Token));
                    httpResponseMessage = await httpHelper.GetRequest($"Qr/AddRegQr?percent={percent}&percentForClient={percentClient}");
                    if (httpResponseMessage.IsSuccessStatusCode)
                        await LoadData();
                    else
                        throw new Exception();
                }
            }
            catch
            {
                CurrentLayoutState = LayoutState.None;
                await Application.Current.MainPage.DisplayAlert("Ошибка", await httpResponseMessage?.Content.ReadAsStringAsync(),
                    "Закрыть");
            }
            CurrentLayoutState = LayoutState.None;
        });
        LoadDataCommand = new AsyncCommand(async () => { await LoadData(); });
        ViewQrCommand = new AsyncCommand<int>(async (int id) =>
        {
            CurrentLayoutState = LayoutState.Loading;
            HttpResponseMessage httpResponseMessage = null;
            try
            {
                using HttpHelper httpHelper = new();
                httpResponseMessage = await httpHelper.GetRequest($"Qr/GetQrToken?qrId={id}");
                if (httpResponseMessage.IsSuccessStatusCode)
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new QrView(await httpResponseMessage.Content.ReadAsStringAsync()));
                else
                    throw new Exception();
            }
            catch
            {
                CurrentLayoutState = LayoutState.None;
                await Application.Current.MainPage.DisplayAlert("Ошибка", await httpResponseMessage.Content.ReadAsStringAsync(),
                    "Закрыть");
            }
        });
        RemoveQrCommand = new AsyncCommand<int>(async (id) =>
        {
            CurrentLayoutState = LayoutState.Loading;
            HttpResponseMessage httpResponseMessage = null;
            try
            {
                using HttpHelper httpHelper = new();
                httpResponseMessage = await httpHelper.DeleteRequest($"Qr?qrId={id}");
                if (httpResponseMessage.IsSuccessStatusCode)
                    await LoadData();
                else
                    throw new Exception();
            }
            catch
            {
                CurrentLayoutState = LayoutState.None;
                await Application.Current.MainPage.DisplayAlert("Ошибка", await httpResponseMessage.Content.ReadAsStringAsync(),
                    "Закрыть");
            }
        });
    }
    #endregion
    #region Methods
    private async Task LoadData()
    {
        CurrentLayoutState = LayoutState.Loading;
        try
        {
            using HttpHelper httpHelper = new();
            HttpResponseMessage responseMessage = await httpHelper.GetRequest("/User/GetDistributorQrs");
            if (responseMessage.IsSuccessStatusCode)
                DistributorQrs = await JsonSerializer.DeserializeAsync<IEnumerable<QrWithUserResponse>>(await responseMessage.Content.ReadAsStreamAsync());
            responseMessage = await httpHelper.GetRequest("/User/GetClients");
            if (responseMessage.IsSuccessStatusCode)
                WorkerQrs = await JsonSerializer.DeserializeAsync<IEnumerable<Qr>>(await responseMessage.Content.ReadAsStreamAsync());
            responseMessage = await httpHelper.GetRequest("/User/GetWorkerQrs");
            if (responseMessage.IsSuccessStatusCode)
                WorkerQrs = await JsonSerializer.DeserializeAsync<IEnumerable<Qr>>(await responseMessage.Content.ReadAsStreamAsync());
            responseMessage = await httpHelper.GetRequest(bool.Parse(TokenHelper.GetRole(_token)) ? "/User/GetDistributorsWorker" : "/User/GetDistributorsClient");
            if (responseMessage.IsSuccessStatusCode)
                Distributors = await JsonSerializer.DeserializeAsync<IEnumerable<QrWithUserResponse>>(await responseMessage.Content.ReadAsStreamAsync());
            responseMessage = await httpHelper.GetRequest("/User/GetHistoryDistributors");
            if (responseMessage.IsSuccessStatusCode)
                HistoryDistributors = await JsonSerializer.DeserializeAsync<IEnumerable<HistoryWithQrUserResponse>>(await responseMessage.Content.ReadAsStreamAsync());
            responseMessage = await httpHelper.GetRequest("/User/GetHistoryClients");
            if (responseMessage.IsSuccessStatusCode)
                HistoryClients = await JsonSerializer.DeserializeAsync<IEnumerable<HistoryWithQrUserResponse>>(await responseMessage.Content.ReadAsStreamAsync());
            responseMessage = await httpHelper.GetRequest("/User/GetHistoryUserQrApplied");
            if (responseMessage.IsSuccessStatusCode)
                HistoryUserQrApplied = await JsonSerializer.DeserializeAsync<IEnumerable<HistoryWithQrUserResponse>>(await responseMessage.Content.ReadAsStreamAsync());
        }
        catch
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка загрзуки данных. Проверьте подключение к интрнету", "Закрыть");
        }
        IsRefreshing = false;
        CurrentLayoutState = LayoutState.None;
    }
    #endregion
    #region Commands
    public ICommand ExitCommand { get; }
    public ICommand OpenSettingsPageCommand { get; }
    public ICommand AddDistributorCommand { get; }
    public ICommand LoadDataCommand { get; }
    public ICommand ViewQrCommand { get; }
    public ICommand RemoveQrCommand { get; }
    #endregion
    #region Properties
    #region Collections
    public IEnumerable<QrWithUserResponse> DistributorQrs { get; set; } = new List<QrWithUserResponse>();
    public IEnumerable<QrWithUserResponse> Clients { get; set; } = new List<QrWithUserResponse>();
    public IEnumerable<Qr> WorkerQrs { get; set; } = new List<Qr>();
    public IEnumerable<QrWithUserResponse> Distributors { get; set; } = new List<QrWithUserResponse>();
    public IEnumerable<HistoryWithQrUserResponse> HistoryDistributors { get; set; } = new List<HistoryWithQrUserResponse>();
    public IEnumerable<HistoryWithQrUserResponse> HistoryClients { get; set; } = new List<HistoryWithQrUserResponse>();
    public IEnumerable<HistoryWithQrUserResponse> HistoryUserQrApplied { get; set; } = new List<HistoryWithQrUserResponse>();
    #endregion
    public string Name { get; private set; }
    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => Set(value, ref _isRefreshing);
    }
    #endregion
}