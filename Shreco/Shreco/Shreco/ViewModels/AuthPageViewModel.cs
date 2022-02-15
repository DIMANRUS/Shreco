using Shreco.Models;

namespace Shreco.ViewModels;

internal class AuthPageViewModel : BaseViewModel {
    public AuthPageViewModel() {
        #region Commands
        OnLoadPageCommand = new Command(() => {
            CurrentLayoutState = LayoutState.None;
        });
        ChangeAuthState = new Command(() => IsRegistration = !IsRegistration);
        GetAddressLocation = new AsyncCommand(async () => {
            CurrentLayoutState = LayoutState.Loading;
            try {
                var location = await Geolocation.GetLocationAsync();
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var address = placemarks?.FirstOrDefault();
                if (address != null)
                    Address = address.CountryName + "," + address.Locality + "," + address.Thoroughfare + "," +
                              address.SubThoroughfare;
            } catch {
                //Ignored
            }
            CurrentLayoutState = LayoutState.None;
        });
        AuthCommand = new AsyncCommand(async () => {
            CurrentLayoutState = LayoutState.Loading;
            //try {
            User user = new() {
                Email = Email,
                Adress = Address,
                Phone = PhoneNumber,
                NameIdentifer = UserName
            };
            using HttpHelper httpHelper = new();
            HttpResponseMessage responseSessionToken = await httpHelper.GetRequest($"/Auth/SendCode/{user.Email}");
            if (responseSessionToken.IsSuccessStatusCode) {
                string userCode = await Application.Current.MainPage.DisplayPromptAsync("Код", "Введите код с почты", "Ок");
                HttpResponseMessage responseAuth = await httpHelper.GetRequest($"/Auth?email={user.Email}&code={userCode}", await responseSessionToken.Content.ReadAsStringAsync());
                string f = await responseAuth.Content.ReadAsStringAsync();
                if (responseAuth.IsSuccessStatusCode)
                    await Application.Current.MainPage.DisplayAlert("Ура", "", "Закрыть");
                else
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "", "Закрыть");
            } else {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка сервера", "Закрыть");
            }
            //} catch {
            //    await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка отправки запроса", "Закрыть");
            //}
            CurrentLayoutState = LayoutState.None;
        });
        #endregion
    }
    #region Private Fields
    private bool _isRegistration;
    private string _address;
    #endregion
    #region Properties
    public bool IsRegistration {
        get => _isRegistration;
        private set => Set(value, ref _isRegistration);
    }
    public string Address {
        get => _address;
        private set => Set(value, ref _address);
    }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    #endregion
    #region Commands
    public ICommand ChangeAuthState { get; }
    public ICommand GetAddressLocation { get; }
    public ICommand AuthCommand { get; }
    #endregion
}