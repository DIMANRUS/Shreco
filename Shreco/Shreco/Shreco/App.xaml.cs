using System.Threading.Tasks;
using Shreco.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Shreco {
    public partial class App {
        public App() {
            InitializeComponent();
            Current.UserAppTheme = OSAppTheme.Dark;
            string userToken = null;
            Task taskLoadData = Task.Factory.StartNew(async ()
                => userToken = await SecureStorage.GetAsync("UserToken"));
            taskLoadData.Wait();
            if (userToken == null)
                MainPage = new AuthPage();
            else
                MainPage = new ShellPage();
        }
    }
}