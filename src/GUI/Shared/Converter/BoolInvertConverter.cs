namespace EFCorePowerTools.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class BoolInvertConverter : IValueConverter
    {
        object IValueConverter.Convert(object value,
                                       Type targetType,
                                       object parameter,
                                       CultureInfo culture)
        {
            return !(bool) value;
        }

        object IValueConverter.ConvertBack(object value,
                                           Type targetType,
                                           object parameter,
                                           CultureInfo culture)
        {
            return !(bool) value;
        }
    }
}