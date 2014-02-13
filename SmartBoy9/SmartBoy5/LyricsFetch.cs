using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class LyricsFetch
    {
        private string raw;
        private string url;
        LyricsUrlGen lyricsURL = new LyricsUrlGen();

        public void LyricsPlan(){
            // Fetch first from AzLyrics
            Console.WriteLine("LyricsFetch | LyricsPlan | Initializing...");
            raw = new GetWebClient().GetWebStringNA(lyricsURL.az(CurrentSongData.artistName, CurrentSongData.trackTitle));

            // to check if lyrics fetched or failed.
            if (azParser().Equals("N/A")) { 

                // Next source ChartLyrics.
                url = lyricsURL.chartlyrics(CurrentSongData.artistName, CurrentSongData.trackTitle);

                Console.WriteLine("Url: " + url);
                if (!url.Equals("N/A"))
                {
                    Console.WriteLine("LyricsFetch | LyricsPlan | No Lyrics Found from AZ");
                    raw = new GetWebClient().GetWebString(url);
                    raw = chartLyricsParser();
                }
                else {
                    CurrentSongData.lyrics = "No Lyrics Available.";
                }
            }

            // Store Lyrics.
            CurrentSongData.lyrics = cleanLyrics(raw);

            // Set the lyrics line count
            CurrentSongData.lyricsLineCount = (CurrentSongData.lyrics.Split('\n').Length >= 10) ? CurrentSongData.lyrics.Split('\n').Length : 10;

            // Data Log.
            Console.WriteLine(CurrentSongData.lyrics);
            Console.WriteLine("LyricsFetch | LyricsPlan | Finalizing...");
        }

        private string azParser() {
            raw = new StringUtil().getBetweenNA(raw, "<!-- start of lyrics -->", "<!-- end of lyrics -->");
            return raw;
        }

        private string chartLyricsParser() {
            raw = new StringUtil().getBetweenNA(raw, "<p>", "</p>");
            return raw;
        }

        private string cleanLyrics(string lyrics) {
            raw = raw.Replace("<i>", string.Empty);
            raw = raw.Replace("</i>", string.Empty);
            return raw.Replace("<br />", string.Empty);
        }
    }
}
