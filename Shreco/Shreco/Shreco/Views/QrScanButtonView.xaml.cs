namespace Shreco.Views;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class QrScanButtonView : ContentView {
    public QrScanButtonView() =>
        InitializeComponent();
    public delegate void ScanMethods(string result);
    public event ScanMethods OnScanResult;
    private async void OpenScanner_Clicked(object sender, EventArgs e)
    {
        ScannerPage scannerPage = new();
        await Navigation.PushModalAsync(scannerPage, true);
        scannerPage.OnScanResult += (result) => {
            Device.BeginInvokeOnMainThread(async () =>
            {
                OnScanResult?.Invoke(result.ToString());
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });
        };
    }
}