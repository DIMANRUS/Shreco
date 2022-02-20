using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZXing.Net.Mobile.Forms;

namespace Shreco.ViewModels;

internal class BaseViewModel : INotifyPropertyChanged {
    #region IPC
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnNotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    protected void Set<T>(T value, ref T field, [CallerMemberName] string propertyName = "")
    {
        if (value == null)
            return;
        field = value;
        OnNotifyPropertyChanged(propertyName);
    }
    #endregion
    #region Commands
    public ICommand QrScanOpen { get; }
    public ICommand OnAppearing { get; protected set; }
    #endregion
    private bool _userRole;
    public bool UserRole {
        get => _userRole;
        protected set => Set(value, ref _userRole);
    }
    private LayoutState _currentLayoutState = LayoutState.Loading;
    public LayoutState CurrentLayoutState {
        get => _currentLayoutState;
        protected set => Set(value, ref _currentLayoutState);
    }
}