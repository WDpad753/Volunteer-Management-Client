using BaseLogger;
using BaseLogger.Models;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Volunteer_Management_UI.MVVM.Views.Login;
using UtilityClass = BaseClass.MethodNameExtractor.FuncNameExtractor;


namespace Volunteer_Management_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        string _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config","App.config");
        string _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"tmp");
        
        public LogWriter logwriter;

        protected override void OnStartup(StartupEventArgs e)
        {
            logwriter = new(_configPath,_logPath);
            // This section is about selecting the View MainWindow and running the application:
            MainWindow = new LoginWindow();
            MainWindow.Show();
            logwriter.LogWrite("Application is running now", this.GetType().Name, UtilityClass.GetMethodName(), MessageLevels.Debug);
            base.OnStartup(e);
        }
    }
}
