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

        private void StackPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as StackPanel;
            if (panel == null)
                return;

            // Ensure it has a TranslateTransform
            if (!(panel.RenderTransform is TranslateTransform transform))
            {
                transform = new TranslateTransform();
                panel.RenderTransform = transform;
            }

            // Define animation duration
            var duration = TimeSpan.FromSeconds(0.4);

            // Slide animation
            var slide = new DoubleAnimation
            {
                Duration = duration,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            // Fade animation
            var fade = new DoubleAnimation
            {
                Duration = duration
            };

            if (panel.Visibility == Visibility.Visible)
            {
                slide.From = 100;
                slide.To = 0;
                fade.From = 0;
                fade.To = 1;
            }
            else
            {
                slide.From = 0;
                slide.To = 100;
                fade.From = 1;
                fade.To = 0;

                // Set visibility after animation completes
                fade.Completed += (s, e) => panel.Visibility = Visibility.Collapsed;
            }

            // Begin animations
            transform.BeginAnimation(TranslateTransform.XProperty, slide);
            panel.BeginAnimation(UIElement.OpacityProperty, fade);
        }
    }
}