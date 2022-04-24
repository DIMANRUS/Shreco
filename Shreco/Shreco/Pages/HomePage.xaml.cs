namespace Shreco.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class HomePage
{
    public HomePage() =>
        InitializeComponent();
    private async void QrScanButtonView_OnScanResult(string result)
    {
        using HttpHelper httpHelper = new();
        //string id = TokenHelper.GetActor(result.ToString());
        //string userId = TokenHelper.GetNameIdentifer(userToken);
        //string role = TokenHelper.GetRole(result);
        QrType qrType = (QrType)Enum.Parse(typeof(QrType), TokenHelper.GetRole(result));
        HttpResponseMessage httpResult = null;
        switch (qrType)
        {
            case QrType.Registration:
                httpResult = await httpHelper.GetRequest($"/Qr/AddWorkerToDistributor?qrId={TokenHelper.GetNameIdentifer(result)}");
                break;
            case QrType.Distibutor:
                httpResult = await httpHelper.GetRequest($"/Qr/AddDistributorToClient?qrId={TokenHelper.GetNameIdentifer(result)}");
                break;
            case QrType.Client:
                int price = int.Parse(await DisplayPromptAsync("", ""));
                if (price > 0)
                {
                    ClientQrAfterScaningRequest request = new()
                    {
                        QrId = int.Parse(TokenHelper.GetNameIdentifer(result)),
                        Price = price
                    };
                    httpResult = await httpHelper.PostRequest("/Qr/WorkerFromQrCLient", request);
                }
                else
                {
                    ErrorPopup();
                }
                break;
            default:
                ErrorPopup();
                break;
        }
        if (!httpResult.IsSuccessStatusCode)
            ErrorPopup();
    }
    private async void ErrorPopup() =>
        await DisplayAlert("Ошибка", "Qr не добавлен", "Закрыть");
}