using System;
using System.Globalization;
using System.Windows.Data;

namespace GuessTheSong.Infrasctucture.Converters
{
    public class AntiBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;
            return boolValue == null ? false : !boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;
            return boolValue == null ? false : !boolValue;
        }
    }
}
