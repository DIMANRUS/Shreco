[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Shreco;

public partial class App {
    public App() =>
        InitializeComponent();

    protected override async void OnStart()
    {
        MainPage = string.IsNullOrEmpty(await UserDataStore.Get(DatasNames.Token)) ? new NavigationPage(new AuthPage()) : new NavigationPage(new HomePage());
    }
}