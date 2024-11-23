using System.Xml.Linq;
using System;
using AutocrossWebScrape;
using static AutocrossWebScrape.ReadingVar;

namespace AutocrossWebScrape
{
    class Cache
    {

        public static bool checkCache(ReadingVar Reading, int year) {

            String line = "";
            String splitBy = ",";
            String fileName = "Cache.csv";
            FileReader fr = new FileReader(fileName);
            BufferedReader br = new BufferedReader(fr);
            string currentMonth = "11";

        Console.WriteLine("Checking Cache...");

        string tempMonth = br.readLine();
        Console.WriteLine(tempMonth);

                if (!tempMonth.Equals(currentMonth))
                {
                    return false;
                }

                while ((line = br.readLine()) != null)
                {
                    string[] data = line.split(splitBy);
                    int year1 = Int32.Parse(data[0]);
                    string lName = data[1];
                    string fName = data[2];

                    if (Reading.name.Equals(lName + ", " + fName, StringComparison.InvariantCultureIgnoreCase) && year == year1)
                    {
                        int docSize1 = Int32.Parse(data[3]);
                        int[] arrayAddress = new int[docSize1];

                        for (int i = 0; i<docSize1; i++)
                        {
                            arrayAddress[i] = Int32.Parse(data[4 + i]);
                        }
                    Reading.docSize = docSize1;
                    Reading.trNthChild[docSize1] = arrayAddress[docSize1]; 
                        //Output data here.
                        return true;
                    }
                }
                Console.WriteLine("Name not in cache.");
                
            return false;
            }
    }
}