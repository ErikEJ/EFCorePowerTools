namespace EFCorePowerTools.Converter
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Windows.Data;

    public class CollectionCountToEnabledConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new ArgumentException(@"Value must be of type System.Boolean.", nameof(targetType));

            if (value == null)
                return false;

            if (!(value is ICollection c))
                throw new ArgumentException(@"Value must implement type System.Collections.ICollection.", nameof(value));

            return c.Count > 0;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}