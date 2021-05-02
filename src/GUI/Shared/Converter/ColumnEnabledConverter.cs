using System;
using System.Windows.Data;

namespace EFCorePowerTools.Converter
{
    public class ColumnEnabledConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isTableSelected = (bool)values[0];
            var isPrimaryKey = (bool)values[1];
            return isTableSelected && !isPrimaryKey;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
