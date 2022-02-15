using Shreco.Pages;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Shreco;

public partial class App {
    public App() {
        InitializeComponent();
        UserAppTheme = OSAppTheme.Light;
        Task taskLoadData = Task.Factory.StartNew(async ()
            => await UserDataStore.Initializate());
        taskLoadData.Wait();
        if (UserDataStore.Token == null)
            MainPage = new AuthPage();
        else
            MainPage = new ShellPage();
    }
}