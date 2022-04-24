using System.Globalization;

namespace Shreco.Converters;
internal class RoleToVisibleConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value) {
            if ((bool)parameter)
                return true;
            return false;
        }
        if ((bool)parameter)
            return false;
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}