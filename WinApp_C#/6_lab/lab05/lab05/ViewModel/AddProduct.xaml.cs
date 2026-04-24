using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace lab05;

public partial class AddProduct : Window
{
    private readonly GunStore _store = new GunStore();
    private readonly string _language;
    private readonly string _theme;
    public AddProduct(string language, string theme)
    {
        
        InitializeComponent();
        this._language = language;
        this._theme=theme;
        ApplyLanguage(language);
    }

    private readonly List<string> _imagePaths = new List<string>();
        
    private void SelectImages_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog
        {
            Multiselect = true,
            Filter = "Images (*.png;*.jpg)|*.png;*.jpg"
        };

        if (dialog.ShowDialog() != true) return;
        _imagePaths.Clear();
        ImagesList.Items.Clear();

        foreach (var file in dialog.FileNames)
        {
            _imagePaths.Add(file);
            ImagesList.Items.Add(file);
        }
    }

    private void OnlyInt(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
    
    private void OnlyFloat(object sender, TextCompositionEventArgs e)
    {
        TextBox? textBox = sender as TextBox;
        
        string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
        Regex regex = new Regex(@"^\d*(\,\d{0,2})?$");

        e.Handled = !regex.IsMatch(newText);
    }

    private void OnlyString(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ]+$");
        e.Handled = !regex.IsMatch(e.Text);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        var main = new GunsList("Admin", _language, _theme);
        main.Show();
        Close();
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(NameBox.Text) ||
            string.IsNullOrEmpty(FullNameBox.Text) ||
            string.IsNullOrEmpty(DescriptionBox.Text) ||
            string.IsNullOrEmpty(FullDescriptionBox.Text) ||
            string.IsNullOrEmpty(CategoryBox.Text) ||
            string.IsNullOrEmpty(PriceBox.Text) ||
            string.IsNullOrEmpty(QuantityBox.Text) ||
            string.IsNullOrEmpty(CountryBox.Text) ||
            string.IsNullOrEmpty(DiscountBox.Text) || _imagePaths.Count == 0)
        {
            MessageBox.Show((string)FindResource("FillAllFields"));
            return;
        }

        _store.LoadJson();
        string name = NameBox.Text;
        string fullName = FullNameBox.Text;
        string description = DescriptionBox.Text;
        string fullDescription = FullDescriptionBox.Text;
        string category = CategoryBox.Text;
        float price = float.Parse(PriceBox.Text);
        int quantity = int.Parse(QuantityBox.Text);
        string country = CountryBox.Text;
        float discount = float.Parse(DiscountBox.Text);

        if (!_store.AddGun(name, fullName, description, fullDescription,
                category, _imagePaths, price, quantity, country, discount))
        {
            MessageBox.Show((string)FindResource("AddProductAlreadyExists"));
            return;
        }
        _store.SaveJson();
        var main = new GunsList("Admin", _language, _theme);
        main.Show();
        Close(); 
    }
    
    private static void ApplyLanguage(string lang)
    {
        var dict = new ResourceDictionary()
        {
            Source = lang == "ru"
                ? new Uri("Resources/Resources.ru.xaml", UriKind.Relative)
                : new Uri("Resources/Resources.en.xaml", UriKind.Relative)
        };
        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(dict);
    }
}