using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace Shreco.Pages {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage {
        public AuthPage() =>
            InitializeComponent();

        private void ZXingScannerView_OnOnScanResult(Result result) {
            Device.BeginInvokeOnMainThread(() => {
                label.Text = result.Text;
            });
        }
    }
}