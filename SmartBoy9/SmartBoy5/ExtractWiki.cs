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
        
    }
}
