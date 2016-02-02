using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Homebase.Phone.Common
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isEnabled;

            if (value == null || !bool.TryParse(value.ToString(), out isEnabled))
                return Visibility.Collapsed;

            return isEnabled
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
