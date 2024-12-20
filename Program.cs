using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AutocrossWebScrape {
    internal class Program {
        static void Main(string[] args) {

            int currentMonth = DateTime.Now.Month;
            string cachePath = "C:\\Users\\Administrator\\Desktop\\Code\\AutocrossWebScrape\\Cache\\Cache.csv";
            ReadingVar Reading = new ReadingVar();
            Reading.Name = "Jarrett, Bruce";
            Reading.Year = 2023;
            Reading.paxRaw = true; //will search final by default (false), will search Pax and Raw times if true.
            DateTime start, end;

            string name = Reading.Name;
            int year = Reading.Year;

            Cache c = new Cache();

            if (c.checkCache(Reading, cachePath)) {
                Console.WriteLine("Name found in Cache");
                start = DateTime.Now;

                if (Reading.paxRaw) {
                    Reading.setYearDoc(year, Reading.paxRaw);
                } else {
                    Reading.setYearDoc(year);
                }
                Reading.TrNthChild = c.getTrNthChild(c.getRow(c.getIndex(), cachePath), Reading.DocSize);

                Reading.OutputToConsole();
                end = DateTime.Now;

                Console.WriteLine(end - start);
            } else {
                Console.WriteLine("Name not found in Cache");
                start = DateTime.Now;

                if (Reading.paxRaw) {
                    Reading.setYearDoc(year, Reading.paxRaw); // finds final and pax Year documents
                    Reading.setTrNthChild(name, Reading.paxRaw);
                } else {
                    Reading.setYearDoc(year); // finds the default documents (Final times)
                    Reading.setTrNthChild(name);
                }
                Reading.OutputToConsole();

                end = DateTime.Now;

                Console.WriteLine(end - start);

                string tempStr = currentMonth.ToString() + "," + Reading.paxRaw.ToString().ToLower() + "," + year.ToString() + "," + Regex.Replace(name, @"\s+", "") + "," + Reading.DocSize.ToString() + ",";

                for (int i = 0; i < Reading.TrNthChild.Length; i++) {
                    tempStr += Reading.TrNthChild[i].ToString();
                    if (i != Reading.TrNthChild.Length - 1) tempStr += ",";
                }

                List<string> cacheString = new List<string> {
                    tempStr
                };

                c.AddToCache(cacheString, cachePath);
            }


            Console.ReadKey();
        }
    }
}
