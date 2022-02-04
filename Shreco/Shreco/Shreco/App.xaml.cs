using System.Threading.Tasks;
using Shreco.Pages;
using Shreco.Stores;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Shreco {
    public partial class App {
        public App() {
            InitializeComponent();
            Task taskLoadData = Task.Factory.StartNew(async ()
                => await UserDataStore.Initializate());
            taskLoadData.Wait();
            if (UserDataStore.Token == null)
                MainPage = new AuthPage();
            else
                MainPage = new ShellPage();
        }
    }
}