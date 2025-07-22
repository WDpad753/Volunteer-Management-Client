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
using UI_Configuration_Editor.MVVM.ViewModels;
using UIBaseClass.Services.Navigation.Interface;

namespace UI_Configuration_Editor.MVVM.Views.Shell_Window
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        private readonly IViewNavigation _navigation;
        private readonly IBase _baseSettings;
        private static double currentheight;
        private static double currentwidth;

        public double currentHeight
        {
            get { return currentheight; }
            set { currentheight = value; }
        }

        public double currentWidth
        {
            get { return currentwidth; }
            set { currentwidth = value; }
        }

        public ShellWindow(IViewNavigation navigation, IBase baseSettings)
        {
            InitializeComponent();

            //_regionManager = regionManager;
            _navigation = navigation;
            _baseSettings = baseSettings;

            MouseLeftButtonDown += delegate { DragMove(); };

            currentHeight = this.Height;
            currentWidth = this.Width;

            // Setting the Maximum Height and Width of the Main Window:
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

            this.Closed += Window_Closed;  // Handle window close cleanup

            DataContext = navigation; // critical to bind CurrentView property

            navigation.NavigateTo<ConfigEditorViewModel>();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                //isButtonClicked = true;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // If the DataContext is IDisposable, dispose it to stop the timer
            if (DataContext is IDisposable disposable)
            {
                disposable.Dispose(); // Dispose of the view model (which will stop the timer and cleanup)
            }

            // Force shutdown after closing the main window
            this.Close();

            // Explicitly shut down the application
            Application.Current.Shutdown();
        }

        // Handle cleanup when the window is closed
        private void Window_Closed(object sender, EventArgs e)
        {
            if (DataContext is IDisposable disposable)
            {
                disposable.Dispose(); // Dispose of the view model when the window is closed
            }
        }
    }
}
