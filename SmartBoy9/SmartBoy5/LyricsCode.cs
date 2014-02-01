using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Net;
using System.ComponentModel;

namespace SmartBoy
{
    class LyricsCode : INotifyPropertyChanged
    {
        string lyrics = "x234";

        public event PropertyChangedEventHandler PropertyChanged;

        public string DisplayLyrics
        {
            get
            {
                return LoadLyrics();
            }
        }

        private string stitle;
        private string salbum;
        private string sartist;

        /// <summary>
        /// get song title
        /// </summary>
        public string Stitle
        {
            get { return this.stitle; }
            set { this.stitle = value; }
        }
        /// <summary>
        /// get song album name
        /// </summary>
        public string Salbum
        {
            get { return this.salbum; }
            set { this.salbum = value; }
        }
        /// <summary>
        /// get song artist name
        /// </summary>
        public string Sartist
        {
            get { return this.sartist; }
            set { this.sartist = value; }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadLyrics();
        }

        private string LoadLyrics()
        {
            try
            {
                string slyrics = pullLyrics(sartist, stitle);
                if (slyrics.Contains("<head>"))
                {
                    slyrics = elyrics(sartist, stitle);
                    lyrics = slyrics;
                    System.Windows.MessageBox.Show(lyrics);
                    return lyrics;
                }
                else if (slyrics == "" || slyrics.Contains("<head>"))
                {
                    slyrics = Hindilyricsoff(salbum, stitle);
                    lyrics = slyrics;
                    if (slyrics == "")
                    {
                        slyrics = Hindiazlyrics(salbum, stitle);
                        lyrics = slyrics;
                        return lyrics;
                    }
                }
                else
                {
                    lyrics = slyrics;

                }
            }
            catch (Exception) { lyrics = "Internet connection unavailable"; }
            RaisePropertyChanged("DisplayLyrics");
            return lyrics;
            //System.Windows.MessageBox.Show(lyrics);
        }

        private string Hindilyricsoff(string album, string title)
        {
            WebClient client = new WebClient();
            string test1 = "http://www.google.co.in/search?q=Site:lyricsoff.com " + album + " " + title;
            string value = client.DownloadString(test1);
            string test2 = getBetween(value, "/url?q=http://www.lyricsoff.com", "&amp;");
            string test3 = "http://www.lyricsoff.com" + test2;
            string value1 = client.DownloadString(test3);
            string dis1 = getBetween(value1, "</u>", "</td>");
            int iStart = 0;
            int iEnd = dis1.Length;
            //Replace webpage standard newline feed with carriage return + newline feed, which is standard on Windows.
            dis1 = slice(dis1, iStart, iEnd).Replace("<br>", Environment.NewLine).TrimEnd();
            dis1 = RemoveBetween(dis1, '<', '>');
            dis1 = dis1.Trim();
            //UpdateLyrics(dis1);
            return dis1;
        }

        private string Hindiazlyrics(string album, string title)
        {
            WebClient client = new WebClient();
            string test1 = "http://www.google.co.in/search?q=Site:www.azlyrics.com " + album + " " + title;
            string value = client.DownloadString(test1);
            string test2 = getBetween(value, "/url?q=http://www.azlyrics.com", "&amp;");
            string test3 = "http://www.azlyrics.com" + test2;
            // Add a user agent header in case the 
            // requested URI contains a query.
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            string value1 = client.DownloadString(test3);

            string dis1 = getBetween(value1, "<!-- start of lyrics -->", "<!-- end of lyrics -->");
            int iStart = 0;
            int iEnd = dis1.Length;
            //Replace webpage standard newline feed with carriage return + newline feed, which is standard on Windows.
            dis1 = slice(dis1, iStart, iEnd).Replace("<br>", Environment.NewLine).TrimEnd();
            dis1 = RemoveBetween(dis1, '<', '>');
            dis1 = dis1.Trim();
            //UpdateLyrics(dis1);
            return dis1;
        }

        private string elyrics(string artist, string title)
        {
            WebClient client = new WebClient();
            string test1 = "http://www.google.co.in/search?q=Site:www.elyrics.net " + sartist + " " + stitle;
            string value = client.DownloadString(test1);
            string test2 = getBetween(value, "/url?q=http://www.elyrics.net", "&amp;");
            string test3 = "http://www.elyrics.net" + test2;
            // Add a user agent header in case the 
            // requested URI contains a query.
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            string value1 = client.DownloadString(test3);
            string dis1 = getBetween(value1, "<div class='ly' style='font-size:12px;' id='lyr'><div id='loading'>", "<em><u>Official lyrics powered by</u></em>");
            int iStart = 0;
            int iEnd = dis1.Length;
            //Replace webpage standard newline feed with carriage return + newline feed, which is standard on Windows.
            dis1 = slice(dis1, iStart, iEnd).Replace("<br>", Environment.NewLine).TrimEnd();
            dis1 = RemoveBetween(dis1, '<', '>');
            dis1 = dis1.Trim();
            //UpdateLyrics(dis1);
            return dis1;
        }


        //Substring method, but with starting index and ending index too.
        private static string slice(string source, int start, int end)
        {
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;
            return source.Substring(start, len);
        }

        //Method replaces first letter of all words to UPPERCASE and replaces all spaces with underscores.
        private static string sanitize(string s)
        {
            char[] array = s.Trim().ToCharArray();
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array).Trim().Replace(' ', '_');
        }

        private string pullLyrics(string strArtist, string strSongTitle)
        {
            WebClient wc = new WebClient();
            string sLyrics = null;
            string sUrl = null;
            int iStart = 0;
            int iEnd = 0;
            sUrl = @"http://lyrics.wikia.com/index.php?title=" + sanitize(strArtist) + ":" + sanitize(strSongTitle) + "&action=edit";
            //Set encoding to UTF8 to handle accented characters.
            wc.Encoding = Encoding.UTF8;
            sLyrics = wc.DownloadString(sUrl);
            //Get surrounding tags.
            iStart = sLyrics.IndexOf("&lt;lyrics>") + 12;
            iEnd = sLyrics.IndexOf("&lt;/lyrics>") - 1;
            //Replace webpage standard newline feed with carriage return + newline feed, which is standard on Windows.
            sLyrics = slice(sLyrics, iStart, iEnd).Replace("\n", Environment.NewLine).TrimEnd();
            //If Lyrics Wikia is suggesting a redirect, pull lyrics for that.
            if (sLyrics.Contains("#REDIRECT"))
            {
                iStart = sLyrics.IndexOf("#REDIRECT [[") + 12;
                iEnd = sLyrics.IndexOf("]]", iStart);
                strArtist = slice(sLyrics, iStart, iEnd).Split(':')[0];
                strSongTitle = slice(sLyrics, iStart, iEnd).Split(':')[1];
                pullLyrics(strArtist, strSongTitle);
            }
            //If lyrics weren't found :-(
            else if (sLyrics.Contains("!-- PUT LYRICS HERE (and delete this entire line) -->"))
                sLyrics = "";

            return sLyrics;
        }

        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Search text within a string
        /// </summary>
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// remove data between
        /// </summary>
        string RemoveBetween(string s, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }

        private void RaisePropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler Handler = PropertyChanged;
            if (Handler != null)
            {
                Handler(this, new PropertyChangedEventArgs(PropertyName));
            }
        }


    }
}
