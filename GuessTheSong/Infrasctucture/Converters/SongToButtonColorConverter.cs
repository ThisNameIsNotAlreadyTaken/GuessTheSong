using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace GuessTheSong.Infrasctucture.Converters
{
    public class SongToButtonColorConverter : IMultiValueConverter
    {
        private static readonly SolidColorBrush DarkSalmon = new SolidColorBrush(Colors.DarkSalmon);
        private static readonly SolidColorBrush Gold = new SolidColorBrush(Colors.Gold);
        private static readonly SolidColorBrush PaleGreen = new SolidColorBrush(Colors.PaleGreen);
        private static readonly SolidColorBrush LightGray = new SolidColorBrush(Colors.LightGray);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3 || values.Any(x => !(x is bool?))) return null;

            var isGuessed = values[0] as bool? ?? false;
            var isSelected = values[1] as bool? ?? false;
            var isDelayed = values[2] as bool? ?? false;

            if (isDelayed) return DarkSalmon;
            if (isSelected) return Gold;
            if (isGuessed) return LightGray;

            return PaleGreen;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
