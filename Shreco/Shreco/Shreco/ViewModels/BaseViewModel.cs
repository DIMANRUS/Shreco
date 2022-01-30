using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Essentials;
using ZXing.Net.Mobile.Forms;

namespace Shreco.ViewModels {
    internal class BaseViewModel : INotifyPropertyChanged {
        public BaseViewModel() {
            ChangeTheme = new AsyncCommand(async () => {
                Application.Current.UserAppTheme = Application.Current.UserAppTheme == OSAppTheme.Dark ? OSAppTheme.Light : OSAppTheme.Dark;
                await SecureStorage.SetAsync("Theme", Application.Current.UserAppTheme == OSAppTheme.Dark ? "Light" : "Dark");
            });
            QrScanOpen = new AsyncCommand(async () => {
                var scanPage = new ZXingScannerPage();
                await Application.Current.MainPage.Navigation.PushModalAsync(scanPage, true);
                scanPage.OnScanResult += (result) => {
                    scanPage.IsScanning = false;
                    Device.BeginInvokeOnMainThread(async () => {
                        await Application.Current.MainPage.Navigation.PopModalAsync();
                        CurrentLayoutState = LayoutState.Loading;
                    });
                };
            });
        }
        #region IPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected void Set<T>(T value, ref T field, [CallerMemberName] string propertyName = "") {
            if (value == null)
                return;
            field = value;
            OnNotifyPropertyChanged(propertyName);
        }
        #endregion
        #region Commands
        public ICommand OnLoadPageCommand { get; protected set; }
        public ICommand ChangeTheme { get; }
        public ICommand QrScanOpen { get; }

        #endregion
        private LayoutState _currentLayoutState = LayoutState.Loading;
        public LayoutState CurrentLayoutState {
            get => _currentLayoutState;
            protected set => Set(value, ref _currentLayoutState);
        }
    }
}