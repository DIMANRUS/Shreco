namespace Shreco.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class NotFoundView : INotifyPropertyChanged {
    public NotFoundView()
    {
        InitializeComponent();
        BindingContext = this;
    }
    private string _text = "Тест";
    public string Text {
        get =>
            _text;
        set {
            _text = value;
            NotifyPropertyChanged();
        }
    }
    #region NPC
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    #endregion
}