using System;
using System.Net.Http;
using HtmlAgilityPack;
using static System.Net.Mime.MediaTypeNames;

namespace AutocrossWebScrape {
    public class ReadingVar {

        public string Name { get; set; }
        public HtmlDocument[] SelectedDocs { get; set; }
        public int DocSize { get; set; }
        public int Year { get; set; }
        public int[] TrNthChild { get; set; }

        public void setYearDoc(int Year) {
            int currentYear = DateTime.Now.Year;
            string url = null;
            string[] urls = new string[12];

            if (Year < (currentYear - 10) || Year > currentYear) {
                Console.WriteLine("Year entered is invalid.");
                return;
            }
            if (Year == currentYear) {
                url = "https://www.texasscca.org/solo/results/";
            } else url = "https://www.texasscca.org/Solo/results/past-results/";

            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            for (int i = 1; i < 12; i++) {
                if (htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]/td[4]/a") != null) {
                    urls[i - 1] = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]/td[4]/a").Attributes["href"].Value; //links for final results
                    DocSize++;
                }
            }
            Console.WriteLine("DocSize is " + DocSize);
            SelectedDocs = new HtmlDocument[DocSize];

            for (int i = 1; i <= DocSize; i++) {

                httpClient = new HttpClient();
                html = httpClient.GetStringAsync(urls[i - 1]).Result;
                htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                SelectedDocs[i - 1] = htmlDocument;
            }
        }
        public HtmlDocument[] getSelectedDocs() {
            return SelectedDocs;
        }
        public void setTrNthChild(string Name) {

            int searchNum = 350;
            int[] trNthChild = new int[DocSize];

            for (int j = 0; j < DocSize; j++) { // loop to locate row that contains name
                Console.WriteLine("Searching doc #" + j);
                for (int i = 1; i < searchNum; i++) {

                    if (SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + i + "]") == null) break; // failsafe, exits for loop if null, this means it has reached the end of the webpage.

                    if (SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + i + "]").InnerText.Contains(Name)) { // checks if name exists on each line.
                        Console.WriteLine("Name found on Doc #" + (j + 1));

                        trNthChild[j] = i; // array for name addresses, correlates with each doc a name is found in. allows for easy lookup when displaying
                        searchNum = i + 100;
                        break;
                    }
                }
                if (trNthChild[j] == 0) Console.WriteLine("Name not found on Doc #" + (j + 1));
                Console.WriteLine("Loading... (" + (j + 1) + "/" + DocSize + ")");
            }
            TrNthChild = trNthChild;
        }
    }
}
