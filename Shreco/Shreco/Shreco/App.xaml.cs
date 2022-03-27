[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Shreco;

public partial class App {
    public App() =>
        InitializeComponent();

    protected override async void OnStart()
    {
        NavigationPage navigationPage = new(string.IsNullOrEmpty(await UserDataStore.Get(DatasNames.Token) )? new AuthPage() : new HomePage()) {
            BarBackgroundColor = Color.Goldenrod
        };
        MainPage = navigationPage;
    }
}