using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using opg_201910_interview.Models;

namespace opg_201910_interview.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IConfiguration _iconfiguration;

        public HomeController(ILogger<HomeController> logger, IConfiguration iconfiguration)
        {
            _logger = logger;
            _iconfiguration = iconfiguration;
        }

        public IActionResult Index()
        {
            ClientSettings clientSettings = GetClientSettings();
            ViewData["ClientSettings"] = clientSettings;
            ViewData["ClientFiles"] = GetClientFiles(clientSettings);
            return View();
        }

        /// <summary>
        /// Retrieves the client settings from the appsetttings.json file.
        /// </summary>
        /// <returns>The client settings corresponding to each section in the appsetttings.</returns>
        public ClientSettings GetClientSettings()
        {
            ClientSettings clientSettings = new ClientSettings();

            IConfigurationSection section = _iconfiguration.GetSection("ClientSettings");
            clientSettings.ClientId = section["ClientId"];
            clientSettings.FileDirectoryPath = section["FileDirectoryPath"];
            clientSettings.Name = clientSettings.FileDirectoryPath.Substring(clientSettings.FileDirectoryPath.IndexOf("/") + 1);
            clientSettings.FilePattern = section["FilePattern"];
            clientSettings.Delimiter = section["Delimiter"];
            clientSettings.SortRules = section.GetSection("SortRules").GetChildren().ToDictionary(x => x.Key, x => x.Value);

            return clientSettings;
        }

        /// <summary>
        /// Retrieves the client files for the specified client based on the search pattern.
        /// </summary>
        /// <param name="clientSettings">The client settings.</param>
        /// <returns>The client files.</returns>
        public IEnumerable<ClientFile> GetClientFiles(ClientSettings clientSettings)
        {
            IList<ClientFile> clientFiles = new List<ClientFile>();

            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), clientSettings.FileDirectoryPath));
            FileInfo[] fileInfos = dirInfo.GetFiles(clientSettings.FilePattern);
            foreach (FileInfo fileInfo in fileInfos)
            {
                string trimmedName = fileInfo.Name.Replace(".xml", "");

                ClientFile clientFile = new ClientFile();
                clientFile.Name = trimmedName.Substring(0, trimmedName.IndexOf(clientSettings.Delimiter));
                if (clientSettings.ClientId == "1001")
                {
                    clientFile.Date = DateTime.ParseExact(trimmedName.Substring(trimmedName.IndexOf(clientSettings.Delimiter) + 1), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                else if (clientSettings.ClientId == "1002")
                {
                    clientFile.Date = DateTime.ParseExact(trimmedName.Substring(trimmedName.IndexOf(clientSettings.Delimiter) + 1), "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                clientFile.Filename = fileInfo.Name;

                clientFile.SortIndex = GetSortIndex(clientSettings.SortRules, clientFile.Name);

                clientFiles.Add(clientFile);
            }

            return clientFiles;
        }

        /// <summary>
        /// Retrieves the sort index of certain file based on the sorting rules for the specific client.
        /// </summary>
        /// <param name="sortRules">The sorting rules from the appsettings.json file.</param>
        /// <param name="name">The name to look for in the sorting rules.</param>
        /// <returns>The sort index of the specified file.</returns>
        public int GetSortIndex(Dictionary<string, string> sortRules, string name)
        {
            int sortIndex = 99;
            string value = "";

            if (sortRules.TryGetValue(name, out value))
            {
                sortIndex = int.Parse(value);
            }

            return sortIndex;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
