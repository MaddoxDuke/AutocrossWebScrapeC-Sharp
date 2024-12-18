using System;

namespace AutocrossWebScrape {
    internal class Program {
        static void Main(string[] args) {

            string cachePath = "C:\\Users\\Administrator\\Desktop\\Code\\AutocrossWebScrape\\Cache\\Cache.csv";
            ReadingVar Reading = new ReadingVar();
            Reading.Name = "Neff, Bob";
            Reading.Year = 2021;

            Cache c = new Cache();

            //if (!c.checkCache(Reading.Name, cachePath)) {
                // Console.WriteLine("Name found");

                
            //}
            //else {
            Reading.setYearDoc();
            Reading.setFindTrNthChild(Reading.DocSize);
            Run(Reading);
            //}


            Console.ReadKey();
        }
        public static void Run(ReadingVar Reading) {

            

            int eventCount = 0;

            string notParticipatedString = "Did not participate in event(s)# ";

            for (int j = 0; j < Reading.DocSize; j++) {

                int counter = 1;

                while (Reading.TrNthChild[j] == -1) {
                    eventCount++;
                    notParticipatedString += ((j + 1).ToString() + ", ");

                    if (j == Reading.DocSize - 1) break;
                    else j++;
                }

                if (eventCount == Reading.DocSize) Console.WriteLine(Reading.Name + " did not participate in any events for the year " + Reading.Year);

                if ((j == (Reading.DocSize - 1)) && Reading.TrNthChild[j] == -1) break;


                string classLabel = Reading.getSelectedDocs()[j].CreateTextNode("body > a > table:nth-child(2) > tbody > tr:nth-child(" + Reading.TrNthChild[j] + ") > td:nth-child(2)").Text;

                Console.WriteLine("Class: " + classLabel);

                for (int i = 7; i <= 9; i++) {
                    Console.WriteLine("Run " + (counter) + ":" + Reading.getSelectedDocs()[j].CreateTextNode("table:nth-child(2) > tbody > tr:nth-child(" + Reading.TrNthChild[j] + ") > td:nth-child(" + i + ")").Text);
                    counter++;

                    Console.WriteLine("Run " + (counter) + ": " + Reading.getSelectedDocs()[j].CreateTextNode("table:nth-child(2) > tbody > tr:nth-child(" + (Reading.TrNthChild[j] + 1) + ") > td:nth-child(" + i + ")").Text);// time results, second row.
                    counter++;
                }

                Console.WriteLine("Placement: " + Reading.getSelectedDocs()[j].CreateTextNode("table:nth-child(2) > tbody > tr:nth-child(" + Reading.TrNthChild[j] + ") > td:nth-child(1)"));

                if (j == Reading.DocSize - 1) notParticipatedString = notParticipatedString.Substring(0, notParticipatedString.Length - 2);

            }
            for (int i = 0; i < Reading.TrNthChild.Length; i++) Console.WriteLine(Reading.TrNthChild[i]);
            Console.ReadKey();

            return;
        }
    }
}
