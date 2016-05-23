using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GuessTheSong.Infrasctucture.Converters
{
    public class SongToButtonColorConverter : IMultiValueConverter
    { 
        private static readonly SolidColorBrush RedBrush = (SolidColorBrush)Application.Current.FindResource("RedBrush");
        private static readonly SolidColorBrush GoldBrush = (SolidColorBrush)Application.Current.FindResource("GoldBrush");
        private static readonly SolidColorBrush GreenBrush = (SolidColorBrush)Application.Current.FindResource("LightGreenBrush");
        private static readonly SolidColorBrush LightGrayBrush = (SolidColorBrush)Application.Current.FindResource("LightGrayBrush");

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3 || values.Any(x => !(x is bool?))) return null;

            var isGuessed = values[0] as bool? ?? false;
            var isSelected = values[1] as bool? ?? false;
            var isDelayed = values[2] as bool? ?? false;

            if (isDelayed) return RedBrush;
            if (isSelected) return GoldBrush;
            if (isGuessed) return LightGrayBrush;

            return GreenBrush;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
