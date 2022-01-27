using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;

namespace Shreco.ViewModels {
    internal class BaseViewModel : INotifyPropertyChanged {
        #region IPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected void Set<T>(T value, ref T field) {
            if (value == null)
                return;
            field = value;
        }
        #endregion
        #region Commands
        public ICommand OnLoadPageCommand { get; protected set; }
        public LayoutState CurrentLayoutState { get; protected set; } = LayoutState.None;
        public string UserRole { get; private set; }

        #endregion
    }
}