using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Data.Linq;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Runtime;
using System.Runtime.InteropServices;

namespace SmartBoy
{
    class PlannerUtilities_1
    {
        Taggot tagger = new Taggot();
        LookupAcoustID acoustid = new LookupAcoustID();
        ExtractWiki wikiAgent = new ExtractWiki();

        string[] keywords;
        public string CreateHash(string path)
        {
            Console.WriteLine("PlannerUtilities | CreateHash | Initializing...");
            using (MD5 md5hash = MD5.Create())
            {
                byte[] data = md5hash.ComputeHash(Encoding.UTF8.GetBytes(path));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++){
                    sBuilder.Append(data[i].ToString("x2"));
                }

                Console.WriteLine("PlannerUtilities | CreateHash | Finalizing...");
                return sBuilder.ToString();
            }
        }

        [DllImport("wininet.dll")]
        public extern static bool InternetGetConnectedState(out int desc, int reservedValue);

        public bool CheckForInternetConnection()
        {
            Console.WriteLine("PlannerUtilities | CheckForInternetConnection | Initializing...");



            try
            {
                using (var stream = new WebClient().OpenRead("http://www.google.com"))
                {
                    Console.WriteLine("PlannerUtilities | CheckForInternetConnection | Internet Available!");
                    return true;
                }
            }
            catch
            {
                Console.WriteLine("PlannerUtilities | CheckForInternetConnection | No Internet.");
                return false;
            }

            //try
            //{ // Create a new WebRequest Object to the mentioned URL.
            //    WebRequest myWebRequest = WebRequest.Create("http://www.google.co.in");

            //    // Set the 'Timeout' property in Milliseconds.
            //    myWebRequest.Timeout = 5000;

            //    // This request will throw a WebException if it reaches the timeout limit before it is able to fetch the resource.
            //    WebResponse myWebResponse = myWebRequest.GetResponse();
            //    return true;
            //}
            //catch (WebException e) 
            //{
            //    Console.WriteLine("PlannerUtilities | CheckForInternetConnection | WebException: " + e);
            //    return false;
            //}


            //Console.WriteLine("PlannerUtilities | CheckForInternetConnection");
            //try
            //{
            //    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            //    {
            //        Console.WriteLine("PlannerUtilities | CheckForInternetConnection | Internet Available!");
            //        return true;
            //    }
            //    else {
            //        return false;
            //    }
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine("PlannerUtilities | CheckForInternetConnection | No Internet :( | Exception: " + e);
            //    return false;
            //}
        }

        public bool TrackMBID_6_plusv2()
        {
            Console.WriteLine("PlannerUtilities | TrackMBID_6_plusv2 | Initializing...");

            string checkID = "";
            var id = from a in CurrentSongData.db.ID_SB
                     where a.Hash == CurrentSongData.filePathHash
                     select a.MB_Track_ID;
            foreach (var item in id)
            {
                checkID = item;
            }

            CurrentSongData.trackMBID = checkID;

            Console.WriteLine("PlannerUtilities | TrackMBID_6_plusv2 | Finalizing...");
            if (checkID.Length > 6)
                return true;
            return false;
        }


        public void FetchTrackMBIDv2()
        {
            Console.WriteLine("PlannerUtilities | FetchTrackMBIDv2 | Initializing...");
            var Fp = from a in CurrentSongData.db.ID_SB
                     where a.Hash == CurrentSongData.filePathHash
                     select a.Fingerprint;

            var Durn = from a in CurrentSongData.db.ID_SB
                       where a.Hash == CurrentSongData.filePathHash
                       select a.Duration;

            foreach (var item in Fp)
            {
                CurrentSongData.fingerprint = item;
            }

            foreach (var item in Durn)
            {
                CurrentSongData.duration = item;
            }

            acoustid.GetRec_IDv2();
            Console.WriteLine("PlannerUtilities | FetchTrackMBIDv2 | Finalizing...");
        }

