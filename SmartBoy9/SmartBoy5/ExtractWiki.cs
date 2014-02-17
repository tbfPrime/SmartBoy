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
        string bestSearchTerm;

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
    }
}
