using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_Configuration_Editor.MVVM.Models
{
    public class ConfigEditorModel : BindableBase
    {
        private string? _RegPath;
        public string? RegPath
        {
            get => _RegPath;
            set
            {
                SetProperty(ref _RegPath, value);
            }
        }
        private string? _RegKey;
        public string? RegKey
        {
            get => _RegKey;
            set
            {
                SetProperty(ref _RegKey, value);
            }
        }
        private string? _EnvKey;
        public string? EnvKey
        {
            get => _EnvKey;
            set
            {
                SetProperty(ref _EnvKey, value);
            }
        }
        private string? _EnvFilePath;
        public string? EnvFilePath
        {
            get => _EnvFilePath;
            set
            {
                SetProperty(ref _EnvFilePath, value);
            }
        }
        private string? _EnvFileKey;
        public string? EnvFileKey
        {
            get => _EnvFileKey;
            set
            {
                SetProperty(ref _EnvFileKey, value);
            }
        }
        private string? _JSONFilePath;
        public string? JSONFilePath
        {
            get => _JSONFilePath;
            set
            {
                SetProperty(ref _JSONFilePath, value);
            }
        }
        private string? _JSONFileKey;
        public string? JSONFileKey
        {
            get => _JSONFileKey;
            set
            {
                SetProperty(ref _JSONFileKey, value);
            }
        }
        private string? _DBServer;
        public string? DBServer
        {
            get => _DBServer;
            set
            {
                SetProperty(ref _DBServer, value);
            }
        }
        private string? _DBFailover;
        public string? DBFailover
        {
            get => _DBFailover;
            set
            {
                SetProperty(ref _DBFailover, value);
            }
        }
        private string? _DataBase;
        public string? DataBase
        {
            get => _DataBase;
            set
            {
                SetProperty(ref _DataBase, value);
            }
        }
        private string? _DBUserName;
        public string? DBUserName
        {
            get => _DBUserName;
            set
            {
                SetProperty(ref _DBUserName, value);
            }
        }
        private string? _DBPassword;
        public string? DBPassword
        {
            get => _DBPassword;
            set
            {
                SetProperty(ref _DBPassword, value);
            }
        }
        private string? _DBPath;
        public string? DBPath
        {
            get => _DBPath;
            set
            {
                SetProperty(ref _DBPath, value);
            }
        }
    }
}
