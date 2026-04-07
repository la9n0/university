using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace lab05
{
    public class DiscountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return null;

            if (values[0] is float price && values[1] is float discount)
            {
                if (discount <= 0) return null;

                float finalPrice = price - (price * discount / 100f);
                return $"{Application.Current.FindResource("DiscountPrice")} {finalPrice:F2}";
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}