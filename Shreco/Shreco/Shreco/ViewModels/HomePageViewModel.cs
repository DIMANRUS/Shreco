namespace Shreco.ViewModels;
internal class HomePageViewModel : BaseViewModel {
    private string _token;
    public HomePageViewModel()
    {
        OnAppearing = new AsyncCommand(async () => {
            _token = await UserDataStore.Get(DatasNames.Token);
            Name = TokenHelper.GetName(_token);
            CurrentLayoutState = LayoutState.None;
        });
        Exit = new AsyncCommand(async () => {
            UserDataStore.Clear();
            await Application.Current.MainPage.Navigation.PushModalAsync(new AuthPage());
        });
    }
    public string Name { get; private set; }
    #region Commands
    public ICommand Exit { get; }
    #endregion
}