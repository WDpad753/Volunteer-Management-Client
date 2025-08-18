using BaseClass.Base;
using BaseClass.Base.Interface;
using BaseClass.Config;
using BaseClass.Helper;
using BaseClass.JSON;
using BaseClass.Model;
using BaseLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VMC_Unit_Tests.EncryptionTests
{
    [TestFixture]
    internal class EncryptionTests
    {
        private IBase? baseConfig;
        private LogWriter logwriter;
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

            logwriter = new LogWriter(Mainconfigpath, logpath);

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
            else if(importEncryptedTestData)
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
            }
            else
            {
                Assert.Fail("Unable to Obtain a Value from Configuration File");
            }
        }

        [Test, Order(2)]
        public void UserRegistryDataReadTest()
        {
            string val = "Hello_Test";

            //string? res = registryHandler.RegistryRead("RegistryPath");
            List<byte[]>? res = registryHandler.RegistryValGet();

            //if (res != null)
            //{
            //    ;

            //    Assert.That( == val, "Value is not equal");
            //}
            //else
            //{
            //    Assert.Fail("Unable to Obtain a Value from Configuration File");
            //}
        }

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
