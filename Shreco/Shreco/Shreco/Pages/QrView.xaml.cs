namespace Shreco.Pages;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class QrView {
    public QrView(string zXingBarcodeImageView)
    {
        InitializeComponent();
        qrView.BarcodeValue = zXingBarcodeImageView;
    }
}