using Redcat.Xmpp;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Redcat.App.Wpf
{
    [ValueConversion(typeof(JID), typeof(string))]
    public class JidValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return JID.Parse(value.ToString());
        }
    }
}
