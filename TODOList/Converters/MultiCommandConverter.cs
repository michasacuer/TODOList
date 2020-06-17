using System;
using System.Globalization;
using System.Windows.Data;

namespace TODOList
{
    public class MultiCommandConverter : IMultiValueConverter
    {
        public static MultiCommandConverter Instance = new MultiCommandConverter();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
