namespace EFCorePowerTools.Converter
{
    using EFCorePowerTools.Contracts.ViewModels;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class ObjectTypeIconToImageStyleConverter : IValueConverter
    {
        private ResourceDictionary _resourceDictionary;
        public ResourceDictionary ResourceDictionary
        {
            get { return _resourceDictionary; }
            set
            {
                _resourceDictionary = value;
            }
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var objectType = (ObjectTypeIcon)value;
            var key = $"{objectType}ImageStyle";
            return _resourceDictionary[key];
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}