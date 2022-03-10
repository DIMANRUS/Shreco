namespace Shreco.Views;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class QrScanButtonView : ContentView {
    public QrScanButtonView() =>
        InitializeComponent();
    public delegate void ScanMethods(string result);
    public event ScanMethods OnScanResult;
    private async void OpenScanner_Clicked(object sender, EventArgs e)
    {
        var scanPage = new ZXingScannerPage();
        await Application.Current.MainPage.Navigation.PushModalAsync(scanPage, true);
        scanPage.OnScanResult += async (result) => {
            scanPage.IsScanning = false;
            Device.BeginInvokeOnMainThread(async () => {
                OnScanResult?.Invoke(result.ToString());
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });
        };
    }
}