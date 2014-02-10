using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SmartBoy
{
    class GetWebClient
    {
        WebClient client = new WebClient();

        public string GetWebString(string a1)
        {
            try
            {
                client.Encoding = System.Text.Encoding.GetEncoding("windows-1252");
                string content = client.DownloadString(a1);
                return content;
            }
            catch
            {
                return "";
            }
        }

        public string GetWebStringWiki(string a1)
        {
            try
            {
                client.Encoding = System.Text.Encoding.GetEncoding("windows-1252");
                client.Headers.Add("user-agent", "Only a test!");
                if (a1 != "N/A")
                {
                    string content = client.DownloadString(a1);
                    return content;
                }
                else
                    return "N/A";
            }
            catch
            {
                return "";
            }
        }

        public string GetWebStringNA(string a1)
        {
            try
            {
                client.Encoding = System.Text.Encoding.GetEncoding("windows-1252");
                string content = client.DownloadString(a1);
                return content;
            }
            catch
            {
                return "N/A";
            }
        }
    }
}
