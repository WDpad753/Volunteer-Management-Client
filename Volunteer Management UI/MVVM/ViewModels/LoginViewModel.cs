using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volunteer_Management_UI.MVVM.Views.ShellWindow;
using Volunteer_Management_UI.MVVM.Views;
using System.Windows;
using System.Windows.Input;
using FuncName = BaseClass.MethodNameExtractor.FuncNameExtractor;
using UIBaseClass.MVVM.ViewBase;
using BaseClass.Base.Interface;

namespace Volunteer_Management_UI.MVVM.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IBase _baseSettings;

        private bool _canLogin;
        public bool CanLogin
        {
            get => _canLogin;
            //set => SetProperty(ref _canLogin, value);
            set => SetProperty(ref _canLogin, value);
        }

        public DelegateCommand LoginSuccessCommand { get; private set; }

        public LoginViewModel(IContainerProvider containerProvider, IBase baseSettings)
        {
            _containerProvider = containerProvider;
            _baseSettings = baseSettings;
            _canLogin = true;

            LoginSuccessCommand = new DelegateCommand(OnLoginSuccess).ObservesCanExecute(() => CanLogin);
        }

        private void OnLoginSuccess()
        {
            var main = _containerProvider.Resolve<MainWindow>();

            _baseSettings.Logger.LogWrite("Login Successful", this.GetType().Name, FuncName.GetMethodName(), BaseLogger.Models.MessageLevels.Log);

            // Close ShellWindow
            Application.Current.Windows.OfType<ShellWindow>().FirstOrDefault()?.Close();

            main.Show();
        }
    }
}
