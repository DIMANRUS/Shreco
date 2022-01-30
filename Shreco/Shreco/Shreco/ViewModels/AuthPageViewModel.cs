using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Shreco.ViewModels {
    internal class AuthPageViewModel : BaseViewModel {
        public AuthPageViewModel() {
            #region Commands
            OnLoadPageCommand = new Command(() => {
                CurrentLayoutState = LayoutState.None;
            });
            #endregion
        }
    }
}