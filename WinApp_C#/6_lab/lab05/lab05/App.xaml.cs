using System;
using System.Windows;
using System.Windows.Input;

namespace lab05
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var cursorStream = Application.GetResourceStream(
                    new Uri("Resources/Arrow.cur", UriKind.Relative)) ?.Stream;
            if (cursorStream != null)
            {
                var cursor = new Cursor(cursorStream);

                Mouse.OverrideCursor = cursor;
            }

            var login = new LoginWindow();
            login.Show();
        }
    }
}