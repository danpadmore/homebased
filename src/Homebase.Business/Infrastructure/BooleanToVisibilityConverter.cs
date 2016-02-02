using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Homebase.Business.Infrastructure
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isVisible;

            if (value == null || !bool.TryParse(value.ToString(), out isVisible))
                return false;

            return isVisible
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
