using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace lab05
{
    public partial class Register : Window
    {
        private string _language;
        private UserList _list;

        public Register(string lang)
        {
            InitializeComponent();
            this._language = lang;
            ApplyLanguage(_language);
            LanguageCombo.SelectedIndex = _language == "ru" ? 0 : 1;
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

        private void LanguageCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LanguageCombo.SelectedIndex == 0)
            {
                ApplyLanguage("ru");
                _language="ru";
            }
            else
            {
                ApplyLanguage("en");
                _language="en";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void OnlyString(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void OnlyText(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[a-zA-Z0-9.@]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void EmailCheck(object sender, RoutedEventArgs e)
        {
            EmailError.Visibility = Visibility.Hidden;
            var textBox = sender as TextBox;
            string email = textBox.Text;
            int count = email.Count(x => x == '@');
            if (count != 1)
            {
                EmailError.Visibility = Visibility.Visible;
                return;
            }

            email = new string(email.SkipWhile(c => c != '@').Skip(1).ToArray());

            if (email == "gmail.com" || email == "yahoo.com" || email == "icloud.com"
                || email == "outlook.com" || email == "mail.ru" ||
                email == "yandex.ru" || email == "yandex.by")
            {
                return;
            }
            EmailError.Visibility = Visibility.Visible;
        }

        private void IsPasswordsMatch(object sender, RoutedEventArgs e)
        {

            if (PasswordBox1.Password != PasswordBox2.Password 
                && !string.IsNullOrEmpty(PasswordBox1.Password) 
                && !string.IsNullOrEmpty(PasswordBox1.Password))
            {
                PasswordsError.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordsError.Visibility = Visibility.Hidden;
            }
        }

        private void CreateButton_click(object sender, RoutedEventArgs e)
        {
            _list = new UserList();
            _list.LoadJson();
            if (string.IsNullOrEmpty(UsernameBox.Text) ||
                string.IsNullOrEmpty(PasswordBox1.Password) ||
                string.IsNullOrEmpty(PasswordBox2.Password) ||
                string.IsNullOrEmpty(NameBox.Text) ||
                string.IsNullOrEmpty(SurnameBox.Text) ||
                string.IsNullOrEmpty(EmailBox.Text) ||
                EmailError.Visibility == Visibility.Visible)
            {
                MessageBox.Show((string)FindResource("FillAllFields"));
                return;
            }
            

            string user = UsernameBox.Text;
            string password = PasswordBox1.Password;
            string name = NameBox.Text;
            string surname = SurnameBox.Text;
            string email = EmailBox.Text;
            string theme = "Light";
            if (!_list.AddUser(user, password, name, surname, email, theme, _language))
            {
                MessageBox.Show((string)FindResource("RegisterAlreadyExists"));
                return;
            }
            _list.SaveJson();
            var main = new GunsList(user, _language, theme);
            main.Show();
            Close(); 
        }
    }
}