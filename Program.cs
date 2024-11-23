using System;

namespace AutocrossWebScrape
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReadingVar Reading = new ReadingVar();
            int docSize = 0;

            Reading.name = "Molyneux, Matthew";
            Reading.year = 2024;

            Reading.setYearDoc();
            Reading.setFindTrNthChild(Reading.docSize);
            Run(Reading);
        }
        public static void Run(ReadingVar Reading) {


            int eventCount = 0;

            string notParticipatedString = "Did not participate in event(s)# ";

            for (int j = 0; j < Reading.docSize; j++) {

                int counter = 1;

                while (Reading.trNthChild[j] == -1) {
                    eventCount++;
                    notParticipatedString += ((j + 1).ToString() + ", ");

                    if (j == Reading.docSize - 1) break;
                    else j++;
                }

                if (eventCount == Reading.docSize) Console.WriteLine(Reading.name + " did not participate in any events for the year " + Reading.year);

                if ((j == (Reading.docSize - 1)) && Reading.trNthChild[j] == -1) break;


            string classLabel = Reading.getSelectedDocs()[j].CreateElement("body > a > table:nth-child(2) > tbody > tr:nth-child(" + Reading.trNthChild[j] + ") > td:nth-child(2)").Text;

            Console.WriteLine("Class: " + classLabel);

            for (int i = 7; i <= 9; i++) {
                Console.WriteLine("Run " + (counter) + ":" + Reading.getSelectedDocs()[j].CreateElement("table:nth-child(2) > tbody > tr:nth-child(" + Reading.trNthChild[j] + ") > td:nth-child(" + i + ")").Text);
                counter++;

                Console.WriteLine("Run " + (counter) + ": " + Reading.getSelectedDocs()[j].CreateElement("table:nth-child(2) > tbody > tr:nth-child(" + (Reading.trNthChild[j] + 1) + ") > td:nth-child(" + i + ")").Text);// time results, second row.
                counter++;
            }

            Console.WriteLine("Placement: " + Reading.getSelectedDocs()[j].CreateElement("table:nth-child(2) > tbody > tr:nth-child(" + Reading.trNthChild[j] + ") > td:nth-child(1)").Text);

            if (j == Reading.docSize - 1) notParticipatedString = notParticipatedString.Substring(0, notParticipatedString.Length - 2);

        }
            for (int i = 0; i < Reading.trNthChild.Length; i++) Console.WriteLine(Reading.trNthChild[i]);

            return;
        }
    }
}
