using System;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;

namespace AutocrossWebScrape.Controllers {
    internal class ReadingController {

        ReadingModel Reading = new ReadingModel();

        public void setYearDoc(int Year) {


            Reading.paxRaw = false;
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
                    Reading.DocSize++;
                }
            }
            Console.WriteLine("DocSize is " + Reading.DocSize);
            Reading.SelectedDocs = new HtmlDocument[Reading.DocSize];

            for (int i = 1; i <= Reading.DocSize; i++) {

                httpClient = new HttpClient();
                html = httpClient.GetStringAsync(urls[i - 1]).Result;
                htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                Reading.SelectedDocs[i - 1] = htmlDocument;
            }
        }
        public void setTrNthChild(string Name) {

            int searchNum = 350;
            Reading.TrNthChild = new int[Reading.DocSize];

            for (int j = 0; j < Reading.DocSize; j++) { // loop to locate row that contains name
                Console.WriteLine("Searching doc #" + j);
                for (int i = 1; i < searchNum; i++) {

                    if (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + i + "]") == null) break; // failsafe, exits for loop if null, this means it has reached the end of the webpage.

                    if (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + i + "]").InnerText.Contains(Name)) { // checks if name exists on each line.
                        Console.WriteLine("Name found on Doc #" + (j + 1));

                        Reading.TrNthChild[j] = i; // array for name addresses, correlates with each doc a name is found in. allows for easy lookup when displaying
                        searchNum = i + 100;
                        break;
                    }
                }
                if (Reading.TrNthChild[j] == 0) Console.WriteLine("Name not found on Doc #" + (j + 1));
                Console.WriteLine("Loading... (" + (j + 1) + "/" + Reading.DocSize + ")");
            }
        }
        public void setYearDoc(int Year, bool paxAndRaw) {

            Reading.paxRaw = paxAndRaw;
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
                    Reading.DocSize++;
                }
            }
            Console.WriteLine("DocSize is " + Reading.DocSize);
            Reading.SelectedDocs = new HtmlDocument[Reading.DocSize];

            for (int i = 1; i <= Reading.DocSize; i++) {

                httpClient = new HttpClient();
                html = httpClient.GetStringAsync(urls[i - 1]).Result;
                htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                Reading.SelectedDocs[i - 1] = htmlDocument;
            }
        }
        public void setTrNthChild(string Name, bool paxAndRaw) {

            int searchNum = 175;
            Reading.TrNthChild = new int[Reading.DocSize];

            for (int j = 0; j < Reading.DocSize; j++) { // loop to locate row that contains name
                Console.WriteLine("Searching doc #" + j);
                for (int i = 2; i < searchNum; i++) {

                    if (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + i + "]/td[5]") == null) break; // failsafe, exits for loop if null, this means it has reached the end of the webpage.

                    if (Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + i + "]/td[5]").InnerText.Contains(Name)) { // checks if name exists on each line.
                        Console.WriteLine("Name found on Doc #" + (j + 1));

                        Reading.TrNthChild[j] = i; // array for name addresses, correlates with each doc a name is found in. allows for easy lookup when displaying
                        searchNum = i + 100;
                        break;
                    }
                }
                if (Reading.TrNthChild[j] == 0) Console.WriteLine("Name not found on Doc #" + (j + 1));
                Console.WriteLine("Loading... (" + (j + 1) + "/" + Reading.DocSize + ")");
            }
        }
        public void OutputToConsole() {

            int eventCount = 0;

            string notParticipatedString = "Did not participate in event(s)# ";

            Console.WriteLine("\nResults for " + Reading.Name + ": ");

            for (int j = 0; j < Reading.DocSize; j++) {

                int counter = 1;

                while (Reading.TrNthChild[j] == 0) {
                    eventCount++;
                    notParticipatedString += ((j + 1).ToString() + ", ");

                    if (j == Reading.DocSize - 1) break;
                    else j++;
                }

                if (eventCount == Reading.DocSize) Console.WriteLine(Reading.Name + " did not participate in any events for the year " + Reading.Year);

                if ((j == (Reading.DocSize - 1)) && Reading.TrNthChild[j] == 0) break;

                if (!Reading.paxRaw) {
                    string classLabel = Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[2]").InnerText;

                    Console.WriteLine("\nClass: " + classLabel);

                    for (int i = 7; i <= 9; i++) {
                        Console.WriteLine("Run " + (counter) + ":" + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[" + i + "]").InnerText);
                        counter++;

                        Console.WriteLine("Run " + (counter) + ": " + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + (Reading.TrNthChild[j] + 1) + "]/td[" + i + "]").InnerText);// time results, second row.
                        counter++;
                    }

                    Console.WriteLine("Placement: " + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[1]").InnerText + "\n");
                } else {

                    string classLabel = Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[3]").InnerText;
                    Console.WriteLine("\nClass: " + classLabel);

                    Console.WriteLine("Pax Time" + ": " + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[9]").InnerText);
                    Console.WriteLine("Raw Time" + ": " + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[7]").InnerText);

                    Console.WriteLine("Pax Position: " + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[1]").InnerText);
                    Console.WriteLine("Class Position: " + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[2]").InnerText + "\n");

                }
                if (j == Reading.DocSize - 1) notParticipatedString += j.ToString();

            }
            return;
        }
    }
}
