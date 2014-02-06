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
        WikiURLGen genURL = new WikiURLGen();
        GetWebClient client = new GetWebClient();

        // New Variables

        StringUtil tools = new StringUtil();
        string content, contentTrim;

        //

        string[] keywords;

        #region Search Interface

        public string SearchWiki(string ip, string [] k)
        {
            keywords = k;
            return wikiBestMatch(ip);
        }

        #endregion

        private string wikiBestMatch(string input)
        {
            string content = client.GetWebStringWiki(genURL.wikiSearchURL(input));
            string contentTrim = tools.getBetweenNA(content, "<Item>", "</Item>");
            if (content != "")
            {

                if (checkKeyword(keywords, contentTrim))
                    return LookupsearchResult(contentTrim); // Return the first Item as result.
                content = getEnd(content, contentTrim);
                contentTrim = getBetween(content, "<Item>", "</Item>");
                if (contentTrim != "N/A")
                {
                    if (moreResults(contentTrim))
                    {
                        while (moreResults(contentTrim))
                        {
                            if (checkKeyword(keywords, contentTrim))
                                return LookupsearchResult(contentTrim);

                            content = getEnd(content, contentTrim);
                            contentTrim = getBetween(content, "</Item>", "</Item>");
                        }

                    }
                }
            }
            return "N/A";
        }

        private string LookupsearchResult(string content)
        {
            return getBetween(content, "<Text xml:space=\"preserve\">", "</Text>");
        }

        private bool moreResults(string checkThis)
        {
            return checkThis.Contains("<Text xml:space=\"preserve\">");
        }

        private int countResults(string content)
        {
            return Regex.Matches(content, "<Text xml:space=\"preserve\">").Count;
        }

        private bool checkKeyword(string [] keywords, string content)
        {
            foreach (string item in keywords)
            {
                if (content.Contains(item))
                    return true;
            }
            return false;
        }

        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            try
            {
                if (strSource.Contains(strStart) && strSource.Contains(strEnd))
                {
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = strSource.IndexOf(strEnd, Start);
                    return strSource.Substring(Start, End - Start);
                }
                else
                {
                    return "N/A";
                }
            }
            catch
            {
                return "N/A";
            }
        }

        private static string getEnd(string strSource, string strStart)
        {
            int Start, End;
            if (strSource.Contains(strStart))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.Length;
                return strSource.Substring(Start, strSource.Length - Start);
            }
            else
            {
                return "";
            }
        }

        // New Code

        public string wikiBestMatchv2(string searchTerm, string[] keywords)
        {
            content = new GetWebClient().GetWebStringWiki(new WikiURLGen().wikiSearchURL(searchTerm)); // Pull Search Info

            if (content != "")
            {
                contentTrim = tools.getBetweenNA(content, "<Item>", "</Item>");

                if (keywordExists(keywords, contentTrim))
                    return getBetween(contentTrim, "<Text xml:space=\"preserve\">", "</Text>"); // Return the first Item as result.

                content = getEnd(content, contentTrim); // Grab any remainder of the search result.
                contentTrim = getBetween(content, "<Item>", "</Item>"); // Trim it 

                while (contentTrim != "N/A" && contentTrim.Contains("<Text xml:space=\"preserve\">"))
                {
                    if (keywordExists(keywords, contentTrim))
                        return getBetween(contentTrim, "<Text xml:space=\"preserve\">", "</Text>"); 

                    content = getEnd(content, contentTrim);
                    contentTrim = getBetween(content, "</Item>", "</Item>");
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

        //
    }
}
