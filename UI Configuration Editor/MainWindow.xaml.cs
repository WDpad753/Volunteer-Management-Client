using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIBaseClass.MessageBox;
using static UIBaseClass.MessageBox.CustomMessageBox;

namespace UI_Configuration_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CustomMessageBox messagebox;

        public MainWindow()
        {
            InitializeComponent();
            messagebox = new CustomMessageBox();
        }

        private void RoundButton_Click(object sender, RoutedEventArgs e)
        {
            messagebox.Show("Button Pressed", CustomDialogTitle.Warning, CustomMessageBox.CustomDialogButton.Ok, CustomMessageBox.CustomDialogButton.Cancel);

            //MessageBox.Show("Button Pressed", "Trial", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }
    }
}