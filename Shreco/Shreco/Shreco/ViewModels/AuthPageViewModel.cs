namespace Shreco.ViewModels;

internal class AuthPageViewModel : BaseViewModel {
    public AuthPageViewModel()
    {
        #region Commands
        OnAppearing = new Command(() => {
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
                OnNotifyPropertyChanged(nameof(Address));
            } catch {
                //Ignored
            }
            CurrentLayoutState = LayoutState.None;
        });
        AuthCommand = new AsyncCommand(async () => {
            CurrentLayoutState = LayoutState.Loading;
            if (!string.IsNullOrEmpty(Email)) {
                try {
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
                        if (!string.IsNullOrEmpty(userCode)) {
                            HttpResponseMessage response;
                            if (_isRegistration)
                                response = await httpHelper.PostRequest($"/Auth/Register/{userCode}", user, await responseSessionToken.Content.ReadAsStringAsync());
                            else
                                response = await httpHelper.GetRequest($"/Auth?email={user.Email}&code={userCode}", await responseSessionToken.Content.ReadAsStringAsync());
                            if (response.IsSuccessStatusCode) {
                                await UserDataStore.Set(DatasNames.Token, await response.Content.ReadAsStringAsync());
                                await Application.Current.MainPage.Navigation.PushAsync(new HomePage(), true);
                            }
                            else {
                                throw new Exception(await response.Content.ReadAsStringAsync());
                            }
                        }
                        else {
                            await Application.Current.MainPage.DisplayAlert("Ошибка", "Пустое поле", "Закрыть");
                        }
                    }
                    else {
                        throw new Exception();
                    }
                } catch (Exception ex) {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка. " + ex.Message, "Закрыть");
                }
            }
            else {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Заполните поля", "Закрыть");
            }
            CurrentLayoutState = LayoutState.None;
        });
        #endregion
    }
    #region Private Fields
    private bool _isRegistration;
    private string _pickerUserRole = "Предприниматель";
    #endregion
    #region Properties
    public string PickerUserRole {
        get => _pickerUserRole;
        set {
            _pickerUserRole = value;
            UserRole = value == "Предприниматель";
        }
    }
    public bool IsRegistration {
        get => _isRegistration;
        private set => Set(value, ref _isRegistration);
    }
    public string Address { get; set; } = string.Empty;
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