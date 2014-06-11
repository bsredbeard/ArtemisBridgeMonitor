using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Artemis.Community.BridgeMonitor
{
    public abstract class BooleanConverter<T> : IValueConverter
    {
        protected BooleanConverter(T trueValue, T falseValue)
        {
            this.TrueValue = trueValue;
            this.FalseValue = falseValue;
        }

        public T TrueValue { get; set; }

        public T FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Object.Equals(value, true) ?
                this.TrueValue : this.FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Object.Equals(value, this.TrueValue) ? true : false;
        }
    }

    public sealed class BooleanToStringConverter : BooleanConverter<string>
    {
        public BooleanToStringConverter() : base("true", "false") { }
    }

    public sealed class VersionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Version)
            {
                return ((Version)value).ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class NullableVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return System.Windows.Visibility.Collapsed;
            return System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
