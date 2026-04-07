using System;
using System.Windows;

namespace lab05
{
    public partial class LoginWindow : Window
    {
        private string currentLanguage = "ru";

        public LoginWindow()
        {
            InitializeComponent();

            ApplyLanguage(currentLanguage);
            LanguageCombo.SelectedIndex = 0;
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

            currentLanguage = lang;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Password;
            string role = "";

            if (login == "admin" && password == "admin")
                role = "Admin";
            else if (login == "user" && password == "user")
                role = "User";
            else
            {
                Error.Visibility = Visibility.Visible;
                return;
            }

            Error.Visibility = Visibility.Hidden;

            var gunsList = new GunsList(role, currentLanguage);
            gunsList.Show();

            this.Close();
        }

        private void LanguageCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LanguageCombo.SelectedIndex == 0)
                ApplyLanguage("ru");
            else
                ApplyLanguage("en");
        }
    }
}