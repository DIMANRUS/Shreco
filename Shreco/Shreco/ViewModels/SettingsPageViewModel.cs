namespace Shreco.ViewModels;
internal class SettingsPageViewModel : BaseViewModel
{
    #region Initialization
    public SettingsPageViewModel()
    {
        OnAppearing = new AsyncCommand(async () =>
        {
            HttpResponseMessage responseMessage = null;
            try
            {
                string token = await UserDataStore.Get(DatasNames.Token);
                UserRole = bool.Parse(TokenHelper.GetRole(token));
                using HttpHelper httpHelper = new();
                responseMessage = await httpHelper.GetRequest("/User");
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    User = await JsonSerializer.DeserializeAsync<User>(await responseMessage.Content.ReadAsStreamAsync());
                    OnNotifyPropertyChanged(nameof(User));
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", await responseMessage.Content.ReadAsStringAsync(), "Закрыть");
            }
            CurrentLayoutState = LayoutState.None;
        });
        UpdateUserCommand = new AsyncCommand(async () =>
        {
            CurrentLayoutState = LayoutState.Loading;
            using HttpHelper httpHelper = new();
            HttpResponseMessage responseMessage = await httpHelper.PutRequest("/User", User);
            if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", await responseMessage.Content.ReadAsStringAsync(), "Закрыть");
            }
            else
            {
                responseMessage = await httpHelper.GetRequest("/User/NewToken");
                if (responseMessage.IsSuccessStatusCode)
                {
                    await UserDataStore.Set(DatasNames.Token, await responseMessage.Content.ReadAsStringAsync());
                }
            }
            CurrentLayoutState = LayoutState.None;
        });
    }
    #endregion
    #region Properties
    public User User { get; set; }
    #endregion
    #region Commands
    public ICommand UpdateUserCommand { get; set; }
    #endregion
}