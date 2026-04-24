using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace lab05
{
    public partial class GunsList : Window
    {
        private readonly GunStore _store = new GunStore();
        public ObservableCollection<Gun> Guns { get; set; }

        private bool _deleteCard = false;
        private readonly string _role;
        private readonly string _language;
        private readonly string _theme;
        private Gun lastCard = null;

        public GunsList(string role, string language, string theme)
        {
            InitializeComponent();
            this._role = role;
            this._language = language;
            this._theme = theme;
            ApplyLanguage(language);
            SetupUI();

            _store.LoadJson();
            Guns = new ObservableCollection<Gun>();
            foreach (var gun in _store.GetGuns())
                Guns.Add(gun);
            DataContext = this;
            
            SearchCombo.SelectedIndex = 0;

            SearchCombo.SelectionChanged -= FindCardsBy;
            SearchText.TextChanged -= FindCardsBy;
            SearchCombo.SelectionChanged += FindCardsBy;
            SearchText.TextChanged += FindCardsBy;

            UpdateWindowSize();
            ThemeManager.ApplyTheme(_theme);
        }

        private void SetupUI()
        {
            RoleText.Text = (string)FindResource("Mode") + " " + _role;

            if (_role == "Admin")
            {
                AddButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                UserMenu.Visibility = Visibility.Hidden;
            }
            else
            {
                AddButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Hidden;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var add = new AddProduct(_language, _theme);
            add.Show();
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _deleteCard = !_deleteCard;
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var gun = border?.DataContext as Gun;

            if (_deleteCard)
            {
                Guns.Remove(gun);
                _store.DelGun(gun.name);
                _store.SaveJson();
                UpdateWindowSize();
            }
            else if (e.ClickCount == 2)
            {
                var window = new ProductWindow(gun, _role, _language, _theme);
                window.Show();
                lastCard = gun;
            }
        }

        private void FindCardsBy(object sender, RoutedEventArgs e)
        {
            _store.LoadJson();

            if (string.IsNullOrWhiteSpace(SearchText.Text))
            {
                Guns.Clear();
                foreach (var gun in _store.GetGuns())
                    Guns.Add(gun);

                UpdateWindowSize();
                return;
            }

            var filtered = _store.GetGuns().Where(gun =>
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

        private void UserMenu_click(object sender, RoutedEventArgs e)
        {
            var window = new UserMenu(_role, _language);
            window.Show();
            
            this.Close();
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (lastCard == null) return;
            var window = new ProductWindow(lastCard, _role, _language, _theme);
            window.Show();
        }
    }
}