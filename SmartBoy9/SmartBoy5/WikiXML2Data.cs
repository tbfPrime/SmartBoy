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
        string outputContent = "N/A";

        public string getInfov2(string rawData)
        {
            if (rawData != "N/A")
            {
                Console.WriteLine("WikiXML2Data | getInfov2 | Initializing...");
                // Extract Content from Raw Data and Decode HTML outputs.
                outputContent = HttpUtility.HtmlDecode(new StringUtil().getBetweenNA(rawData, "\"preserve\">", "</extract>"));
                removeTagsv2();
                Console.WriteLine("WikiXML2Data | getInfov2 | Finalizing...");
                return outputContent;
            }
            return outputContent;
        }

        // replace all the below tags with empty string.
        private void removeTagsv2()
        {
            Console.WriteLine("WikiXML2Data | removeTagsv2 | Initializing...");

            outputContent = outputContent.Replace("<p>", string.Empty);
            outputContent = outputContent.Replace("</p>", string.Empty);
            outputContent = outputContent.Replace("<i>", string.Empty);
            outputContent = outputContent.Replace("</i>", string.Empty);
            outputContent = outputContent.Replace("<b>", string.Empty);
            outputContent = outputContent.Replace("</b>", string.Empty);
            outputContent = outputContent.Replace("<br>", string.Empty);

            Console.WriteLine("WikiXML2Data | removeTagsv2 | Finalizing...");
        }
    }
}
