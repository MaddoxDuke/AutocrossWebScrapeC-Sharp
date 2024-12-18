using System;
using System.Collections.Generic;
using Supremes.Nodes;
using System.Net;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace AutocrossWebScrape {
    internal class Program {
        static void Main(string[] args) {

            int currentMonth = DateTime.Now.Month;
            string cachePath = "C:\\Users\\Administrator\\Desktop\\Code\\AutocrossWebScrape\\Cache\\Cache.csv";
            ReadingVar Reading = new ReadingVar();
            Reading.Name = "Molyneux, Matthew";
            Reading.Year = 2023;

            string name = Reading.Name;
            int year = Reading.Year;

            Cache c = new Cache();

            //if (c.checkCache(Reading, cachePath)) {
            //    Console.WriteLine("Name found in Cache");

            //    Reading.setYearDoc(year);
            //    Reading.TrNthChild = c.getTrNthChild(c.getRow(c.getIndex(), cachePath), Reading.DocSize);
       
            //    Run(Reading);
            //}
            //else {
                Console.WriteLine("Name not found in Cache");
                Reading.setYearDoc(year);
                Reading.setTrNthChild(name);
                Run(Reading);


                string tempStr = currentMonth.ToString() + "," + year.ToString() + "," + Regex.Replace(name, @"\s+", "") + "," + Reading.DocSize.ToString() + ",";

                for (int i = 0; i < Reading.TrNthChild.Length; i++) {
                    tempStr += Reading.TrNthChild[i].ToString();
                    if (i != Reading.TrNthChild.Length-1) tempStr += ",";
                }

                List<string> cacheString = new List<string> {
                    tempStr
                };

                //c.AddToCache(cacheString, cachePath);
            //}


            Console.ReadKey();
        }
        public static void Run(ReadingVar Reading) {

            

            int eventCount = 0;

            string notParticipatedString = "Did not participate in event(s)# ";

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

                string classLabel = Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[2]").InnerText;

                Console.WriteLine("Class: " + classLabel);

                for (int i = 7; i <= 9; i++) {
                    Console.WriteLine("Run " + (counter) + ":" + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[" + i + "]").InnerText);
                    counter++;

                    Console.WriteLine("Run " + (counter) + ": " + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + (Reading.TrNthChild[j] + 1) + "]/td[" + i + "]").InnerText);// time results, second row.
                    counter++;
                }

                Console.WriteLine("Placement: " + Reading.SelectedDocs[j].DocumentNode.SelectSingleNode("/html/body/a/table[2]/tbody/tr[" + Reading.TrNthChild[j] + "]/td[1]").InnerText);

                if (j == Reading.DocSize - 1) notParticipatedString += j.ToString() ;

            }
            return;
        }
    }
}
