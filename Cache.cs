using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AutocrossWebScrape {
    public class Cache {
        public string currentMonth = DateTime.Now.Month.ToString();
        public int index = 0;
        public bool checkCache(ReadingModel Reading, string filePath) {

            string name = Regex.Replace(Reading.Name, @"\s+", "");
                foreach (string row in File.ReadLines(filePath)) {
                    if (row.StartsWith(currentMonth + "," + Reading.paxRaw.ToString().ToLower() + "," + Reading.Year.ToString() + "," + name)) return true;
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
               
                if (count > 5) {
                    arr[count-6] = Int32.Parse(field);
                }
                count++;
            }
            return arr;
        }
        public int getDocSize(string row) {
            int docSizeIndex = 5;
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
        public void ClearCache(string filePath) => File.WriteAllText(filePath, "Month Accessed,PaxAndRaw,Year,Last Name,First Name,Document Size,Results Address\n");
    }
}