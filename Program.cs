using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutocrossWebScrape.Controllers;

namespace AutocrossWebScrape {
    internal class Program {
        static void Main(string[] args) {

            ReadingController controller = new ReadingController();
            int currentMonth = DateTime.Now.Month;
            string cachePath = "C:\\Users\\Administrator\\Desktop\\Code\\AutocrossWebScrape\\Cache\\Cache.csv";
            ReadingModel Reading = new ReadingModel();

            Reading.Name = "Duke, Maddox";
            Reading.Year = 2022;
            Reading.paxRaw = true; //will search final by default (false), will search Pax and Raw times if true.
            DateTime start, end;

            string name = Reading.Name;
            int year = Reading.Year;

            //Cache c = new Cache();

            //if (c.checkCache(Reading, cachePath)) {
            //    Console.WriteLine("Name found in Cache");
            //    start = DateTime.Now;

            //    if (Reading.paxRaw) {
            //        controller.setYearDoc(year, Reading.paxRaw);
            //    } else {
            //        controller.setYearDoc(year);
            //    }
            //    Reading.TrNthChild = c.getTrNthChild(c.getRow(c.getIndex(), cachePath), Reading.DocSize);

            //    controller.OutputToConsole();
            //    end = DateTime.Now;

            //    Console.WriteLine(end - start);
            //} else {
            //    Console.WriteLine("Name not found in Cache");
                start = DateTime.Now;

                if (Reading.paxRaw) {
                    controller.setYearDoc(year, Reading.paxRaw); // finds final and pax Year documents
                    controller.setTrNthChild(name, Reading.paxRaw);
                } else {
                    controller.setYearDoc(year); // finds the default documents (Final times)
                    controller.setTrNthChild(name);
                }
                controller.OutputToConsole();

                end = DateTime.Now;

                Console.WriteLine(end - start);

                //string tempStr = currentMonth.ToString() + "," + Reading.paxRaw.ToString().ToLower() + "," + year.ToString() + "," + Regex.Replace(name, @"\s+", "") + "," + Reading.DocSize.ToString() + ",";

                //int cLength = Reading.TrNthChild.Length;
                //Console.WriteLine(cLength);

                //for (int i = 0; i < cLength; i++) {
                //    tempStr += Reading.TrNthChild[i].ToString();
                //    if (i != cLength - 1) tempStr += ",";
                //}

                //List<string> cacheString = new List<string> {
                //    tempStr
                //};

                //c.AddToCache(cacheString, cachePath);
            //  }


            Console.ReadKey();
        }
    }
}
