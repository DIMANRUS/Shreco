[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Shreco;

public partial class App {
    public App() =>
        InitializeComponent();

    protected override async void OnStart()
    {
        if (string.IsNullOrEmpty(await UserDataStore.Get(DatasNames.Token)))
            MainPage = new NavigationPage(new AuthPage());
        else
            MainPage = new NavigationPage(new HomePage());
    }
}