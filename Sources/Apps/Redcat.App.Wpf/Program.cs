using System;

namespace Redcat.App.Wpf
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App();
            app.InitializeComponent();
            app.Run(new MainWindow());
        }
    }
}
