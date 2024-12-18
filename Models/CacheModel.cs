using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace AutocrossWebScrape.Models
{
    internal class CacheModel
    {
        [Name("Month Accessed")]
        public int Month { get; set; }
        [Name("Year")]
        public int Year { get; set; }
        [Name("Last Name")]
        public string LastName { get; set; }
        [Name("First Name")]
        public string FirstName { get; set; }
        [Name("Document Size")]
        public int DocSize { get; set; }
        [Name("Results Address")]
        public List<int> ResultsAddresses { get; set; }
    }
}
