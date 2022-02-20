namespace Shreco.ViewModels;
internal class HomePageViewModel : BaseViewModel {
    private string _token;
    public HomePageViewModel()
    {
        OnAppearing = new AsyncCommand(async () => {
            _token = await UserDataStore.Get(DatasNames.Token);
            Name = TokenHelper.GetName(_token);
            UserRole = bool.Parse(TokenHelper.GetRole(_token));
            CurrentLayoutState = LayoutState.None;
        });
        ExitCommand = new AsyncCommand(async () => {
            UserDataStore.Clear();
            await Application.Current.MainPage.Navigation.PushModalAsync(new AuthPage());
        });
        OpenSettingsPageCommand = new AsyncCommand(async () =>
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage()));
    }
    public string Name { get; private set; }
    #region Commands
    public ICommand ExitCommand { get; }
    public ICommand OpenSettingsPageCommand { get; }
    #endregion
}