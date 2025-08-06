using BaseClass.Base.Interface;
using BaseClass.Model;
using CustomMessageBox.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UI_Configuration_Editor.MVVM.Models;
using UI_Configuration_Editor.MVVM.Views.ConfigEditor;
using UI_Configuration_Editor.MVVM.Views.Shell_Window;
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

        public ObservableCollection<ComboOptions> ConfigOptions { get; } = new ObservableCollection<ComboOptions>()
        {
            new ComboOptions() { Name = "-- Select an option --", Tag = null },
            new ComboOptions() { Name = "Registry", Tag = "Reg" },
            new ComboOptions() { Name = "Environment", Tag = "Env" },
            new ComboOptions() { Name = "Environment File", Tag = "EnvFile" },
            new ComboOptions() { Name = "JSON File", Tag = "JsonFile" },
        };

        public ObservableCollection<EnvAccessMode> EnvTypeOptions { get; } = new ObservableCollection<EnvAccessMode>();

        //public ObservableCollection<ComboOptions> RegTypeOptions { get; } = new ObservableCollection<ComboOptions>()
        //{
        //    new ComboOptions() { Name = "-- Select an option --" , Tag = null},
        //    new ComboOptions() { Name = RegPath.User.ToString() , Tag = RegPath.User.ToString()},
        //    new ComboOptions() { Name = RegPath.Machine.ToString() , Tag = RegPath.Machine.ToString() },
        //};

        public ObservableCollection<RegPath> RegTypeOptions { get; } = new ObservableCollection<RegPath>();

        private string? _selectedRegType;
        public string? SelectedRegType
        {
            get => _selectedRegType;
            set => SetProperty(ref _selectedRegType, value);
        }

        private string _selectedPanel;
        public string SelectedPanel
        {
            get => _selectedPanel;
            set
            {
                if (SetProperty(ref _selectedPanel, value))
                {
                    if (value != "Reg")
                    {
                        SelectedRegType = null;
                    }
                }
            }
        }

        public DelegateCommand SaveCommand { get; private set; }

        public ConfigEditorViewModel(IContainerProvider containerProvider, IBase baseSettings)
        {
            _containerProvider = containerProvider;
            baseConfig = baseSettings;
            _canSave = true;

            foreach (EnvAccessMode value in Enum.GetValues(typeof(EnvAccessMode)))
            {
                if(value.ToString() != EnvAccessMode.Project.ToString() && value.ToString() != EnvAccessMode.File.ToString())
                {
                    EnvTypeOptions.Add(value);
                }
            }

            foreach (RegPath value in Enum.GetValues(typeof(RegPath)))
            {
                RegTypeOptions.Add(value);
            }

            SaveCommand = new DelegateCommand(OnSave).ObservesCanExecute(() => CanSave);
        }

        private void OnSave()
        {
            baseConfig.Messagebox.Show("Save Successful", DialogTitle.Info, DialogButtons.Ok);

            baseConfig.Logger.LogWrite("Save Successful", this.GetType().Name, FuncName.GetMethodName(), BaseLogger.Models.MessageLevels.Log);

            //// Close ShellWindow
            //Application.Current.Windows.OfType<ShellWindow>().FirstOrDefault()?.Close();
        }
    }
}
