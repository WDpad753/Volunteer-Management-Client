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
using UI_Configuration_Editor.MVVM.ViewModels;
using UIBaseClass.MessageBox;
using UIBaseClass.MVVM.Base;
using UIBaseClass.Services.Navigation.Interface;
using static UIBaseClass.MessageBox.CustomMessageBox;

namespace UI_Configuration_Editor.MVVM.Views.ConfigEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ConfigEditor : UserControl
    {
        public ConfigEditor()
        {
            InitializeComponent();
        }
    }
}