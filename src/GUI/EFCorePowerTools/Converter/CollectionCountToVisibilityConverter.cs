namespace EFCorePowerTools.Converter
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class CollectionCountToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new ArgumentException(@"Value must be of type System.Windows.Visibility.", nameof(targetType));

            if (!(value is ICollection c))
                throw new ArgumentException(@"Value must implement type System.Collections.ICollection.", nameof(value));

            return c.Count > 0
                       ? Visibility.Visible
                       : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}