        public void FlushLocalInfov2()
        {
            Console.WriteLine("PlannerUtilities | FlushLocalInfov2 | Initializing...");

            string trackID = "", artistID = "", albumID = "";

            var trackIDFlush = from a in CurrentSongData.db.ID_SB
                               where a.Hash == CurrentSongData.filePathHash
                               select a.MB_Track_ID;

            foreach (var item in trackIDFlush)
            {
                trackID = item;
            }

            var artistIDFlush = from a in CurrentSongData.db.Track_Artist_Reln
                                where a.MB_Track_ID == trackID
                                select a.MB_ArtistID;

            foreach (var item in artistIDFlush)
            {
                artistID = item;
            }

            var albumIDFlush = from a in CurrentSongData.db.Track_Album_Reln
                               where a.MB_Track_ID == trackID
                               select a.MB_AlbumID;

            foreach (var item in albumIDFlush)
            {
                albumID = item;
            }


            var flushIDRow = from a in CurrentSongData.db.ID_SB
                             where a.Hash == CurrentSongData.filePathHash
                             select a;

            var trackFlush = from a in CurrentSongData.db.Track_SB
                             where a.MB_TrackID == trackID
                             select a;

            var artistFlush = from a in CurrentSongData.db.Artist_SB
                              where a.MB_Artist_ID == artistID
                              select a;

            var albumFlush = from a in CurrentSongData.db.Album_SB
                             where a.MB_Release_ID == albumID
                             select a;

            string albumTemp = trackID + albumID;

            var albumRelnFlush = from a in CurrentSongData.db.Track_Album_Reln
                                 where a.id == albumTemp
                                 select a;

            string artistTemp = trackID + artistID;

            var artistRelnFlush = from a in CurrentSongData.db.Track_Artist_Reln
                                  where a.id == artistTemp
                                  select a;


            foreach (var item in flushIDRow)
            {
                ObjectContext oc = ((IObjectContextAdapter)CurrentSongData.db).ObjectContext;
                oc.DeleteObject(item);
            }
            CurrentSongData.db.SaveChanges();
            foreach (var item in trackFlush)
            {
                ObjectContext oc = ((IObjectContextAdapter)CurrentSongData.db).ObjectContext;
                oc.DeleteObject(item);
            }
            CurrentSongData.db.SaveChanges();
            foreach (var item in artistFlush)
            {
                ObjectContext oc = ((IObjectContextAdapter)CurrentSongData.db).ObjectContext;
                oc.DeleteObject(item);
            }
            CurrentSongData.db.SaveChanges();
            foreach (var item in albumFlush)
            {
                ObjectContext oc = ((IObjectContextAdapter)CurrentSongData.db).ObjectContext;
                oc.DeleteObject(item);
            }
            CurrentSongData.db.SaveChanges();
            foreach (var item in albumRelnFlush)
            {
                ObjectContext oc = ((IObjectContextAdapter)CurrentSongData.db).ObjectContext;
                oc.DeleteObject(item);
            }
            CurrentSongData.db.SaveChanges();
            foreach (var item in artistRelnFlush)
            {
                ObjectContext oc = ((IObjectContextAdapter)CurrentSongData.db).ObjectContext;
                oc.DeleteObject(item);
            }
            CurrentSongData.db.SaveChanges();

            Console.WriteLine("PlannerUtilities | FlushLocalInfov2 | Finalizing...");
        }

        // Offline Meta Storage
        public void Offline_Storagev2()
        {
            Console.WriteLine("PlannerUtilities | Offline_Storagev2 | Initializing...");
            tagger.CurrentTrack(CurrentSongData.filePath);

            // Track Table Data
            CurrentSongData.trackMBID = TrackPKv2();
            CurrentSongData.trackTitle = tagger.GetTitle;
            CurrentSongData.trackCounter = 1;

            // Album Table Data
            CurrentSongData.ReleaseID[0] = AlbumPKv2();
            CurrentSongData.AlbumTitle[0] = tagger.GetAlbum;
            CurrentSongData.AlbumYear[0] = tagger.GetYear.ToString();

            // Artist Table Data
            CurrentSongData.artistMBID = ArtistPKv2();
            if (tagger.GetFirstPerformer == null || tagger.GetFirstPerformer == "N/A") // Check if method returns null
            {
                CurrentSongData.artistName = stringArrayToStringv2(tagger.GetArtist);
            }
            else
            {
                CurrentSongData.artistName = tagger.GetFirstPerformer;
            }

            // Data Log.
            Console.WriteLine("PlannerUtilities | Offline_Storagev2 | CurrentSongData.trackMBID: " + CurrentSongData.trackMBID);
            Console.WriteLine("PlannerUtilities | Offline_Storagev2 | CurrentSongData.trackTitle: " + CurrentSongData.trackTitle);
            Console.WriteLine("PlannerUtilities | Offline_Storagev2 | CurrentSongData.ReleaseID[0]: " + CurrentSongData.ReleaseID[0]);
            Console.WriteLine("PlannerUtilities | Offline_Storagev2 | CurrentSongData.AlbumTitle[0]: " + CurrentSongData.AlbumTitle[0]);
            Console.WriteLine("PlannerUtilities | Offline_Storagev2 | CurrentSongData.AlbumYear[0]: " + CurrentSongData.AlbumYear[0]);
            Console.WriteLine("PlannerUtilities | Offline_Storagev2 | CurrentSongData.artistName: " + CurrentSongData.artistName);

            Console.WriteLine("PlannerUtilities | Offline_Storagev2 | Finalizing...");
        }


