using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace lab07
{
    public partial class Register : Window
    {
        private string _language;

        public Register(string lang)
        {
            InitializeComponent();
            _language = lang;
            EnbFunctional.ApplyLanguage(_language);
            EnbFunctional.ApplyTheme("Dark");
            LanguageDrop.AddItem("Русский");
            LanguageDrop.AddItem("English");
            
            LanguageDrop.SelectedIndex = _language == "ru" ? 0 : 1;
        }

        private void LanguageCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _language = LanguageDrop.SelectedIndex == 0 ? "ru" : "en";
            EnbFunctional.ApplyLanguage(_language);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var login = new LogIn();
            login.Show();
            Close();
        }

        private void OnlyText(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z0-9.@]+$");
        }

        private void EmailCheck(object sender, RoutedEventArgs e)
        {
            EmailError.Visibility = Visibility.Hidden;

            var email = ((TextBox)sender).Text;

            if (email.Count(c => c == '@') != 1)
            {
                EmailError.Visibility = Visibility.Visible;
                return;
            }

            var domain = new string(email.SkipWhile(c => c != '@').Skip(1).ToArray());

            if (domain is "gmail.com" or "yahoo.com" or "icloud.com"
                       or "outlook.com" or "mail.ru"
                       or "yandex.ru" or "yandex.by")
                return;

            EmailError.Visibility = Visibility.Visible;
        }

        private void IsPasswordsMatch(object sender, RoutedEventArgs e)
        {
            PasswordsError.Visibility =
                PasswordBox1.Password != PasswordBox2.Password
                && !string.IsNullOrEmpty(PasswordBox1.Password)
                && !string.IsNullOrEmpty(PasswordBox2.Password)
                    ? Visibility.Visible
                    : Visibility.Hidden;
        }

        private void CreateButton_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameBox.Text)      ||
                string.IsNullOrEmpty(PasswordBox1.Password) ||
                string.IsNullOrEmpty(PasswordBox2.Password) ||
                string.IsNullOrEmpty(EmailBox.Text)         ||
                EmailError.Visibility    == Visibility.Visible ||
                PasswordsError.Visibility == Visibility.Visible)
            {
                MessageBox.Show((string)FindResource("FillAllFields"));
                return;
            }

            var list = new UserList();
            list.LoadJson();

            if (!list.AddUser(UsernameBox.Text, PasswordBox1.Password,
                    EmailBox.Text, "Dark", _language))
            {
                MessageBox.Show((string)FindResource("RegisterAlreadyExists"));
                return;
            }

            list.SaveJson();

            EnbFunctional.ApplyTheme("Dark");
            var main = new MainWindow(UsernameBox.Text);
            main.Show();
            Close();
        }
    }
}