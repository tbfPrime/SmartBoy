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
            raw = new GetWebClient().GetWebString(lyricsURL.az(CurrentSongData.artistName, CurrentSongData.trackTitle));
            if (azParser() == "N/A") { // to check if lyrics fetched or failed.
                // Next source ChartLyrics.
                url = lyricsURL.chartlyrics(CurrentSongData.artistName, CurrentSongData.trackTitle);
                if (url != "N/A")
                {
                    Console.WriteLine("LyricsFetch | LyricsPlan | No Lyrics Found from AZ");
                    raw = new GetWebClient().GetWebString(url);
                    raw = chartLyricsParser();
                }
                else {
                    CurrentSongData.lyrics = "No Lyrics Available.";
                }
            }

            CurrentSongData.lyrics = cleanLyrics(raw);
            Console.WriteLine(CurrentSongData.lyrics);
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
