using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmartBoy
{
    class MB_Recording
    {
        int releaseCount;
        string current_MB_Track_ID, trackTitle, trackLength, releaseID, content, title, status, quality, date, language, script, country, barcode, packaging, contentTrim = "N/A";
        bool trapdoor = true;

        StringUtil tools = new StringUtil();

        public void MB_Recording_LookUp(string id)
        {
            current_MB_Track_ID = id;
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
            content = MB_Content(url.RecordingLookupURL(current_MB_Track_ID));


            if (content != "")
            {
                if (!check_Track_MB_ID())
                {
                    MB_Track_LookUp(content);
                    MB_Title_Storage();
                }
                contentTrim = tools.getBetweenNA(content, "<release-list", "</release>");
                string temp = tools.getBetweenNA(content, "<release-list count=\"", "\">");
                try
                {
                    releaseCount = int.Parse(temp);
                }
                catch (Exception){ }
                string contentEnd;
                for (int i = 0; i < releaseCount; i++)
                {
                    releaseID = tools.getBetweenNA(contentTrim, "release id=\"", "\">");
                    if (check_MB_ID())
                    { }
                    else
                    {
                        releaseLookup();
                        DecodeCharacters();
                        MB_Storage();
                    }
                    if (!check_reln())
                        MB_Reln_Storage();

                    contentEnd = new StringUtil().getEnd(content, contentTrim);
                    contentTrim = tools.getBetweenNA(contentEnd, "<", "</release>");
                }
                trapdoor = true;
            }
        }

        private void MB_Track_LookUp(string content)
        {
            contentTrim = tools.getBetweenNA(content, "<metadata", "<release-list");
            trackTitle = tools.getBetweenNA(contentTrim, "<title>", "</title>");
            trackLength = tools.getBetweenNA(contentTrim, "<length>", "</length>");
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

        private bool check_Track_MB_ID()
        {
            using (var db = new TestContext())
            {
                db.Database.Connection.Open();
                if (db.Track_SB.Any(u => u.MB_TrackID == current_MB_Track_ID))
                    return true;

                return false;
            }

        }

        private bool check_MB_ID()
        {
            using (var db = new TestContext())
            {
                db.Database.Connection.Open();
                if (db.Album_SB.Any(u => u.MB_Release_ID == releaseID))
                    return true;

                return false;
            }

        }

        private bool check_reln()
        {
            using (var db = new TestContext())
            {
                string temp = current_MB_Track_ID + releaseID;
                if (db.Track_Album_Reln.Any(u => u.id == temp))
                    return true;
                return false;
            }
        }

        private void cleanDate()
        {
            try
            {
                char[] c = date.ToCharArray();
                StringBuilder s = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    s.Append(c[i]);
                }
                date = s.ToString();
            }
            catch { }
        }

        private void releaseLookup()
        {

            title = tools.getBetweenNA(contentTrim, "<title>", "</title>");
            status = tools.getBetweenNA(contentTrim, "<status>", "</status>");
            quality = tools.getBetweenNA(contentTrim, "<quality>", "</quality>");
            language = tools.getBetweenNA(contentTrim, "<language>", "</language>");
            script = tools.getBetweenNA(contentTrim, "<script>", "</script>");
            date = tools.getBetweenNA(contentTrim, "<date>", "</date>");
            country = tools.getBetweenNA(contentTrim, "<country>", "</country>");
            barcode = tools.getBetweenNA(contentTrim, "<barcode>", "</barcode>");
            packaging = tools.getBetweenNA(contentTrim, "<packaging>", "</packaging>");

        }

        private void DecodeCharacters()
        {
            title = HttpUtility.HtmlDecode(title);
            script = HttpUtility.HtmlDecode(script);
            packaging = HttpUtility.HtmlDecode(packaging);
        }

        private void MB_Title_Storage()
        {
            using (var db = new TestContext())
            {
                var Album = new Album_SB();
                if (trapdoor)
                {
                    var Title = new Track_SB();
                    Title.MB_TrackID = current_MB_Track_ID;
                    Title.Title = trackTitle;
                    try
                    {
                        Title.Track_length = int.Parse(trackLength);
                    }
                    catch { }
                    trapdoor = false;
                    db.Track_SB.Add(Title);
                    db.SaveChanges();
                }
            }
        }

        private void MB_Reln_Storage()
        {
            using (var db = new TestContext())
            {
                var reln = new Track_Album_Reln();
                string temp = current_MB_Track_ID + releaseID;
                reln.id = temp;
                reln.MB_Track_ID = current_MB_Track_ID;
                reln.MB_AlbumID = releaseID;
                db.Track_Album_Reln.Add(reln);
                db.SaveChanges();
            }
        }

        private void MB_Storage()
        {
            using (var db = new TestContext())
            {
                var Album = new Album_SB();

                Album.MB_Release_ID = releaseID;
                Album.Album_Name = title;
                Album.Album_Status = status;
                Album.Album_Quality = quality;
                Album.Album_Packaging = packaging;
                Album.Album_Language = language;
                Album.Album_Script = script;
                if (checkDate(date))
                    Album.Release_Date = DateTime.Parse(date);
                cleanDate();
                Album.Release_Year = date;
                Album.Album_Country = country;
                Album.Album_Barcode = barcode;
                db.Album_SB.Add(Album);

                db.SaveChanges();
            }
        }


        // New Code

        public void LookUpv2()
        {
            // Pull MusicBrainz Recordings XML content
            content = new GetWebClient().GetWebString(new MB_Lookup_URL_Generator().RecordingLookupURL(CurrentSongData.trackMBID)); 
            
            if (content != "")
            {
                if (!CurrentSongData.db.Track_SB.Any(u => u.MB_TrackID == current_MB_Track_ID)) // to check if the Track Data already exists
                {
                    // Extract Artist MB_ID
                    CurrentSongData.artistMBID = tools.getBetweenNA(content, "<artist id=\"", "\">");
                    // Extract Track Data
                    contentTrim = tools.getBetweenNA(content, "<metadata", "<release-list");
                    CurrentSongData.trackTitle = tools.getBetweenNA(contentTrim, "<title>", "</title>");
                    CurrentSongData.trackLength = tools.getBetweenNA(contentTrim, "<length>", "</length>"); 
                    
                    // MB_Title_Storage(); // Storage is bypassed at this stage for use of static variables
                }
                contentTrim = tools.getBetweenNA(content, "<release-list", "</release>");
                try
                {
                    // Extract releaseCount storage
                    CurrentSongData.releaseCount = int.Parse(tools.getBetweenNA(content, "<release-list count=\"", "\">"));
                }
                catch (Exception) { }

                string contentEnd;

                for (int i = 0; i < CurrentSongData.releaseCount; i++)
                {
                    releaseID = tools.getBetweenNA(contentTrim, "release id=\"", "\">");
                    if (!CurrentSongData.db.Album_SB.Any(u => u.MB_Release_ID == releaseID)){

                        // Extract Album Data in which the current track has featured. 
                        // HttpUtility.HtmlDecode is used in some cases to decode HTML char.
                        CurrentSongData.albumTitle = HttpUtility.HtmlDecode(tools.getBetweenNA(contentTrim, "<title>", "</title>"));
                        CurrentSongData.status = tools.getBetweenNA(contentTrim, "<status>", "</status>");
                        CurrentSongData.quality = tools.getBetweenNA(contentTrim, "<quality>", "</quality>");
                        CurrentSongData.language = tools.getBetweenNA(contentTrim, "<language>", "</language>");
                        CurrentSongData.script = HttpUtility.HtmlDecode(tools.getBetweenNA(contentTrim, "<script>", "</script>"));
                        CurrentSongData.date = tools.getBetweenNA(contentTrim, "<date>", "</date>");
                        CurrentSongData.country = tools.getBetweenNA(contentTrim, "<country>", "</country>");
                        CurrentSongData.barcode = tools.getBetweenNA(contentTrim, "<barcode>", "</barcode>");
                        CurrentSongData.packaging = HttpUtility.HtmlDecode(tools.getBetweenNA(contentTrim, "<packaging>", "</packaging>"));

                    }
                    // Add record to Album relations table.
                    //if (!check_reln())
                    //    MB_Reln_Storage();

                    contentEnd = new StringUtil().getEnd(content, contentTrim);
                    contentTrim = tools.getBetweenNA(contentEnd, "<", "</release>");
                }
                trapdoor = true;
            }
        }

        //
    }
}
