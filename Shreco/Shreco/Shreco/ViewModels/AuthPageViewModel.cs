using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Shreco.ViewModels {
    internal class AuthPageViewModel : BaseViewModel {
        public AuthPageViewModel() {
            #region Commands
            OnLoadPageCommand = new Command(() => {
                CurrentLayoutState = LayoutState.None;
            });
            ChangeAuthState = new Command(() => IsRegistration = !IsRegistration);
            GetAddressLocation = new AsyncCommand(async () => {
                CurrentLayoutState = LayoutState.Loading;
                var location = await Geolocation.GetLocationAsync();
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var address = placemarks?.FirstOrDefault();
                if (address != null)
                    Address = address.CountryName + "," + address.Locality + "," + address.Thoroughfare + "," + address.SubThoroughfare;
                CurrentLayoutState = LayoutState.None;
            });
            #endregion
        }

        private bool _isRegistration;
        public bool IsRegistration {
            get => _isRegistration;
            private set => Set(value, ref _isRegistration);
        }

        private string _address;
        public string Address {
            get => _address;
            private set => Set(value, ref _address);
        }

        public ICommand ChangeAuthState { get; private set; }
        public ICommand GetAddressLocation { get; private set; }
    }
}