using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SmartBoy
{
    class WikiSearch
    {   
        StringUtil tools = new StringUtil();
        string content, contentTrim;
        
        public string wikiBestMatchv2(string searchTerm, string[] keywords)
        {
            content = new GetWebClient().GetWebStringWiki(new WikiURLGen().wikiSearchURL(searchTerm)); // Pull Search Info

            if (content != "")
            {
                contentTrim = tools.getBetweenNA(content, "<Item>", "</Item>");

                if (keywordExists(keywords, contentTrim))
                    return tools.getBetweenNA(contentTrim, "<Text xml:space=\"preserve\">", "</Text>"); // Return the first Item as result.

                content = tools.getEnd(content, contentTrim); // Grab any remainder of the search result.
                contentTrim = tools.getBetweenNA(content, "<Item>", "</Item>"); // Trim it 

                while (contentTrim != "N/A" && contentTrim.Contains("<Text xml:space=\"preserve\">"))
                {
                    if (keywordExists(keywords, contentTrim))
                        return tools.getBetweenNA(contentTrim, "<Text xml:space=\"preserve\">", "</Text>"); 

                    content = tools.getEnd(content, contentTrim);
                    contentTrim = tools.getBetweenNA(content, "</Item>", "</Item>");
                }
            }

            return "N/A";
        }

        // Check for matching keywords.
        private bool keywordExists(string[] keywords, string content)
        {
            foreach (string item in keywords)
            {
                if (content.Contains(item))
                    return true;
            }
            return false;
        }
    }
}
