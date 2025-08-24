using BaseClass.Base.Interface;
using BaseClass.Helper;
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
using System.Windows.Data;
using UI_Configuration_Editor.MVVM.Models;
using UI_Configuration_Editor.MVVM.Views.ConfigEditor;
using UI_Configuration_Editor.MVVM.Views.Shell_Window;
using UIBaseClass.MVVM.ViewBase;


namespace UI_Configuration_Editor.MVVM.ViewModels
{
    public class ConfigEditorViewModel : ViewModelBase
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IBase baseConfig;
        public ConfigEditorModel ConfigEditorMdl { get; }
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

        public ObservableCollection<ComboOptions> DatabaseOptions { get; } = new ObservableCollection<ComboOptions>()
        {
            new ComboOptions() { Name = "-- Select an DB Type --", Tag = null },
            new ComboOptions() { Name = "MSSQL", Tag = "SQLSERV" },
            new ComboOptions() { Name = "PostGres", Tag = "PG" },
            new ComboOptions() { Name = "SQLite", Tag = "SQLITE" },
        };

        public ObservableCollection<EncryptionMode> EncTypeOptions { get; } = new ObservableCollection<EncryptionMode>();
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

        private string? _selectedEnvType;
        public string? SelectedEnvType
        {
            get => _selectedEnvType;
            set => SetProperty(ref _selectedEnvType, value);
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
                        ConfigEditorMdl.RegPath = "";
                    }
                    if(value != "Env")
                    {
                        SelectedEnvType = null;
                    }
                }
            }
        }

        private string _selectedEncType;
        public string SelectedEncType
        {
            get => _selectedEncType;
            set => SetProperty(ref _selectedEncType, value);
        }

        private string _selectedDBPanel;
        public string SelectedDBPanel
        {
            get => _selectedDBPanel;
            set
            {
                if (SetProperty(ref _selectedDBPanel, value))
                {
                    if (value != "SQLSERV")
                    {
                        ConfigEditorMdl.DBServer = "";
                        ConfigEditorMdl.DBFailover = "";
                        ConfigEditorMdl.DataBase = "";
                        ConfigEditorMdl.DBUserName = "";
                        ConfigEditorMdl.DBPassword = "";
                    }
                    if (value != "PG")
                    {
                        ConfigEditorMdl.DBServer = "";
                        ConfigEditorMdl.DataBase = "";
                        ConfigEditorMdl.DBUserName = "";
                        ConfigEditorMdl.DBPassword = "";
                    }
                    if (value != "SQLITE")
                    {
                        ConfigEditorMdl.DBPath = "";
                        ConfigEditorMdl.DBPassword = "";
                    }
                }
            }
        }

        private bool _IsPasswordBoxEnabled = false;
        public bool IsPasswordBoxEnabled
        {
            get => _IsPasswordBoxEnabled;
            set
            {
                if (ConfigEditorMdl.DBPassword != null)
                {
                    ConfigEditorMdl.DBPassword = "";
                    SetProperty(ref _IsPasswordBoxEnabled, value);
                }
                else
                {
                    SetProperty(ref _IsPasswordBoxEnabled, value);
                }
            }
        }

        public DelegateCommand SaveCommand { get; private set; }
        //public DelegateCommand DBLitePassCommand { get; private set; }

        public ConfigEditorViewModel(IContainerProvider containerProvider, IBase baseSettings)
        {
            _containerProvider = containerProvider;
            baseConfig = baseSettings;
            //_canSave = true;

            ConfigEditorMdl = new ConfigEditorModel();

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

            foreach (EncryptionMode value in Enum.GetValues(typeof(EncryptionMode)))
            {
                EncTypeOptions.Add(value);
            }

            ConfigEditorMdl.PropertyChanged += (s, e) => UpdateCanSave();
            SaveCommand = new DelegateCommand(OnSave).ObservesCanExecute(() => CanSave);
            //DBLitePassCommand = new DelegateCommand(PassEnabled).ObservesCanExecute(() => IsPasswordBoxEnabled);
        }

        private void UpdateCanSave()
        {
            if(SelectedPanel == "Reg")
            {
                if (SelectedDBPanel == "SQLSERV")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.RegPath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.RegKey) &&
                              !string.IsNullOrWhiteSpace(SelectedRegType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBServer) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBFailover) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DataBase) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBUserName) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
                else if (SelectedDBPanel == "PG")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.RegPath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.RegKey) &&
                              !string.IsNullOrWhiteSpace(SelectedRegType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBServer) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DataBase) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBUserName) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
                else if (SelectedDBPanel == "SQLITE")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.RegPath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.RegKey) &&
                              !string.IsNullOrWhiteSpace(SelectedRegType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
            }
            else if(SelectedPanel == "Env")
            {
                if (SelectedDBPanel == "SQLSERV")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.EnvKey) &&
                              !string.IsNullOrWhiteSpace(SelectedEnvType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBServer) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBFailover) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DataBase) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBUserName) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
                else if (SelectedDBPanel == "PG")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.EnvKey) &&
                              !string.IsNullOrWhiteSpace(SelectedEnvType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBServer) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DataBase) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBUserName) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
                else if (SelectedDBPanel == "SQLITE")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.EnvKey) &&
                              !string.IsNullOrWhiteSpace(SelectedEnvType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
            }
            else if (SelectedPanel == "EnvFile")
            {
                if (SelectedDBPanel == "SQLSERV")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.EnvFilePath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.EnvFileKey) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBServer) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBFailover) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DataBase) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBUserName) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
                else if (SelectedDBPanel == "PG")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.EnvFilePath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.EnvFileKey) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBServer) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DataBase) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBUserName) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
                else if (SelectedDBPanel == "SQLITE")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.EnvFilePath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.EnvFileKey) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
            }
            else if(SelectedPanel == "JsonFile")
            {
                if (SelectedDBPanel == "SQLSERV")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.JSONFilePath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.JSONFileKey) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBServer) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBFailover) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DataBase) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBUserName) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
                else if (SelectedDBPanel == "PG")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.JSONFilePath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.JSONFileKey) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBServer) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DataBase) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBUserName) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
                else if (SelectedDBPanel == "SQLITE")
                {
                    CanSave = !string.IsNullOrWhiteSpace(SelectedEncType) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.JSONFilePath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.JSONFileKey) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPath) &&
                              !string.IsNullOrWhiteSpace(ConfigEditorMdl.DBPassword);
                }
            }
        }

        private void OnSave()
        {
            if(!string.IsNullOrWhiteSpace(SelectedRegType))
            {
                RegPath? reg = RegModeSelector.RegSelector(SelectedRegType);
            }



            baseConfig.Messagebox?.Show("Save Successful.", DialogTitle.Info, DialogButtons.Ok);

            baseConfig.Logger?.LogBase("Save Successful");
        }
    }
}
