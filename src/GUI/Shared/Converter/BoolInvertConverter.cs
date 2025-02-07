using System;
using System.Globalization;
using System.Windows.Data;

namespace EFCorePowerTools.Converter
{
    public class BoolInvertConverter : IValueConverter
    {
        object IValueConverter.Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return !(bool)value;
        }

        object IValueConverter.ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}