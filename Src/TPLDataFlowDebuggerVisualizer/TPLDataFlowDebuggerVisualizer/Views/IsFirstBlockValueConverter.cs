using System;
using System.Windows.Data;
using System.Windows.Media;

namespace TPLDataFlowDebuggerVisualizer.Views
{
    public class IsFirstBlockValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var isFirstBlock = (bool)value;
                if (isFirstBlock)
                {
                    return Brushes.Orange;
                }
                //else
                return Brushes.AliceBlue;
            }

            return Colors.AliceBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