        private string TrackPKv2()
        {
            Console.WriteLine("PlannerUtilities | TrackPKv2 | Initializing...");
            char[] c1 = CurrentSongData.fingerprint.ToCharArray();
            char[] c2 = CurrentSongData.filePathHash.ToCharArray();
            StringBuilder s1 = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c2[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c1[i]);
            }

            Console.WriteLine("PlannerUtilities | TrackPKv2 | Finalizing...");
            return s1.ToString();
        }

        private string AlbumPKv2()
        {
            Console.WriteLine("PlannerUtilities | AlbumPKv2 | Initializing...");
            char[] c1 = CurrentSongData.fingerprint.ToCharArray();
            char[] c2 = CurrentSongData.filePathHash.ToCharArray();
            StringBuilder s1 = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c2[i]);
            }
            for (int i = 3; i < 6; i++)
            {
                s1.Append(c1[i]);
            }

            Console.WriteLine("PlannerUtilities | AlbumPKv2 | Finalizing...");
            return s1.ToString();
        }

        private string ArtistPKv2()
        {
            Console.WriteLine("PlannerUtilities | ArtistPKv2 | Initializing...");
            char[] c1 = CurrentSongData.fingerprint.ToCharArray();
            char[] c2 = CurrentSongData.filePathHash.ToCharArray();
            StringBuilder s1 = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c2[i]);
            }
            for (int i = 6; i < 9; i++)
            {
                s1.Append(c1[i]);
            }

            Console.WriteLine("PlannerUtilities | ArtistPKv2 | Finalizing...");
            return s1.ToString();
        }

        private string ComposerPKv2()
        {
            char[] c1 = CurrentSongData.fingerprint.ToCharArray();
            char[] c2 = CurrentSongData.filePathHash.ToCharArray();
            StringBuilder s1 = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c2[i]);
            }
            for (int i = 9; i < 12; i++)
            {
                s1.Append(c1[i]);
            }
            return s1.ToString();
        }

        private string ConductorPKv2()
        {
            char[] c1 = CurrentSongData.fingerprint.ToCharArray();
            char[] c2 = CurrentSongData.filePathHash.ToCharArray();
            StringBuilder s1 = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c2[i]);
            }
            for (int i = 12; i < 15; i++)
            {
                s1.Append(c1[i]);
            }

            return s1.ToString();
        }

        private string stringArrayToStringv2(string[] s)
        {
            Console.WriteLine("PlannerUtilities | stringArrayToStringv2 | Initializing...");
            StringBuilder t = new StringBuilder();
            int count = 0;
            foreach (string a in s)
            {
                if (count > 0)
                    t.Append(", ");

                t.Append(a);
                count++;
            }

            Console.WriteLine("PlannerUtilities | stringArrayToStringv2 | Finalizing...");
            return t.ToString();
        }


        public void ActivateWikiv2()
        {
            Console.WriteLine("PLannerUtilities | ActivateWikiv2 | Initializing...");
            // fetch Track wiki
            keywords = new string[] { CurrentSongData.trackTitle, CurrentSongData.artistName };
            CurrentSongData.trackWiki = wikiAgent.wikiContentv2(CurrentSongData.trackTitle, keywords);

            Console.WriteLine("PLannerUtilities | ActivateWikiv2 | CurrentSongData.trackWiki: " + CurrentSongData.trackWiki);

            // fetch Album wiki
            keywords = new string[] { CurrentSongData.AlbumTitle[0], CurrentSongData.artistName };
            CurrentSongData.albumWiki = wikiAgent.wikiContentv2(CurrentSongData.AlbumTitle[0], keywords);

            Console.WriteLine("PLannerUtilities | ActivateWikiv2 | CurrentSongData.albumWiki: " + CurrentSongData.albumWiki);

            // fetch Artist wiki
            keywords = new string[] { CurrentSongData.artistName };
            CurrentSongData.artistWiki = wikiAgent.wikiContentv2(CurrentSongData.artistName, keywords);

            Console.WriteLine("PLannerUtilities | ActivateWikiv2 | CurrentSongData.artistWiki: " + CurrentSongData.artistWiki);

            Console.WriteLine("PlannerUtilities | ActivateWikiv2 | Finalizing...");
        }
    }
}
