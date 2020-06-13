using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace opg_201910_interview.Models
{
    /// <summary>
    /// Contains the settings for each client as defined in appsettings.json.
    /// </summary>
    public class ClientSettings
    {
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string FileDirectoryPath { get; set; }
        public string FilePattern { get; set; }
        public string Delimiter { get; set; }
        public Dictionary<string, string> SortRules { get; set; }
    }
}
