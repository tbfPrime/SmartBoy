using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class MB_Artist
    {
        string currentTrackID;
        string content, artist_MBID, artistName, artistGender, artistCountry, artistDOB, artistType, artistBegin = "N/A";

        StringUtil tools = new StringUtil();

        public void MB_Artist_Lookup(string id)
        {
            currentTrackID = id;
            MB_LookUp();
        }

        private string MB_Content(string url)
        {
            GetWebClient fetcher = new GetWebClient();
            return fetcher.GetWebString(url);
        }


        private void MB_LookUp()
        {
            MB_Lookup_URL_Generator url = new MB_Lookup_URL_Generator();
            content = MB_Content(url.RecordingLookupURL(currentTrackID));

            if (content != "")
            {
                artist_MBID = tools.getBetweenNA(content, "<artist id=\"", "\">");
            }

            if (!check_Artist_MB_ID())
            {
                content = MB_Content(url.ArtistLookupURL(artist_MBID));
                artistLookup();
                MB_Storage();
            }
            if (!check_reln())
                MB_Reln_Storage();
        }


        private void artistLookup()
        {
            artistType = tools.getBetweenNA(content, "<artist type=\"", "\" ");
            artistName = tools.getBetweenNA(content, "<name>", "</name>");
            artistCountry = tools.getBetweenNA(content, "<country>", "</country>");
            if (artistType == "Group")
                artistBegin = tools.getBetweenNA(content, "<begin>", "</begin>");
            else if (artistType == "Person")
            {
                artistGender = tools.getBetweenNA(content, "<gender>", "</gender>");
                artistDOB = tools.getBetweenNA(content, "<begin>", "</begin>");
            }
        }

        private void cleanDate()
        {
            try
            {
                char[] c = artistBegin.ToCharArray();
                StringBuilder s = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    s.Append(c[i]);
                }
                artistBegin = s.ToString();
            }
            catch { }
        }

        private bool checkDate(string date)
        {
            try
            {
                DateTime d = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool check_Artist_MB_ID()
        {
            using (var db = new TestContext())
            {
                db.Database.Connection.Open();
                if (db.Artist_SB.Any(u => u.MB_Artist_ID == artist_MBID))
                    return true;

                return false;
            }

        }

        private bool check_reln()
        {
            using (var db = new TestContext())
            {
                string temp = currentTrackID + artist_MBID;
                if (db.Track_Artist_Reln.Any(u => u.id == temp))
                    return true;

                return false;
            }
        }

        private void MB_Reln_Storage()
        {
            using (var db = new TestContext())
            {
                var reln = new Track_Artist_Reln();
                reln.id = currentTrackID + artist_MBID;
                reln.MB_Track_ID = currentTrackID;
                reln.MB_ArtistID = artist_MBID;
                db.Track_Artist_Reln.Add(reln);
                db.SaveChanges();
            }
        }

        private void MB_Storage()
        {
            using (var db = new TestContext())
            {
                var artist = new Artist_SB();
                artist.MB_Artist_ID = artist_MBID;
                artist.Artist_Name = artistName;
                artist.Artist_Type = artistType;
                if (artistBegin != null)
                {
                    cleanDate();
                    artist.Artist_Begin = artistBegin;
                }
                if (checkDate(artistDOB))
                {
                    artist.Artist_DOB = DateTime.Parse(artistDOB);
                    artist.Artist_Gender = artistGender;
                }
                artist.Artist_Country = artistCountry;

                db.Artist_SB.Add(artist);
                db.SaveChanges();
            }
        }

        // New Code

        public void LookUpv2()
        {
            Console.WriteLine("MB_Artist| LookUpv2 | Initializing...");
            Console.WriteLine("MB_Artist| LookUpv2 | Lookup for Artist data.");

            
            //content = new GetWebClient().GetWebString(new MB_Lookup_URL_Generator().RecordingLookupURL(currentTrackID));

            if (!CurrentSongData.db.Artist_SB.Any(u => u.MB_Artist_ID == artist_MBID))
            {
                // Pull MusicBrainz Recordings XML content
                content = MB_Content(new MB_Lookup_URL_Generator().ArtistLookupURL(CurrentSongData.artistMBID));

                CurrentSongData.artistType = tools.getBetweenNA(content, "<artist type=\"", "\" ");
                CurrentSongData.artistName = tools.getBetweenNA(content, "<name>", "</name>");
                CurrentSongData.artistCountry = tools.getBetweenNA(content, "<country>", "</country>");
                if (artistType == "Group")
                    CurrentSongData.artistBegin = tools.getBetweenNA(content, "<begin>", "</begin>");
                else if (artistType == "Person")
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
            // To add relations in Artist reln
            //if (!check_reln())
            //    MB_Reln_Storage();

            Console.WriteLine("MB_Artist| LookUpv2 | Finalizing...");
        }

        //
    }
}
