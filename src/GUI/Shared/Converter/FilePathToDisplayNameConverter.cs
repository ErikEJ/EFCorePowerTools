namespace EFCorePowerTools.Converter
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;

    public class FilePathToDisplayNameConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new ArgumentException(@"Value must be of type System.String.", nameof(targetType));

            if (!(value is string s))
                throw new ArgumentException(@"Value must be of type System.String.", nameof(value));

            if (string.IsNullOrWhiteSpace(s))
                return "<null>";

            if (s.EndsWith(".sqlproj"))
                return Path.GetFileNameWithoutExtension(s);

            if (s.EndsWith(".dacpac"))
            {
                return s.Length > 55
                           ? "..." + s.Substring(s.Length - 55)
                           : s;
            }

            return s;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}