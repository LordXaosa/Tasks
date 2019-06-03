using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace DetegoRFID.Helpers
{
    public class GenericFactoryHelper: MarkupExtension, IValueConverter
    {
        public Type Type { get; set; }
        public Type T { get; set; }
        public object Instance { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Instance is IValueConverter conv)
                return conv.Convert(value, targetType, parameter, culture);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Instance is IValueConverter conv)
                return conv.ConvertBack(value, targetType, parameter, culture);
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Instance == null)
            {
                var genericType = Type.MakeGenericType(T);
                Instance = Activator.CreateInstance(genericType);
            }
            return Instance;
        }
    }
}
