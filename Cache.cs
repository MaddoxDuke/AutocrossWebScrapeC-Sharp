using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace AutocrossWebScrape {
    public class Cache {
        public string currentMonth = DateTime.Now.Month.ToString();
        public int index = 0;
        public bool checkCache(ReadingVar Reading, string filePath) {

            string name = Regex.Replace(Reading.Name, @"\s+", "");
                foreach (string row in File.ReadLines(filePath)) {
                    if (row.StartsWith(currentMonth + "," + Reading.Year.ToString() + "," + name)) return true;
                    index++;
                }
                return false;
        }
        public int getIndex() {
            return index;
        }
        public int[] getTrNthChild(string row, int docSize) {
            int count = 0;
            int[] arr = new int[docSize];

            foreach (string field in row.Split(',')) {
               
                if (count > 4) {
                    arr[count-5] = Int32.Parse(field);
                }
                count++;
            }
            return arr;
        }
        public int getDocSize(string row) {
            int docSizeIndex = 4;
            int count = 0;
            foreach (string field in row.Split(',')) {
                if (count == docSizeIndex) return Int32.Parse(field);
                count++;
            }
            return -1;
        }
        public string getRow(int i, string filePath) {
            int count = 0;
            foreach (string row in File.ReadLines(filePath)) {
                if (i == count) return row;
                count++;
            }
            return null;
        }

        public void AddToCache(List<string> addedString, string filePath) {
            File.AppendAllLines(filePath, addedString);
            Console.WriteLine("Added to cache.");
        }

        public void ClearCache(string filePath) => File.WriteAllText(filePath, "Month Accessed,Year,Last Name,First Name,Document Size,Results Address\n");
        public string getValues(string filePath, string get) {

            foreach (string row in File.ReadLines(filePath)) {
                foreach (string field in row.Split(',')) {
                    if (!row.StartsWith(currentMonth)) break;
                   
                }
            }
            return null;
        }
    }
}


        //    while ((line = br.readLine()) != null) {
        //        string[] data = line.split(splitBy);
        //        int year1 = Int32.Parse(data[0]);
        //        string lName = data[1];
        //        string fName = data[2];

        //        if (Reading.Name.Equals(lName + ", " + fName, StringComparison.InvariantCultureIgnoreCase) && year == year1) {
        //            int docSize1 = Int32.Parse(data[3]);
        //            int[] arrayAddress = new int[docSize1];

        //            for (int i = 0; i < docSize1; i++) {
        //                arrayAddress[i] = Int32.Parse(data[4 + i]);
        //            }
        //            Reading.DocSize = docSize1;
        //            Reading.TrNthChild[docSize1] = arrayAddress[docSize1];
        //            //Output data here.
        //            return true;
        //        }
        //    }
        //    Console.WriteLine("Name not in cache.");

        //    return false;
        //}
        //public static bool newCache(ReadingVar Reading) {
        //    string cache = "Cache.csv";
        //    string currentMonth = "11";

        //    if (br.readLine().Equals(currentMonth)) return false;

        //    try (CsvReader csvReader = new CsvReader(cache, CultureInfo.InvariantCulture)) {

        //        tempWriter.append(Int32.Parse(currentMonth))
        //              .append("\n")
        //              .append(Int32.Parse(in.getYear()))
        //              .append(',')
        //              .append(Reading.getName().replaceAll("\\s+", ""))
        //              .append(',')
        //              .append(Integer.toString(Reading.getDocSize()))
        //              .append(',');
        //        for (int i = 0; i < Reading.DocSize; i++) {
        //            tempWriter.append(Integer.toString(Reading.TrNthChild[i]))
        //                      .append(',');
        //        }
        //        tempWriter.append("\n");
        //        tempWriter.close();
        //        Console.WriteLine("Added to Cache.");

        //        tempWriter.close();
        //        return true;
        //    }

        //catch (IOException e) {
        //    }
        //    return false;
        //}


//    }
//}
//}