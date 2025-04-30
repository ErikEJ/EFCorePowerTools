using System;
using System.Globalization;
using System.Windows.Data;

namespace EFCorePowerTools.Converter
{
    public class BoolNullConverter : IValueConverter
    {
        object IValueConverter.Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return value != null;
        }

        object IValueConverter.ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return value != null;
        }
    }
}