﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace GuessTheSong.Infrasctucture.Converters.NullConverters
{
    public class NullToUnknownStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
