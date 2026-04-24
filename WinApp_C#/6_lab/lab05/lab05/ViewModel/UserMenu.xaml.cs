using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace lab05;

public partial class UserMenu : Window
{
    private string _language;
    private readonly string _role;
    private bool _isEditMode;
    private readonly UserList _userList = new();
    private User? _user;
    private string _theme;

    public UserMenu(string role, string language)
    {
        InitializeComponent();

        _language = language;
        _role = role;

        _userList.LoadJson();
        _user = _userList.GetUser(role);

        LoadData();
        ApplyLanguage(language);

        LanguageCombo.SelectedIndex = language == "ru" ? 0 : 1;

        _theme = _user.theme;
        ThemeComboBox.SelectedIndex = _theme == "Light" ? 0 : 1;
        
        ThemeComboBox.IsEnabled = false;
        LanguageCombo.IsEnabled = false;

        ThemeManager.ApplyTheme(_theme);
    }

    private static void ApplyLanguage(string lang)
    {
        var app = Application.Current;

        for (int i = app.Resources.MergedDictionaries.Count - 1; i >= 0; i--)
        {
            var dict = app.Resources.MergedDictionaries[i];

            if (dict.Source != null &&
                (dict.Source.OriginalString.Contains("Resources.ru.xaml") ||
                 dict.Source.OriginalString.Contains("Resources.en.xaml")))
            {
                app.Resources.MergedDictionaries.RemoveAt(i);
            }
        }

        app.Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(
                lang == "ru"
                    ? "Resources/Resources.ru.xaml"
                    : "Resources/Resources.en.xaml",
                UriKind.Relative)
        });
    }

    private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ThemeComboBox.SelectedIndex == 0)
            ThemeManager.ApplyTheme("Light");
        else
            ThemeManager.ApplyTheme("Dark");
    }

    private void SetEditMode(bool enabled)
    {
        _isEditMode = enabled;

        PasswordBox.IsReadOnly = !enabled;
        NameBox.IsReadOnly = !enabled;
        SurnameBox.IsReadOnly = !enabled;
        EmailBox.IsReadOnly = !enabled;

        ThemeComboBox.IsEnabled = enabled;
        LanguageCombo.IsEnabled = enabled;

        EditButton.Visibility = enabled ? Visibility.Collapsed : Visibility.Visible;
        SaveButton.Visibility = enabled ? Visibility.Visible : Visibility.Collapsed;
        CancelButton.Visibility = enabled ? Visibility.Visible : Visibility.Collapsed;
    }

    private void LoadData()
    {
        UsernameBox.Text = _role;
        PasswordBox.Text = _user.password;
        NameBox.Text = _user.name;
        SurnameBox.Text = _user.surname;
        EmailBox.Text = _user.email;

        ThemeComboBox.SelectedIndex = _user.theme == "Light" ? 0 : 1;
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        SetEditMode(true);
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(PasswordBox.Text) ||
            string.IsNullOrEmpty(NameBox.Text) ||
            string.IsNullOrEmpty(SurnameBox.Text) ||
            string.IsNullOrEmpty(EmailBox.Text) ||
            EmailError.Visibility == Visibility.Visible)
        {
            MessageBox.Show((string)FindResource("FillAllFields"));
            return;
        }

        string theme = ThemeComboBox.SelectedIndex == 0 ? "Light" : "Dark";
        this._theme = theme;
        _userList.LoadJson();
        _userList.DelUser(_role);
        _userList.AddUser(_role,
            PasswordBox.Text,
            NameBox.Text,
            SurnameBox.Text,
            EmailBox.Text,
            theme,
            _language);

        _userList.SaveJson();
        _user = _userList.GetUser(_role);

        SetEditMode(false);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        SetEditMode(false);
        LoadData();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        var main = new GunsList(_role, _language, _theme);
        main.Show();
        Close();
    }

    private void OnlyText(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z0-9.@]+$");
    }

    private void OnlyString(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-ЯёЁ]+$");
    }

    private void EmailCheck(object sender, RoutedEventArgs e)
    {
        EmailError.Visibility = Visibility.Hidden;

        var textBox = (TextBox)sender;
        string email = textBox.Text;

        if (email.Count(x => x == '@') != 1)
        {
            EmailError.Visibility = Visibility.Visible;
            return;
        }

        email = new string(email.SkipWhile(c => c != '@').Skip(1).ToArray());

        if (email is "gmail.com" or "yahoo.com" or "icloud.com"
            or "outlook.com" or "mail.ru"
            or "yandex.ru" or "yandex.by")
        {
            return;
        }

        EmailError.Visibility = Visibility.Visible;
    }

    private void LanguageCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageCombo.SelectedIndex == 0)
        {
            ApplyLanguage("ru");
            _language = "ru";
        }
        else
        {
            ApplyLanguage("en");
            _language = "en";
        }
    }
    
    private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}