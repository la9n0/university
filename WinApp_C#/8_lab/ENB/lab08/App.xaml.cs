using System.Windows;

namespace ENB_project
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаём БД и всю инфраструктуру если её нет
            try
            {
                Database.EnsureCreated();
            }
            catch
            {
                // Ошибка уже показана в Database.EnsureCreated через MessageBox
                Shutdown(1);
                return;
            }

            var login = new LogIn();
            login.Show();
        }
    }
}