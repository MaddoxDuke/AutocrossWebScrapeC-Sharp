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

        public void setYearDoc() {
            int currentYear = DateTime.Now.Year;
            string url = null;
            string tempUrl = null;
            bool curYear = false;

            if (Year < (currentYear - 10) || Year > currentYear) {
                Console.WriteLine("Year entered is invalid.");
                return;
            }
            if (Year == currentYear) {
                url = "https://www.texasscca.org/solo/results/";
                curYear = true;
            } else url = "https://www.texasscca.org/Solo/results/past-results/";

            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            for (int i = 1; i < 12; i++) {
                if (htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]") != null) DocSize++;
            }
            Console.WriteLine("DocSize is " + DocSize);
            SelectedDocs = new HtmlDocument[DocSize];

            for (int i = 1; i <= DocSize; i++) {
                if (curYear == true) tempUrl = "https://www.texasscca.org/Solo/" + Year + "/Results/" + Year + "-E" + i + "-Final.htm";
                else tempUrl = "https://www.texasscca.org/Solo/" + Year + "/past-results/" + Year + "-E" + i + "-Final.htm";
                if (httpClient.GetStringAsync(tempUrl) != null) {
                    html = httpClient.GetStringAsync(url).Result;
                    htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);
                    SelectedDocs[i - 1] = htmlDocument;
                }
            }
        }
        public HtmlDocument[] getSelectedDocs() {
            return SelectedDocs;
        }
        public void setFindTrNthChild(int docSize) {

            int searchNum = 350;
            int[] trNthChild = new int[12];
            string temp = "";
            // "/html/body/a/table[2]/tbody/tr[" + i + "]/td[4]"

            //21.7
            for (int j = 0; j < docSize; j++) { // loop to locate row that contains name
                    Console.WriteLine("Searching doc #" + j);
                for (int i = 0; i < searchNum; i++) {
                    // //*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]
                    // //*[@name=\"#top"]/tbody/tr[4]
                   


                    if (SelectedDocs[j].DocumentNode.SelectSingleNode("//*[@Name=\"#top\"]/tr["+ 2 +"]/td[4]").InnerText.Equals(Name, StringComparison.InvariantCultureIgnoreCase)) {
                        Console.WriteLine("Name found on Doc #" + j);


                        trNthChild[j] = i; // array for name addresses, correlates with each doc a name is found in. allows for easy lookup when displaying
                        searchNum = i + 100;
                        break;
                    } else trNthChild[j] = -1; // assigns a -1 to the events not participated in
                }
                Console.WriteLine("Loading... (" + (j + 1) + "/" + docSize + ")");
            }
            TrNthChild = trNthChild;
        }
    }
}
