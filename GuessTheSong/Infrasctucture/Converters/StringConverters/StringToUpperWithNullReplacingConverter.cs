using System;
using System.Globalization;
using System.Windows.Data;

namespace GuessTheSong.Infrasctucture.Converters.StringConverters
{
    public class StringToUpperWithNullReplacingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0) return null;

            var value = values[0] as string;
            object nullReplacing = null;

            if (values.Length == 2)
            {
                nullReplacing = values[1] as string;
            }

            return value?.ToUpperInvariant() ?? nullReplacing;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
