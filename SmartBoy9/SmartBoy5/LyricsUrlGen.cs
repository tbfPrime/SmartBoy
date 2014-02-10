using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class LyricsUrlGen
    {
        private string rootUrl;
        private string searchTag;

        // Generate ChartLyrics API URL for current Song
        public string chartlyrics(string artist, string title) {
            try
            {
                rootUrl = "http://api.chartlyrics.com/apiv1.asmx/";
                searchTag = "SearchLyric?";
                artist = "artist=" + artist;
                title = "song=" + title;
                string temp = new GetWebClient().GetWebString(rootUrl + searchTag + artist + title);
                temp = new StringUtil().getBetweenNA(temp, "<SongUrl>", "</SongUrl>");
                return temp;
            }
            catch {
                Console.WriteLine("LyricsURLGen | chartLyrics | catch");
                return "N/A";
            }
        }

        // Generate AzLyrics URL for Current Song
        public string az(string artist, string title){
            try
            {
                Console.WriteLine("LyricsURLGen | az | Generating Lyrics URL");
                rootUrl = "http://www.azlyrics.com/lyrics/";
                artist = fixString(artist);
                title = fixString(title);
                string temp = rootUrl + artist + "/" + title + ".html";

                Console.WriteLine("LyricsURLGen | az | Lyrics URL: " + temp);
                return temp;
            }
            catch(Exception e) 
            {
                Console.WriteLine("LyricsURLGen | az | catch | Exception: " + e);
                return "N/A";
            }
        }

        // Remove spaces and parse string to lowercase.
        private string fixString(string input) { 
            string temp = input;
            temp = temp.Replace(" ", string.Empty);
            temp = temp.Replace("'", string.Empty);
            return temp.ToLower();            
        }
    }
}
