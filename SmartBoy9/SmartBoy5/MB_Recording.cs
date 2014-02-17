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
        StringUtil tools = new StringUtil();
        string content, contentTrim = "N/A";
                
        public void LookUpv2()
        {
            Console.WriteLine("MB_Recording | LookUpv2 | Initializing...");
            Console.WriteLine("MB_Recording | LookUpv2 | Lookup for Track data.");
            
            // Pull MusicBrainz Recordings XML content
            content = new GetWebClient().GetWebString(new MB_Lookup_URL_Generator().RecordingLookupURL(CurrentSongData.trackMBID)); 
            
            if (content != "")
            {
                if (!CurrentSongData.db.Track_SB.Any(u => u.MB_TrackID == CurrentSongData.trackMBID)) // to check if the Track Data already exists
                {
                    // Extract Artist MB_ID
                    CurrentSongData.artistMBID = tools.getBetweenNA(content, "<artist id=\"", "\">");
                    // Extract Track Data
                    contentTrim = tools.getBetweenNA(content, "<metadata", "<release-list");
                    CurrentSongData.trackTitle = tools.getBetweenNA(contentTrim, "<title>", "</title>");
                    CurrentSongData.trackLength = tools.getBetweenNA(contentTrim, "<length>", "</length>");

                    Console.WriteLine("MB_Recording | LookUpv2 | Found Data");
                    Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.artistMBID: " + CurrentSongData.artistMBID);
                    Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.trackTitle: " + CurrentSongData.trackTitle);
                    Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.trackLength: " + CurrentSongData.trackLength);
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

                Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.releaseCount: " + CurrentSongData.releaseCount);

                for (int i = 0; i < CurrentSongData.releaseCount; i++)
                {
                    Console.WriteLine("MB_Recording | LookUpv2 | AlbumInfo | Release counter: " + (i + 1));
                    try
                    {
                        CurrentSongData.ReleaseID[i] = tools.getBetweenNA(contentTrim, "release id=\"", "\">");

                        string temp = CurrentSongData.ReleaseID[i];

                        if (!CurrentSongData.db.Album_SB.Any(u => u.MB_Release_ID == temp))
                        {

                            // Extract Album Data in which the current track has featured. 
                            // HttpUtility.HtmlDecode is used in some cases to decode HTML char.
                            Console.WriteLine("Before");
                            CurrentSongData.AlbumTitle[i] = HttpUtility.HtmlDecode(tools.getBetweenNA(contentTrim, "<title>", "</title>"));
                            Console.WriteLine("After"); 
                            CurrentSongData.AlbumStatus[i] = tools.getBetweenNA(contentTrim, "<status>", "</status>");
                            CurrentSongData.AlbumQuality[i] = tools.getBetweenNA(contentTrim, "<quality>", "</quality>");
                            CurrentSongData.AlbumLanguage[i] = tools.getBetweenNA(contentTrim, "<language>", "</language>");
                            CurrentSongData.AlbumScript[i] = HttpUtility.HtmlDecode(tools.getBetweenNA(contentTrim, "<script>", "</script>"));
                            CurrentSongData.AlbumDate[i] = tools.getBetweenNA(contentTrim, "<date>", "</date>");
                            CurrentSongData.AlbumCountry[i] = tools.getBetweenNA(contentTrim, "<country>", "</country>");
                            CurrentSongData.AlbumBarcode[i] = tools.getBetweenNA(contentTrim, "<barcode>", "</barcode>");
                            CurrentSongData.AlbumPackaging[i] = HttpUtility.HtmlDecode(tools.getBetweenNA(contentTrim, "<packaging>", "</packaging>"));

                            // Data Log.
                            Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.albumTitle: " + CurrentSongData.AlbumTitle[i]);
                            Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.status: " + CurrentSongData.AlbumStatus[i]);
                            Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.quality: " + CurrentSongData.AlbumQuality[i]);
                            Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.language: " + CurrentSongData.AlbumLanguage[i]);
                            Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.script: " + CurrentSongData.AlbumScript[i]);
                            Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.date: " + CurrentSongData.AlbumDate[i]);
                            Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.country: " + CurrentSongData.AlbumCountry[i]);
                            Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.barcode: " + CurrentSongData.AlbumBarcode[i]);
                            Console.WriteLine("MB_Recording | LookUpv2 | CurrentSongData.packaging: " + CurrentSongData.AlbumPackaging[i]);
                        }
                    }
                    catch (Exception e) 
                    {
                        Console.WriteLine("Log: " + e);
                    }

                    contentEnd = new StringUtil().getEnd(content, contentTrim);
                    contentTrim = tools.getBetweenNA(contentEnd, "<", "</release>");
                }
            }
            Console.WriteLine("MB_Recording | LookUpv2 | Finalizing...");
        }
    }
}