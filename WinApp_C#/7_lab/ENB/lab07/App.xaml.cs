using System.Windows;

namespace lab07
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var login = new LogIn();
            login.Show();
        }
    }
}