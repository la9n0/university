using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace lab05
{
    public partial class ProductWindow : Window
    {
        private Gun gun { get; set; }
        private GunStore Store = new GunStore();
        private string language;

        public ProductWindow(Gun g, string role, string language)
        {
            InitializeComponent();

            this.language = language;
            ApplyLanguage(language);

            gun = g;

            LoadData();
            LoadImages();

            SetReadOnly(true);

            RoleText.Text = (string)FindResource("Mode") + " " + role;

            if (role == "Admin")
                EditButton.Visibility = Visibility.Visible;
        }

        private void ApplyLanguage(string lang)
        {
            var dict = new ResourceDictionary();

            if (lang == "ru")
                dict.Source = new Uri("Resources/Resources.ru.xaml", UriKind.Relative);
            else
                dict.Source = new Uri("Resources/Resources.en.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void LoadData()
        {
            NameText.Text = gun.name;

            NameBox.Text = gun.name;
            PriceBox.Text = gun.price.ToString();
            DescriptionBox.Text = gun.description;
            CategoryBox.Text = gun.category;
            DiscountBox.Text = gun.discount.ToString();
            QuantityBox.Text = gun.quantity.ToString();
            CountryBox.Text = gun.country;
        }

        private void LoadImages()
        {
            try
            {
                if (gun.imagePath.Count > 0)
                {
                    var img = new BitmapImage(new Uri(gun.imagePath[0], UriKind.RelativeOrAbsolute));
                    MainImage.Source = img;
                    Img1.Source = img;
                }

                if (gun.imagePath.Count > 1)
                    Img2.Source = new BitmapImage(new Uri(gun.imagePath[1], UriKind.RelativeOrAbsolute));

                if (gun.imagePath.Count > 2)
                    Img3.Source = new BitmapImage(new Uri(gun.imagePath[2], UriKind.RelativeOrAbsolute));
            }
            catch { }
        }

        private void Image_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var img = sender as System.Windows.Controls.Image;
            if (img?.Source != null)
                MainImage.Source = img.Source;
        }

        private void SetReadOnly(bool value)
        {
            PriceBox.IsReadOnly = value;
            DescriptionBox.IsReadOnly = value;
            CategoryBox.IsReadOnly = value;
            DiscountBox.IsReadOnly = value;
            QuantityBox.IsReadOnly = value;
            CountryBox.IsReadOnly = value;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            SetReadOnly(false);

            EditButton.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;
        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            SetReadOnly(true);

            EditButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Collapsed;
            CancelButton.Visibility = Visibility.Collapsed;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            gun.price = float.Parse(PriceBox.Text);
            gun.description = DescriptionBox.Text;
            gun.category = CategoryBox.Text;
            gun.discount = float.Parse(DiscountBox.Text);
            gun.quantity = int.Parse(QuantityBox.Text);
            gun.country = CountryBox.Text;

            Store.LoadJSON();
            Store.DelGun(NameBox.Text);
            Store.AddGun(gun.name, gun.fullName, gun.description,
                gun.fullDescription, gun.category, gun.imagePath,
                gun.price, gun.quantity, gun.country, gun.discount);
            Store.SaveJSON();

            SetReadOnly(true);

            EditButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Collapsed;
            CancelButton.Visibility = Visibility.Collapsed;
        }
        
        private void OnlyInt(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    
        private void OnlyFloat(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
        
            string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
            Regex regex = new Regex(@"^\d*(\,\d{0,2})?$");

            e.Handled = !regex.IsMatch(newText);
        }

        private void OnlyString(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}