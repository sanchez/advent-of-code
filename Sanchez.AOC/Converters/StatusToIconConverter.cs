using Avalonia.Data.Converters;

using Material.Icons;

using Sanchez.AOC.Enums;

using System.Globalization;

namespace Sanchez.AOC.Converters
{
    public class StatusToIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return (SolutionStatus)value switch
            {
                SolutionStatus.FAILED => MaterialIconKind.ProgressClose,
                SolutionStatus.NOT_STARTED => MaterialIconKind.ProgressClock,
                SolutionStatus.PARTIAL_FAIL => MaterialIconKind.ProgressAlert,
                SolutionStatus.RUNNING => MaterialIconKind.ProgressHelper,
                SolutionStatus.SUCCESS => MaterialIconKind.ProgressCheck,
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
