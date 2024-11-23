using System;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using Supremes;
using Supremes.Fluent;
using Supremes.Nodes;

namespace AutocrossWebScrape
{
    public class ReadingVar
    {

        public string name { get; set; }
        public Document[] selectedDocs { get; set; }
        public int docSize { get; set; }
        public int year { get; set; }
        public int[] trNthChild { get; set; }

        public void setYearDoc()
        {
            int currentYear = DateTime.Now.Year;
            Document[] selectedDocs = new Document[12];
            Document doc = null;

            if (year == currentYear) {

                doc = Dcsoup.Parse("https://www.texasscca.org/solo/results/");

                for (int i = 1; i <= 10; i++) {
                        if (i % 2 == 0)
                        {
                        Element link1 = doc.CreateElement("https://www.texasscca.org/Solo/2024/Results/2024-E" + i + "-Final.htm"); //even rows
                        Console.WriteLine(link1.ToString());
                            if (link1 != null)
                            {
                            Console.WriteLine(link1 + " was not null");
                                string url = link1.Attr("href");
                                selectedDocs[docSize] = Dcsoup.Parse(url);
                                this.docSize++;
                            }
                        }
                    if (i % 2 != 0) 
                    {
                        Element link = doc.CreateElement("https://www.texasscca.org/Solo/2024/Results/2024-E" + i + "-Final.htm"); // odd rows 
                        Console.WriteLine(link.ToString());
                        if (link != null)
                        {
                            Console.WriteLine(link + " was not null");
                            string url = link.Attr("href");
                            selectedDocs[docSize] = Dcsoup.Parse(url);
                            this.docSize++;
                        }
                    }
			    }
			this.selectedDocs = selectedDocs;
            this.docSize = docSize;
                Console.WriteLine("DocSize = "+  this.docSize);
		    }
		if (year >= (currentYear - 10) && year < currentYear) {
            doc = Dcsoup.Parse("https://www.texasscca.org/solo/results/past-results/");

            for (int i = 1; i <= 10; i++) { // for loop to find the links from the year chosen
                if (i % 2 == 0) {
                    Element link1 = doc.Select("#tablepress-300-" + (year - 2000) + "R > tbody > tr.row-" + (i) + ".even > td.column-4 > a").First(); //even rows
                    if (link1 != null) {
                        String url = link1.Attr("href");
                        selectedDocs[docSize] = Dcsoup.Parse(url);
                        this.docSize++;
                    }
                }
                if (i % 2 != 0) {
                    Element link = doc.Select("#tablepress-300-" + (year - 2000) + "R > tbody > tr.row-" + (i) + ".odd > td.column-4 > a").First(); // odd rows
                    if (link != null) {
                        String url = link.Attr("href");
                        selectedDocs[docSize] = Dcsoup.Parse(url);
                        this.docSize++;
                    }
                }
            }
        }
        if (year < (currentYear - 10) || year > currentYear) {
            Console.WriteLine("Year entered is invalid.");
        }

        this.selectedDocs = selectedDocs;
	}
	public Document[] getSelectedDocs()
{
    return selectedDocs;
}
public void setFindTrNthChild(int docSize) {

           
    Console.WriteLine("DocSize = " + docSize);
    int searchNum = 350;
            int[] trNthChild = new int[12];
    
    //21.7
    for (int j = 0; j < docSize; j++) { // loop to locate row that contains name
        for (int i = 0; i < searchNum; i++) {

            string temp = selectedDocs[j].CreateElement("/html/body/a/table[2]/tbody/tr[" + i + "]/td[4]").Text; // name locations

                    Console.WriteLine(temp);
            if (temp.Equals(name, StringComparison.InvariantCultureIgnoreCase)) {

                trNthChild[j] = i; // array for name addresses
                searchNum = i + 100;
                break;
            }
            else trNthChild[j] = -1; // assigns a -1 to the events not participated in
        }
        Console.WriteLine("Loading... (" + (j + 1) + "/" + docSize + ")");
    }
        this.trNthChild = trNthChild;
}
    }
}
