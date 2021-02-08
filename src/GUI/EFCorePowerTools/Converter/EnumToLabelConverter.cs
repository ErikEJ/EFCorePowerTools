namespace EFCorePowerTools.Converter
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    public class EnumToLabelConverter : IValueConverter
    {
        private ResourceDictionary _resourceDictionary;
        public ResourceDictionary ResourceDictionary
        {
            get
            {
                return _resourceDictionary;
            }
            set
            {
                _resourceDictionary = value;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (attributes?.Any() ?? false)
            {
                return attributes.First().Name;
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}