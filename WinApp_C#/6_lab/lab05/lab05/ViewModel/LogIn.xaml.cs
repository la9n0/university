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

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Password;
            string role = "";
            UserList userList = new UserList();
            userList.LoadJson();
            User? user = userList.GetUser(login);

            if (login == "admin" && password == "admin")
            {
                role = "Admin";
                var gunsList1 = new GunsList(role, currentLanguage, "Light");
                gunsList1.Show();
                this.Close();
                return;
            }
            else if (login == user?.username && password == user?.password)
            {
                role = login;
            }
            else
            {
                Error.Visibility = Visibility.Visible;
                return;
            }

            Error.Visibility = Visibility.Hidden;

            var gunsList = new GunsList(role, currentLanguage, user.theme);
            gunsList.Show();

            this.Close();
        }

        private void LanguageCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LanguageCombo.SelectedIndex == 0)
            {
                ApplyLanguage("ru");
                currentLanguage="ru";
            }
            else
            {
                ApplyLanguage("en");
                currentLanguage="en";
            }
        }

        private void RegistrationButton_click(object sender, RoutedEventArgs e)
        {
            var register = new Register(currentLanguage);
            register.Show();

            this.Close();
        }
    }
}