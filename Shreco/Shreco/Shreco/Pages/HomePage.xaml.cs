namespace Shreco.Pages;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class HomePage {
    public HomePage() =>
        InitializeComponent();

    private async void QrScanButtonView_OnScanResult(string result)
    {
        using HttpHelper httpHelper = new();
        //string id = TokenHelper.GetActor(result.ToString());
        //string userId = TokenHelper.GetNameIdentifer(userToken);
        //string role = TokenHelper.GetRole(result);
        QrType qrType = (QrType)Enum.Parse(typeof(QrType), TokenHelper.GetRole(result));
        switch (qrType) {
            case QrType.Registration:
                HttpResponseMessage httpResult = await httpHelper.GetRequest($"/Qr/AddWorkerToDistributor?qrId={TokenHelper.GetNameIdentifer(result)}");
                if (!httpResult.IsSuccessStatusCode)
                    MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("Ошибка", "Qr не добавлен", "Закрыть"));
                break;
            case QrType.Distibutor:
                HttpResponseMessage httpResponse = await httpHelper.GetRequest($"/Qr/AddDIstributorToClient?qrId={TokenHelper.GetNameIdentifer(result)}");
                if (!httpResponse.IsSuccessStatusCode)
                    MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("Ошибка", "Qr не добавлен", "Закрыть"));
                break;
            case QrType.Client:

                break;
            default:
                await DisplayAlert("Ошибка", "Qr код недействителен", "Закрыть");
                break;
        }
    }
}