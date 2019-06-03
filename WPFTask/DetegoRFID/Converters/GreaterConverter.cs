using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace DetegoRFID.Converters
{
    public class GreaterConverter<T> : IValueConverter where T: IComparable
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter is string p)
                return ((T)value).CompareTo(int.Parse(p)) > 0;
            return ((T)value).CompareTo(parameter) > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
