using Shreco.Pages;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Shreco {
    public partial class App {
        public App() {
            InitializeComponent();
            MainPage = new AuthPage();
        }
    }
}