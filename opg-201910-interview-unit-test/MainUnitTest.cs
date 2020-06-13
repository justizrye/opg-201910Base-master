using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using opg_201910_interview.Controllers;
using opg_201910_interview.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace opg_201910_interview_unit_test
{
    [TestClass]
    public class MainUnitTest
    {
        IConfiguration Configuration { get; set; }
        void LoadConfigFile(string appSettingsFile)
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(appSettingsFile);
            Configuration = builder.Build();
        }

        [TestMethod]
        public void ClientA_LoadClientSettingsTest()
        {
            LoadConfigFile("appsettings.Development.ClientA.json");

            HomeController homeController = new HomeController(null, Configuration);

            ClientSettings expected = new ClientSettings
            {
                ClientId = "1001",
                FileDirectoryPath = "UploadFiles/ClientA",
                FilePattern = "*-????-??-??.xml",
                Delimiter = "-",
                SortRules = new Dictionary<string, string>()
            };
            expected.SortRules.Add("shovel", "1");
            expected.SortRules.Add("waghor", "2");
            expected.SortRules.Add("blaze", "3");
            expected.SortRules.Add("discus", "4");

            ClientSettings actual = homeController.GetClientSettings();

            Assert.AreEqual(expected.ClientId, actual.ClientId);
            Assert.AreEqual(expected.FileDirectoryPath, actual.FileDirectoryPath);
            Assert.AreEqual(expected.FilePattern, actual.FilePattern);
            Assert.AreEqual(expected.Delimiter, actual.Delimiter);

            Assert.AreEqual(expected.SortRules["shovel"], actual.SortRules["shovel"]);
            Assert.AreEqual(expected.SortRules["waghor"], actual.SortRules["waghor"]);
            Assert.AreEqual(expected.SortRules["blaze"], actual.SortRules["blaze"]);
            Assert.AreEqual(expected.SortRules["discus"], actual.SortRules["discus"]);
        }

        [TestMethod]
        public void ClientB_LoadClientSettingsTest()
        {
            LoadConfigFile("appsettings.Development.ClientB.json");

            HomeController homeController = new HomeController(null, Configuration);

            ClientSettings expected = new ClientSettings
            {
                ClientId = "1002",
                FileDirectoryPath = "UploadFiles/ClientB",
                FilePattern = "*_????????.xml",
                Delimiter = "_",
                SortRules = new Dictionary<string, string>()
            };
            expected.SortRules.Add("orca", "1");
            expected.SortRules.Add("widget", "2");
            expected.SortRules.Add("eclair", "3");
            expected.SortRules.Add("talon", "4");

            ClientSettings actual = homeController.GetClientSettings();

            Assert.AreEqual(expected.ClientId, actual.ClientId);
            Assert.AreEqual(expected.FileDirectoryPath, actual.FileDirectoryPath);
            Assert.AreEqual(expected.FilePattern, actual.FilePattern);
            Assert.AreEqual(expected.Delimiter, actual.Delimiter);

            Assert.AreEqual(expected.SortRules["orca"], actual.SortRules["orca"]);
            Assert.AreEqual(expected.SortRules["widget"], actual.SortRules["widget"]);
            Assert.AreEqual(expected.SortRules["eclair"], actual.SortRules["eclair"]);
            Assert.AreEqual(expected.SortRules["talon"], actual.SortRules["talon"]);
        }

        [TestMethod]
        public void ClientA_LoadClientFilesTest()
        {
            LoadConfigFile("appsettings.Development.ClientA.json");

            HomeController homeController = new HomeController(null, Configuration);

            IList<ClientFile> expectedList = new List<ClientFile>();
            expectedList.Add(new ClientFile
            {
                Name = "blaze",
                Date = DateTime.Parse("2018-05-01"),
                SortIndex = 3,
                Filename = "blaze-2018-05-01.xml"
            });
            expectedList.Add(new ClientFile
            {
                Name = "blaze",
                Date = DateTime.Parse("2019-01-23"),
                SortIndex = 3,
                Filename = "blaze-2019-01-23.xml"
            });
            expectedList.Add(new ClientFile
            {
                Name = "discus",
                Date = DateTime.Parse("2015-12-16"),
                SortIndex = 4,
                Filename = "discus-2015-12-16.xml"
            });
            expectedList.Add(new ClientFile
            {
                Name = "shovel",
                Date = DateTime.Parse("2000-01-01"),
                SortIndex = 1,
                Filename = "shovel-2000-01-01.xml"
            });
            expectedList.Add(new ClientFile
            {
                Name = "waghor",
                Date = DateTime.Parse("2012-06-20"),
                SortIndex = 2,
                Filename = "waghor-2012-06-20.xml"
            });

            ClientFile[] expectedArray = expectedList.ToArray();

            ClientFile[] actualArray = homeController.GetClientFiles(homeController.GetClientSettings()).ToArray();

            for (int i = 0; i < expectedArray.Length; i++)
            {
                ClientFile expected = expectedArray[i];
                ClientFile actual = actualArray[i];

                Assert.AreEqual(expected.Name, actual.Name);
                Assert.AreEqual(expected.Date, actual.Date);
                Assert.AreEqual(expected.SortIndex, actual.SortIndex);
                Assert.AreEqual(expected.Filename, actual.Filename);
            }
        }

        [TestMethod]
        public void ClientB_LoadClientFilesTest()
        {
            LoadConfigFile("appsettings.Development.ClientB.json");

            HomeController homeController = new HomeController(null, Configuration);

            IList<ClientFile> expectedList = new List<ClientFile>();
            expectedList.Add(new ClientFile
            {
                Name = "eclair",
                Date = DateTime.ParseExact("20180908", "yyyyMMdd", CultureInfo.InvariantCulture),
                SortIndex = 3,
                Filename = "eclair_20180908.xml"
            });
            expectedList.Add(new ClientFile
            {
                Name = "orca",
                Date = DateTime.ParseExact("20130219", "yyyyMMdd", CultureInfo.InvariantCulture),
                SortIndex = 1,
                Filename = "orca_20130219.xml"
            });
            expectedList.Add(new ClientFile
            {
                Name = "orca",
                Date = DateTime.ParseExact("20170916", "yyyyMMdd", CultureInfo.InvariantCulture),
                SortIndex = 1,
                Filename = "orca_20170916.xml"
            });
            expectedList.Add(new ClientFile
            {
                Name = "talon",
                Date = DateTime.ParseExact("20150831", "yyyyMMdd", CultureInfo.InvariantCulture),
                SortIndex = 4,
                Filename = "talon_20150831.xml"
            });
            expectedList.Add(new ClientFile
            {
                Name = "widget",
                Date = DateTime.ParseExact("20101101", "yyyyMMdd", CultureInfo.InvariantCulture),
                SortIndex = 2,
                Filename = "widget_20101101.xml"
            });

            ClientFile[] expectedArray = expectedList.ToArray();

            ClientFile[] actualArray = homeController.GetClientFiles(homeController.GetClientSettings()).ToArray();

            for (int i = 0; i < expectedArray.Length; i++)
            {
                ClientFile expected = expectedArray[i];
                ClientFile actual = actualArray[i];

                Assert.AreEqual(expected.Name, actual.Name);
                Assert.AreEqual(expected.Date, actual.Date);
                Assert.AreEqual(expected.SortIndex, actual.SortIndex);
                Assert.AreEqual(expected.Filename, actual.Filename);
            }
        }
    }
}