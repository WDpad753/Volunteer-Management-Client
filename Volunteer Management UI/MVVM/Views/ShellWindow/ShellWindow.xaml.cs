using BaseClass.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UIBaseClass.Services.Navigation.Interface;
using Volunteer_Management_UI.MVVM.ViewModels;
using Volunteer_Management_UI.MVVM.Views.Registration;

namespace Volunteer_Management_UI.MVVM.Views.ShellWindow
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        //private readonly IRegionManager _regionManager;
        private readonly IViewNavigation _navigation;
        private readonly IBase _baseSettings;

        //public ShellWindow(IRegionManager regionManager)
        public ShellWindow(IViewNavigation navigation, IBase baseSettings)
        {
            InitializeComponent();
            //_regionManager = regionManager;
            _navigation = navigation;
            _baseSettings = baseSettings;

            // Navigate to LoginView on startup
            //regionManager.RequestNavigate("AuthRegion", nameof(Login));
            //navigation.NavigateTo<LoginViewModel>(default);

            DataContext = navigation; // critical to bind CurrentView property

            navigation.NavigateTo<LoginViewModel>();
        }

        private void LoginView_Click(object sender, RoutedEventArgs e)
        {
            //_regionManager.RequestNavigate("AuthRegion", nameof(Login));
            _navigation.NavigateTo<LoginViewModel>();
        }

        private void RegistrationView_Click(object sender, RoutedEventArgs e)
        {
            //_regionManager.RequestNavigate("AuthRegion", nameof(Registration));
            _navigation.NavigateTo<RegistrationViewModel>();
        }
    }
}
