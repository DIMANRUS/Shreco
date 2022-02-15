using System.Globalization;

namespace Shreco.Converters;

internal class AuthStateToButtonTextConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if ((bool)value)
            return parameter?.ToString() == "AuthButton" ? "Зарегистрироваться" : "Вход";
        return parameter?.ToString() == "AuthButton" ? "Войти" : "Регистрация";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}