using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI_Configuration_Editor.MVVM.Views.ConfigEditor;
using UI_Configuration_Editor.MVVM.Views.Shell_Window;
using UIBaseClass.MVVM.Base;
using UIBaseClass.MVVM.Base.Interface;
using UIBaseClass.MVVM.ViewBase;
using FuncName = BaseClass.MethodNameExtractor.FuncNameExtractor;

namespace UI_Configuration_Editor.MVVM.ViewModels
{
    public class ConfigEditorViewModel : ViewModelBase
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IBase baseConfig;
        private bool _canSave;
        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        public DelegateCommand SaveCommand { get; private set; }

        public ConfigEditorViewModel(IContainerProvider containerProvider, IBase baseSettings)
        {
            _containerProvider = containerProvider;
            baseConfig = baseSettings;
            _canSave = true;

            SaveCommand = new DelegateCommand(OnSave).ObservesCanExecute(() => CanSave);
        }

        private void OnSave()
        {
            baseConfig.Messagebox.Show();

            baseConfig.Logger.LogWrite("Save Successful", this.GetType().Name, FuncName.GetMethodName(), BaseLogger.Models.MessageLevels.Log);

            // Close ShellWindow
            Application.Current.Windows.OfType<ShellWindow>().FirstOrDefault()?.Close();
        }
    }
}
