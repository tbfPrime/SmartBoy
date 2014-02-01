using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmartBoy
{
    class WikiXML2Data
    {
        string xmlData;
        string outputContent = "N/A";

        public string getInfo(string rawData)
        {
            if (rawData != "N/A")
            {
                xmlData = rawData;
                xmlExtractor();
                DecodeCharacters();
                removeTags();
                return outputContent;
            }
            return outputContent;
        }

        private void removeTags()
        {
            outputContent = outputContent.Replace("<p>", string.Empty);
            outputContent = outputContent.Replace("</p>", string.Empty);
            outputContent = outputContent.Replace("<i>", string.Empty);
            outputContent = outputContent.Replace("</i>", string.Empty);
            outputContent = outputContent.Replace("<b>", string.Empty);
            outputContent = outputContent.Replace("</b>", string.Empty);
            outputContent = outputContent.Replace("<br>", string.Empty);

        }

        private void xmlExtractor()
        {
            outputContent = getBetween(xmlData, "\"preserve\">", "</extract>");
        }

        private void DecodeCharacters()
        {
            outputContent = HttpUtility.HtmlDecode(outputContent);
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

    }
}
