using BaseClass.Base;
using BaseClass.Base.Interface;
using BaseClass.BaseRegistry;
using BaseClass.Config;
using BaseClass.Helper;
using BaseClass.JSON;
using BaseClass.Model;
using BaseLogger;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VMC_Unit_Tests.EncryptionTests
{
    [TestFixture]
    internal class EncryptionTests
    {
        private IBase? baseConfig;
        private ILogger? logwriter;
        private ConfigHandler configReader;
        private JSONFileHandler jsonFileHandler;
        private EnvFileHandler envFileHandler;
        private EnvHandler envHandler;
        private RegistryHandler registryHandler;
        private EncryptionModel? _encModel;
        private string logpath;
        private string configPath;
        private string testValue;
        private bool importEncryptedData;
        private bool importEncryptedTestData;

        [SetUp]
        public void Setup()
        {
            string Mainconfigpath = @$"{AppDomain.CurrentDomain.BaseDirectory}Config\MainAppTest.config";

            logpath = @$"{AppDomain.CurrentDomain.BaseDirectory}TempLogs\";
            configPath = Mainconfigpath;

            if (Directory.Exists(logpath))
            {
                Directory.Delete(logpath, true); // Ensure the log directory is clean before starting the test
            }

            logwriter = new Logger(Mainconfigpath, logpath);

            baseConfig = new BaseSettings()
            {
                Logger = logwriter,
                ConfigPath = Mainconfigpath,
            };

            _encModel = new EncryptionModel()
            {
                ConfigKey = "RegistryPath",
                PathKey = ProtectedData.Protect(Encoding.UTF8.GetBytes("SOFTWARE\\AppTest"), null, DataProtectionScope.CurrentUser),
                Key = ProtectedData.Protect(Encoding.UTF8.GetBytes("Test"), null, DataProtectionScope.CurrentUser),
                Keys = new List<byte[]>() { ProtectedData.Protect(Encoding.UTF8.GetBytes("Test"), null, DataProtectionScope.CurrentUser) },
                EnvType = ProtectedData.Protect(ProtectedData.Protect(Encoding.UTF8.GetBytes("None"), null, DataProtectionScope.CurrentUser), null, DataProtectionScope.CurrentUser),
                RegType = ProtectedData.Protect(Encoding.UTF8.GetBytes("User"), null, DataProtectionScope.CurrentUser),
            };

            configReader = new(baseConfig);
            jsonFileHandler = new(baseConfig);
            envFileHandler = new(baseConfig);
            envHandler = new(baseConfig);
            registryHandler = new(baseConfig, _encModel);
            baseConfig.ConfigHandler = configReader;
            baseConfig.JSONFileHandler = jsonFileHandler;
            baseConfig.EnvFileHandler =  envFileHandler;
            baseConfig.EnvHandler = envHandler;
            baseConfig.RegistryHandler = registryHandler;

            testValue = "TestPath";

            if (importEncryptedData)
            {
                ConvertStringToBytes();
                importEncryptedData = false;
            }
            else if (importEncryptedTestData)
            {
                testValue = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes("SOFTWARE\\AppTest"), null, DataProtectionScope.CurrentUser));
                ConvertStringToBytesReg();
            }
            else
            {
                ConvertBytesToString();
            }
        }

        [Test, Order(0)]
        public void UserRegistryPathReadTest()
        {
            string val = "TestPath";

            string? res = registryHandler.RegistryRead("RegistryPath");

            if (res != null)
            {
                Assert.That(res == val, "Value is not equal");
                importEncryptedData = true;
                testValue = val;
            }
            else
            {
                Assert.Fail("Unable to Obtain a Value from Configuration File");
            }
        }

        [Test, Order(1)]
        public void UserRegistryPathReadTest2()
        {
            string val = "TestPath";

            string? res = registryHandler.RegistryRead("RegistryPath");

            if (res != null)
            {
                Assert.That(res == val, "Value is not equal");
                importEncryptedTestData = true;
                testValue = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes("SOFTWARE\\AppTest"), null, DataProtectionScope.CurrentUser));
            }
            else
            {
                Assert.Fail("Unable to Obtain a Value from Configuration File");
            }
        }

        [Test, Order(2)]
        public void UserRegistryDataReadTest()
        {
            if (!importEncryptedTestData)
            {
                testValue = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes("SOFTWARE\\AppTest"), null, DataProtectionScope.CurrentUser));
                registryHandler.RegistrySave("RegistryPath", testValue);
            }

            string val = "Hello_Test";

            //string? res = registryHandler.RegistryRead("RegistryPath");
            List<byte[]>? res = registryHandler.RegistryValGet();

            if (res != null)
            {
                string result = Encoding.UTF8.GetString(ProtectedData.Unprotect(res[0], null, DataProtectionScope.CurrentUser));
                Assert.That(result == val, "Value is not equal");
            }
            else
            {
                Assert.Fail("Unable to Obtain a Value from Configuration File");
            }
        }

        //[Test, Order(3)]
        //public void UserRegistryDataSaveTest()
        //{
        //    if (!importEncryptedTestData)
        //    {
        //        testValue = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes("SOFTWARE\\AppTest"), null, DataProtectionScope.CurrentUser));
        //        registryHandler.RegistrySave("RegistryPath", testValue);
        //    }

        //    List<object>? Datas = new List<object>();
        //    string val = "Hello_Test2";

        //    byte[]? val_byte = Encoding.UTF8.GetBytes(val);
        //    val_byte = ProtectedData.Protect(val_byte, null, DataProtectionScope.CurrentUser);
        //    string value = Convert.ToBase64String(val_byte);
        //    Datas.Add(value);

        //    registryHandler.RegistryValSave(Datas, RegistryValueKind.String);

        //    List<byte[]>? res = registryHandler.RegistryValGet();

        //    if (res != null)
        //    {
        //        string result = Encoding.UTF8.GetString(ProtectedData.Unprotect(res[0], null, DataProtectionScope.CurrentUser));
        //        Assert.That(result == val, "Value is not equal");
        //    }
        //    else
        //    {
        //        Assert.Fail("Unable to Obtain a Value from Configuration File");
        //    }
        //}

        //[Test, Order(4)]
        //public void UserEnvDataReadTest()
        //{

        //}

        //[Test, Order(5)]
        //public void AESEncryptionCreationWithRegistryTest()
        //{
        //    _encModel = new EncryptionModel()
        //    {
        //        Keys = new List<byte[]>() { ProtectedData.Protect(Encoding.UTF8.GetBytes("KeyA"), null, DataProtectionScope.CurrentUser),
        //                                    ProtectedData.Protect(Encoding.UTF8.GetBytes("KeyIV"), null, DataProtectionScope.CurrentUser)},
        //    };

        //    encryption = BaseEncryption.GetEncryption(EncryptionMode.AES, ConfigAccessMode.Registry, _encModel, baseConfig);
        //}

        private void ConvertStringToBytes()
        {
            registryHandler.RegistrySave("RegistryPath", Encoding.UTF8.GetBytes(testValue));
        }

        private void ConvertStringToBytesReg()
        {
            registryHandler.RegistrySave("RegistryPath", testValue);
        }

        private void ConvertBytesToString()
        {
            registryHandler.RegistrySave("RegistryPath", testValue);
        }
    }
}