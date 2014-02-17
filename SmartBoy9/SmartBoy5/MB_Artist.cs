using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class MB_Artist
    {
        StringUtil tools = new StringUtil();
        string content = "";

        public void LookUpv2()
        {
            Console.WriteLine("MB_Artist| LookUpv2 | Initializing...");
            Console.WriteLine("MB_Artist| LookUpv2 | Lookup for Artist data.");

            if (!CurrentSongData.db.Artist_SB.Any(u => u.MB_Artist_ID == CurrentSongData.artistMBID))
            {
                // Pull MusicBrainz Recordings XML content
                content = new GetWebClient().GetWebString(new MB_Lookup_URL_Generator().ArtistLookupURL(CurrentSongData.artistMBID));

                CurrentSongData.artistType = tools.getBetweenNA(content, "<artist type=\"", "\" ");
                CurrentSongData.artistName = tools.getBetweenNA(content, "<name>", "</name>");
                CurrentSongData.artistCountry = tools.getBetweenNA(content, "<country>", "</country>");
                if (CurrentSongData.artistType == "Group")
                    CurrentSongData.artistBegin = tools.getBetweenNA(content, "<begin>", "</begin>");
                else if (CurrentSongData.artistType == "Person")
                {
                    CurrentSongData.artistGender = tools.getBetweenNA(content, "<gender>", "</gender>");
                    CurrentSongData.artistDOB = tools.getBetweenNA(content, "<begin>", "</begin>");
                }

                // Data Log.
                Console.WriteLine("MB_Artist| LookUpv2 | CurrentSongData.artistName: " + CurrentSongData.artistName);
                Console.WriteLine("MB_Artist| LookUpv2 | CurrentSongData.artistType: " + CurrentSongData.artistType);
                Console.WriteLine("MB_Artist| LookUpv2 | CurrentSongData.artistCountry: " + CurrentSongData.artistCountry);
                Console.WriteLine("MB_Artist| LookUpv2 | CurrentSongData.artistBegin: " + CurrentSongData.artistBegin);
                Console.WriteLine("MB_Artist| LookUpv2 | CurrentSongData.artistGender: " + CurrentSongData.artistGender);
            }

            Console.WriteLine("MB_Artist| LookUpv2 | Finalizing...");
        }
    }
}
