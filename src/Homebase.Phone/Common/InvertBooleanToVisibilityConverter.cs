using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Homebase.Phone.Common
{
    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isEnabled;

            if (value == null || !bool.TryParse(value.ToString(), out isEnabled))
                return Visibility.Visible;

            return isEnabled
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
