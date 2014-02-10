using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace SmartBoy
{
    class ExtractWiki
    {
        WikiURLGen w1 = new WikiURLGen();
        GetWebClient client = new GetWebClient();
        WikiXML2Data fetchData = new WikiXML2Data();
        WikiSearch search = new WikiSearch();

        string bestSearchTerm;

        public string wikiContent(string input, string [] keywords)
        {
            bestSearchTerm = search.SearchWiki(input, keywords);
            if(bestSearchTerm != "N/A")
                return fetchData.getInfo(client.GetWebStringWiki(w1.wikiURL(bestSearchTerm)));
            else
                return "N/A";

            //return fetchData.getInfo(client.GetWebStringWiki(w1.wikiURL(search.SearchWiki(input, keywords))));
        }

        // New Code

        public string wikiContentv2(string input, string[] keywords)
        {
            Console.WriteLine("ExtractWiki | wikiContentv2");

            bestSearchTerm = new WikiSearch().wikiBestMatchv2(input, keywords); // Fetch search term.
            Console.WriteLine("ExtractWiki | wikiContentv2 | bestSearchTerm: " + bestSearchTerm);

            if (bestSearchTerm != "N/A")
            {
                // extracts info from Wikipedia for bestSearchTerm.
                return new WikiXML2Data().getInfov2(new GetWebClient().GetWebStringWiki(new WikiURLGen().wikiURL(bestSearchTerm)));
            }
            else
            {
                return "N/A";
            }
        }

        //
        
    }
}
