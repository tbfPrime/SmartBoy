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
                contentTrim = getBetween(content, "<release-list", "</release>");
                string temp = getBetween(content, "<release-list count=\"", "\">");
                try
                {
                    releaseCount = int.Parse(temp);
                }
                catch (Exception e){ }
                string contentEnd;
                for (int i = 0; i < releaseCount; i++)
                {
                    releaseID = getBetween(contentTrim, "release id=\"", "\">");
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

                    contentEnd = getEnd(content, contentTrim);
                    contentTrim = getBetween(contentEnd, "<", "</release>");
                }
                trapdoor = true;
            }
        }

        private void MB_Track_LookUp(string content)
        {
            contentTrim = getBetween(content, "<metadata", "<release-list");
            trackTitle = getBetween(contentTrim, "<title>", "</title>");
            trackLength = getBetween(contentTrim, "<length>", "</length>");
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

            title = getBetween(contentTrim, "<title>", "</title>");
            status = getBetween(contentTrim, "<status>", "</status>");
            quality = getBetween(contentTrim, "<quality>", "</quality>");
            language = getBetween(contentTrim, "<language>", "</language>");
            script = getBetween(contentTrim, "<script>", "</script>");
            date = getBetween(contentTrim, "<date>", "</date>");
            country = getBetween(contentTrim, "<country>", "</country>");
            barcode = getBetween(contentTrim, "<barcode>", "</barcode>");
            packaging = getBetween(contentTrim, "<packaging>", "</packaging>");

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

        private static string getEnd(string strSource, string strStart)
        {
            int Start, End;
            if (strSource.Contains(strStart))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.Length;
                return strSource.Substring(Start, strSource.Length - Start);
            }
            else
            {
                return "";
            }
        }

    }
}
