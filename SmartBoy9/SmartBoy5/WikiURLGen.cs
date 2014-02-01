using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class WikiURLGen
    {
        string rootURL = "http://en.wikipedia.org/w/api.php?action=query&prop=extracts&format=xml&exintro=1&titles=";

        string searchURL = "http://en.wikipedia.org/w/api.php?action=opensearch&format=xml&search=";


        public string wikiURL(string searchTerm)
        {
            return rootURL + searchTerm;
        }


        public string wikiSearchURL(string searchThis)
        {
            if (searchThis != "N/A")
                return searchURL + searchThis;
            else
                return "N/A";
        }


    }
}
