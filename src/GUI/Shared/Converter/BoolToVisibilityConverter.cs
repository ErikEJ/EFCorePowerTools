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
            if (!(value is bool) && value != null)
                throw new ArgumentException(@"Value must be of type System.Boolean or Nullable<System.Boolean>.", nameof(value));

            var b = value as bool?;
            if (invert)
                b = !b;

            return b == null ? Visibility.Hidden : b.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}