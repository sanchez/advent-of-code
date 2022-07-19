using Avalonia.Data.Converters;

using System.Globalization;

namespace Sanchez.AOC.Converters
{
    public class NullConverter : IValueConverter
    {
        public object IsNullValue { get; set; }
        public object IsNotNullValue { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value == null ? IsNullValue : IsNotNullValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
