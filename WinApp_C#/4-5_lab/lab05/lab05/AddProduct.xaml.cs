using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace lab05;

public partial class AddProduct : Window
{
    private GunStore Store = new GunStore();
    private string language;
    public AddProduct(string language)
    {
        
        InitializeComponent();
        this.language = language;
    }
    
    public List<string> imagePaths = new List<string>();
        
    private void SelectImages_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Multiselect = true;
        dialog.Filter = "Images (*.png;*.jpg)|*.png;*.jpg";

        if (dialog.ShowDialog() == true)
        {
            imagePaths.Clear();
            ImagesList.Items.Clear();

            foreach (var file in dialog.FileNames)
            {
                imagePaths.Add(file);
                ImagesList.Items.Add(file);
            }
        }
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

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        var main = new GunsList("Admin", language);
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
            string.IsNullOrEmpty(DiscountBox.Text) || imagePaths.Count == 0)
        {
            MessageBox.Show("Заполните все поля!");
            return;
        }


        Store.LoadJSON();
        string name = NameBox.Text;
        string fullName = FullNameBox.Text;
        string description = DescriptionBox.Text;
        string fullDescription = FullDescriptionBox.Text;
        string category = CategoryBox.Text;
        float price = float.Parse(PriceBox.Text);
        int quantity = int.Parse(QuantityBox.Text);
        string country = CountryBox.Text;
        float discount = float.Parse(DiscountBox.Text);

        if (!Store.AddGun(name, fullName, description, fullDescription,
                category, imagePaths, price, quantity, country, discount))
        {
            MessageBox.Show("Товар с таким именем уже существует");
            return;
        }
        Store.SaveJSON();
        var main = new GunsList("Admin", language);
        main.Show();
        Close(); 
    }
    
}