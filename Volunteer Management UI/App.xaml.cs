using BaseLogger;
using BaseLogger.Models;
using Prism.Unity;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using UIBaseClass.Services.Navigation;
using UIBaseClass.Services.Navigation.Interface;
using Volunteer_Management_UI.MVVM.Views;
using Volunteer_Management_UI.MVVM.Views.Login;
using Volunteer_Management_UI.MVVM.Views.Registration;
using Volunteer_Management_UI.MVVM.Views.ShellWindow;
using FuncName = BaseClass.MethodNameExtractor.FuncNameExtractor;
using Microsoft.Win32;
using Prism.Ioc;
using Prism.Unity;
using Prism.Navigation.Regions;
using BaseClass.Helper;
using UIBaseClass.MVVM.Base;

namespace Volunteer_Management_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    //public partial class App : PrismApplication
    //{
    //    protected override Window CreateShell()
    //    {
    //        // Return the first window to show (ShellWindow)
    //        return Container.Resolve<ShellWindow>();
    //    }

    //    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    //    {
    //        // Register services
    //        containerRegistry.RegisterSingleton<IViewNavigation, ViewNavigation>();

    //        // Register views for navigation
    //        containerRegistry.RegisterForNavigation<Login>();
    //        containerRegistry.RegisterForNavigation<Registration>();
    //        containerRegistry.RegisterForNavigation<MainWindow>();

    //        containerRegistry.Register<ShellWindow>();
    //    }
    //}


    public partial class App : PrismApplication
    {
        string _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "App.config");
        string _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tmp");

        public LogWriter logwriter;
        public BaseSettings baseSettings;


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
            containerRegistry.RegisterForNavigation<Login>();
            containerRegistry.RegisterForNavigation<Registration>();
            containerRegistry.RegisterForNavigation<MainWindow>();
            containerRegistry.Register<ShellWindow>();

            // Register ViewNavigation
            var viewNavigation = new ViewNavigation(Container, viewFactory, viewResolver);
            containerRegistry.RegisterInstance<IViewNavigation>(viewNavigation);
            containerRegistry.RegisterInstance(viewNavigation);

            // Register BaseSettings
            logwriter = new(_configPath, _logPath);
            baseSettings = new BaseSettings()
            {
                Logger = logwriter
            };
            containerRegistry.RegisterInstance(baseSettings);
        }

        //protected override void ConfigureViewModelLocator()
        //{
        //    base.ConfigureViewModelLocator();

        //    ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
        //    {
        //        var viewName = viewType.FullName;          // e.g. YourApp.Views.Login.Login
        //        var assemblyName = viewType.Assembly.FullName;

        //        // Replace ".Views." with ".ViewModels."
        //        string? ViewName = StringHandler.RemoveDublicatesInString(viewName, ".");
        //        var viewModelName = ViewName.Replace(".Views.", ".ViewModels.")+"ViewModel";

        //        return Type.GetType($"{viewModelName}, {assemblyName}");
        //    });
        //}

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";

            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
            }
            catch (Exception ex)
            {
                logwriter.LogWrite($"Exception in LogUnhandledException: {ex}", this.GetType().Name, FuncName.GetMethodName(), MessageLevels.Fatal);

            }
            finally
            {
                logwriter.LogWrite($"Message: {message}; Exception: {exception}.", this.GetType().Name, FuncName.GetMethodName(), MessageLevels.Fatal);
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
            SetupExceptionHandling();
            return Container.Resolve<ShellWindow>();
        }
    }
}
