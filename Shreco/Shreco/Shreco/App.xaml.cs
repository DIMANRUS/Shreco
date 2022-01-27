using Shreco.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Shreco {
    public partial class App {
        public App() {
            InitializeComponent();
            MainPage = new AuthPage();
        }

        private void App_OnPageAppearing(object sender, Page e) {
            Current.UserAppTheme = OSAppTheme.Dark;
        }
    }
}