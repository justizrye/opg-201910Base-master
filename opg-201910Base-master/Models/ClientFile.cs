using System;

namespace opg_201910_interview.Models
{
    /// <summary>
    /// Client file definition.
    /// </summary>
    public class ClientFile
    {
        public string Name { get; set; }
        public int SortIndex { get; set; }
        public DateTime Date { get; set; }
        public string Filename { get; set; }
    }
}
