using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TODOList
{
    /// <summary>
    /// Converter that convert date form date picker
    /// to short string that is displayed on text block
    /// </summary>
    class DateConverter : IValueConverter
    {
        public static DateConverter Instance = new DateConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!=null) return ((DateTime)(value)).ToShortDateString();

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
