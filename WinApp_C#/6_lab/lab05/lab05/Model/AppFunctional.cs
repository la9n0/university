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
                return $"{Application.Current.FindResource("GunListDiscountPrice")} {finalPrice:F2}";
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public static class ThemeManager
    {
        public static void ApplyTheme(string themeName)
        {
            var app = Application.Current;
            
            for (int i = app.Resources.MergedDictionaries.Count - 1; i >= 0; i--)
            {
                var dict = app.Resources.MergedDictionaries[i];

                if (dict.Source != null)
                {
                    string src = dict.Source.OriginalString;

                    if (src.Contains("LightTheme.xaml") ||
                        src.Contains("DarkTheme.xaml"))
                    {
                        app.Resources.MergedDictionaries.RemoveAt(i);
                    }
                }
            }
            
            string themePath = themeName switch
            {
                "Light" => "Resources/LightTheme.xaml",
                "Dark" => "Resources/DarkTheme.xaml",
                _ => "Resources/LightTheme.xaml"
            };
            
            app.Resources.MergedDictionaries.Add(
                new ResourceDictionary
                {
                    Source = new Uri(themePath, UriKind.Relative)
                });
        }
    }
    
    public class HighlightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 1)
                return false;

            if (values[0] == null)
                return false;

            double discount = System.Convert.ToDouble(values[0]);

            return discount >= 20;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}