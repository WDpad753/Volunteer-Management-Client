using BaseClass.Base.Interface;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI_Configuration_Editor.MVVM.Models;
using UI_Configuration_Editor.MVVM.ViewModels;
using UIBaseClass.Services.Navigation.Interface;

namespace UI_Configuration_Editor.MVVM.Views.ConfigEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ConfigEditor : UserControl
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IBase baseConfig;
        public ConfigEditor(IContainerProvider containerProvider, IBase baseSettings)
        {
            _containerProvider = containerProvider;
            baseConfig = baseSettings;

            InitializeComponent();
        }
    }
}