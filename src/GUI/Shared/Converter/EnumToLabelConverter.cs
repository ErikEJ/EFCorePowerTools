namespace EFCorePowerTools.Converter
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Windows;
    using System.Windows.Data;

    public class EnumToLabelConverter : IValueConverter
    {
        public ResourceDictionary ResourceDictionary { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (attributes?.Any() ?? false)
            {
                var attribute = attributes.First();
                if (attribute.ResourceType != null)
                {
                    return ((ResourceManager)attribute.ResourceType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.Public).GetValue(null, null)).GetString(attribute.Name);
                }
                else
                {
                    return attribute.Name;
                }
                
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}