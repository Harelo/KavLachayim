using System;
using System.Globalization;
using Xamarin.Forms;

namespace KavLachayim.Converters
{
    /// <summary>
    /// A converter whose purpose is to relay its functionality
    /// to other objects by invoking delegates.
    /// </summary>
    public class RelayConverter : IValueConverter
    {
        readonly Func<object, Type, object, CultureInfo, object> convert, convertBack;
        public RelayConverter(Func<object, Type, object, CultureInfo, object> _convert, Func<object, Type, object, CultureInfo, object> _convertBack)
        {
            convert = _convert ?? throw new ArgumentNullException("convert");
            convertBack = _convertBack;
        }

        public RelayConverter(Func<object, Type, object, CultureInfo, object> convert) : this(convert, null) { }

        /// <summary>
        /// The conversion logic for the converter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return convert(value, targetType, parameter, culture);
        }

        /// <summary>
        /// The logic for converting back a converted value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (convertBack != null)
                return convertBack(value, targetType, parameter, culture);

            else throw new NotImplementedException();
        }
    }
}
