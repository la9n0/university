using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace lab05
{
    public partial class GunsList : Window
    {
        private GunStore Store = new GunStore();
        public ObservableCollection<Gun> Guns { get; set; }

        private bool DeleteCard = false;
        private string role;
        private string language;

        public GunsList(string role, string language)
        {
            InitializeComponent();
            this.role = role;
            this.language = language;
            ApplyLanguage(language);
            SetupUI();

            Store.LoadJSON();
            Guns = new ObservableCollection<Gun>();
            foreach (var gun in Store.GetGuns())
                Guns.Add(gun);
            DataContext = this;
            
            SearchCombo.SelectedIndex = 0;

            SearchCombo.SelectionChanged -= FindCardsBy;
            SearchText.TextChanged -= FindCardsBy;
            SearchCombo.SelectionChanged += FindCardsBy;
            SearchText.TextChanged += FindCardsBy;

            UpdateWindowSize();
        }

        private void SetupUI()
        {
            RoleText.Text = (string)FindResource("Mode") + " " + role;

            if (role == "Admin")
            {
                AddButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
            else if (role == "User")
            {
                AddButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Hidden;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var add = new AddProduct(language);
            add.Show();
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteCard = !DeleteCard;
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var gun = border?.DataContext as Gun;

            if (DeleteCard)
            {
                Guns.Remove(gun);
                Store.DelGun(gun.name);
                Store.SaveJSON();
                UpdateWindowSize();
            }
            else if (e.ClickCount == 2)
            {
                var window = new ProductWindow(gun, role, language);
                window.Show();
            }
        }

        private void FindCardsBy(object sender, RoutedEventArgs e)
        {
            Store.LoadJSON();

            if (string.IsNullOrWhiteSpace(SearchText.Text))
            {
                Guns.Clear();
                foreach (var gun in Store.GetGuns())
                    Guns.Add(gun);

                UpdateWindowSize();
                return;
            }

            var filtered = Store.GetGuns().Where(gun =>
            {
                return SearchCombo.SelectedIndex switch
                {
                    0 => gun.name != null && gun.name.Contains(SearchText.Text, StringComparison.CurrentCultureIgnoreCase),
                    1 => gun.category != null && gun.category.Contains(SearchText.Text, StringComparison.CurrentCultureIgnoreCase),
                    2 => gun.country != null && gun.country.Contains(SearchText.Text, StringComparison.CurrentCultureIgnoreCase),
                    _ => true
                };
            }).ToList();

            Guns.Clear();
            foreach (var gun in filtered)
                Guns.Add(gun);

            UpdateWindowSize();
        }

        private void SortCardsBy(object sender, RoutedEventArgs e)
        {
            var sorted = Guns.ToList();

            if (SortByCombobox.SelectedIndex == 0)
                sorted = sorted.OrderBy(g => g.price).ToList();
            else if (SortByCombobox.SelectedIndex == 1)
                sorted = sorted.OrderBy(g => g.discount).ToList();
            else if (SortByCombobox.SelectedIndex == 2)
                sorted = sorted.OrderBy(g => g.quantity).ToList();

            if (HowSortCombobox.SelectedIndex == 1)
                sorted.Reverse();

            Guns.Clear();
            foreach (var gun in sorted)
                Guns.Add(gun);

            UpdateWindowSize();
        }

        private void UpdateWindowSize()
        {
            int columns = 3;
            int cardHeight = 240;
            int rows = (int)Math.Ceiling((double)Guns.Count / columns);

            this.Height = rows * cardHeight + 150;
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
    }
}