using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace ZividCapture.App.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    class NullToVisibilityConverter : IValueConverter
    {
        private enum Parameters
        {
            Normal, Inverted
        }

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            Parameters? direction;
            if (parameter == null)
                direction = Parameters.Normal;
            else
                direction = (Parameters)Enum.Parse(typeof(Parameters), (string)parameter);

            var result = value != null;
            if (direction == Parameters.Inverted)
                return !result ? Visibility.Visible : Visibility.Collapsed;

            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
