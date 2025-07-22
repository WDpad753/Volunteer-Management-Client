using BaseClass.Base;
using BaseClass.Base.Interface;
using BaseLogger;
using BaseLogger.Models;
using Prism.Unity;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using UI_Configuration_Editor.MVVM.Views.ConfigEditor;
using UI_Configuration_Editor.MVVM.Views.Shell_Window;
using UIBaseClass.MessageBox;
using UIBaseClass.Services.Navigation;
using UIBaseClass.Services.Navigation.Interface;
using static UIBaseClass.MessageBox.CustomMessageBox;
using FuncName = BaseClass.MethodNameExtractor.FuncNameExtractor;

namespace UI_Configuration_Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        string _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "App.config");
        string _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tmp");
        IBase? baseSettings = null;
        LogWriter? logwriter = null;
        CustomMessageBox? messageBox = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupExceptionHandling();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Create view resolver
            Func<Type, Type> viewResolver = (viewModelType) =>
            {
                var viewName = viewModelType.FullName.Replace("ViewModel", "View");
                return Type.GetType(viewName); // Or use custom mapping logic
            };

            // View factory using Prism container
            Func<Type, object> viewFactory = (viewType) => Container.Resolve(viewType);

            // Register views
            containerRegistry.RegisterForNavigation<ConfigEditor>();
            containerRegistry.Register<ShellWindow>();

            // Register ViewNavigation
            var viewNavigation = new ViewNavigation(Container, viewFactory, viewResolver);
            containerRegistry.RegisterInstance<IViewNavigation>(viewNavigation);
            containerRegistry.RegisterInstance(viewNavigation);

            // Register BaseSettings
            logwriter = new(_configPath, _logPath);
            messageBox = new();
            baseSettings = new BaseSettings()
            {
                Logger = logwriter,
                Messagebox = messageBox
            };
            containerRegistry.RegisterInstance(baseSettings);
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";

            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
                messageBox.Show(message, CustomDialogTitle.Error, CustomDialogButton.Ok);
            }
            catch (Exception ex)
            {
                logwriter.LogWrite($"Exception in LogUnhandledException: {ex}", this.GetType().Name, FuncName.GetMethodName(), MessageLevels.Fatal);
                messageBox.Show($"Critical error occurred while handling an exception.Exception: {ex}", CustomDialogTitle.Error, CustomDialogButton.Ok);
            }
            finally
            {
                logwriter.LogWrite($"Message: {message}; Exception: {exception}.", this.GetType().Name, FuncName.GetMethodName(), MessageLevels.Fatal);
                messageBox.Show($"Message: {message}; Exception: {exception}.", CustomDialogTitle.Error, CustomDialogButton.Ok);
            }
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }
    }
}
