namespace EFCorePowerTools.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class BoolToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new ArgumentException(@"Value must be of type System.Windows.Visibility.", nameof(targetType));

            var invert = parameter?.ToString().ToLower() == "invert";
            if (!(value is bool b))
                throw new ArgumentException(@"Value must be of type System.Boolean.", nameof(value));

            if (invert)
                b = !b;

            return b ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}