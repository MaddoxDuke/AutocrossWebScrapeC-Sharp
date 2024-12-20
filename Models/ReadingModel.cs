using System;
using System.Net.Http;
using HtmlAgilityPack;

namespace AutocrossWebScrape {
    public class ReadingModel {

        public string Name { get; set; }
        public HtmlDocument[] SelectedDocs { get; set; }
        public int DocSize { get; set; }
        public int Year { get; set; }
        public int[] TrNthChild { get; set; }
        public bool paxRaw { get; set; }
    }
}
