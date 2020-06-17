using System;
using System.Globalization;
using System.Windows.Data;

namespace TODOList
{
    public class IndexToEnableConverter : IValueConverter
    {
        public static IndexToEnableConverter instance = new IndexToEnableConverter();
        /// <summary>
        /// Convert int to enable
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && (int)value > -1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Convert visibility to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
