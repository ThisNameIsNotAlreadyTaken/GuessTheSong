using System;
using System.Globalization;
using System.Windows.Data;

namespace GuessTheSong.Infrasctucture.Converters
{
    public class AudioTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan)value;
            return timeSpan.ToString(timeSpan.Hours > 0 ? "h\\:mm\\:ss" : "m\\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
