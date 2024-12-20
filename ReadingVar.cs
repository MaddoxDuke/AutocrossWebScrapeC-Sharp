using System;
using System.Net.Http;
using HtmlAgilityPack;

namespace AutocrossWebScrape {
    public class ReadingVar {

        public string Name { get; set; }
        public HtmlDocument[] SelectedDocs { get; set; }
        public int DocSize { get; set; }
        public int Year { get; set; }
        public int[] TrNthChild { get; set; }
        public bool paxRaw { get; set; }

        public void setYearDoc(int Year) {

            paxRaw = false;
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
        public void setYearDoc(int Year, bool paxAndRaw) {

            paxRaw = paxAndRaw;
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
                if (htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]/td[5]/a") != null) { // td[5] instead of td[4] for the pax and raw link
                    urls[i - 1] = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"tablepress-300-" + (Year - 2000) + "R\"]/tbody/tr[" + i + "]/td[5]/a").Attributes["href"].Value; //links for final results
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
        public void setTrNthChild(string Name, bool paxAndRaw) {

            int searchNum = 175;
            int[] trNthChild = new int[DocSize];

            for (int j = 0; j < DocSize; j++) { // loop to locate row that contains name
                Console.WriteLine("Searching doc #" + j);
                for (int i = 2; i < searchNum; i++) {

                    //Name xpath:  / html / body / table[2] / tbody / tr[2] / td[5] 
                    // /html/body/table[2]/tbody/tr[3]/td[5]
                   
                    if (SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + i + "]/td[5]") == null) break; // failsafe, exits for loop if null, this means it has reached the end of the webpage.

                    if (SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + i + "]/td[5]").InnerText.Contains(Name)) { // checks if name exists on each line.
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
        public void OutputToConsole() {

            int eventCount = 0;

            string notParticipatedString = "Did not participate in event(s)# ";

            Console.WriteLine("\nResults for " + Name + ": ");

            for (int j = 0; j < DocSize; j++) {

                int counter = 1;

                while (TrNthChild[j] == 0) {
                    eventCount++;
                    notParticipatedString += ((j + 1).ToString() + ", ");

                    if (j == DocSize - 1) break;
                    else j++;
                }

                if (eventCount == DocSize) Console.WriteLine(Name + " did not participate in any events for the year " + Year);

                if ((j == (DocSize - 1)) && TrNthChild[j] == 0) break;

                if (!paxRaw) {
                    string classLabel = SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + TrNthChild[j] + "]/td[2]").InnerText;

                    Console.WriteLine("\nClass: " + classLabel);

                    for (int i = 7; i <= 9; i++) {
                        Console.WriteLine("Run " + (counter) + ":" + SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + TrNthChild[j] + "]/td[" + i + "]").InnerText);
                        counter++;

                        Console.WriteLine("Run " + (counter) + ": " + SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + (TrNthChild[j] + 1) + "]/td[" + i + "]").InnerText);// time results, second row.
                        counter++;
                    }

                    Console.WriteLine("Placement: " + SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + TrNthChild[j] + "]/td[1]").InnerText + "\n");
                } else {

                    string classLabel = SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + TrNthChild[j] + "]/td[3]").InnerText;
                    Console.WriteLine("\nClass: " + classLabel);

                    Console.WriteLine("Pax Time" + ": " + SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + TrNthChild[j] + "]/td[9]").InnerText);
                    Console.WriteLine("Raw Time" + ": " + SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + TrNthChild[j] + "]/td[7]").InnerText);

                    Console.WriteLine("Pax Position: " + SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + TrNthChild[j] + "]/td[1]").InnerText);
                    Console.WriteLine("Class Position: " + SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + TrNthChild[j] + "]/td[2]").InnerText + "\n");

                }
                if (j == DocSize - 1) notParticipatedString += j.ToString();

            }
            return;
        }
    }
}
