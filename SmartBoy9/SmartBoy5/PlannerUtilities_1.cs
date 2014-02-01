﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Data.Linq;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;

namespace SmartBoy
{
    class PlannerUtilities_1
    {
        Taggot tagger = new Taggot();
        LookupAcoustID look = new LookupAcoustID();
        ExtractWiki wikiAgent = new ExtractWiki();

        string currentHash, currentFP, currentDurn, wikiContent;
        string[] keywords;

        string TrackID, TrackName, AlbumID, AlbumName, ArtistID, ArtistName, Content; 

        #region Hash

        public string CreateHash(string path)
        {
            using (MD5 md5hash = MD5.Create())
            {
                byte[] data = md5hash.ComputeHash(Encoding.UTF8.GetBytes(path));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++){
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public bool HashExists(string Hash)
        {
            using (var db = new TestContext())
            {
                var idCheck = new ID_SB();

                if (db.ID_SB.Any(u => u.Hash == Hash))
                    return true;
                return false;
            }

        }

        #endregion


        public void ID_Storage36(string hash, string trackID, string fp, string duration)
        {
            using (var db = new TestContext())
            {
                var id_store = new ID_SB();
                id_store.Hash = hash;
                id_store.MB_Track_ID = trackID;
                id_store.Fingerprint = fp;
                id_store.Duration = duration;
                db.ID_SB.Add(id_store);
                db.SaveChanges();
            }
        }

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool TrackMBID_6_plus(string hash)
        {
            string checkID="";
            using (var db = new TestContext())
            {
                var id = from a in db.ID_SB
                         where a.Hash == hash
                         select a.MB_Track_ID;
                foreach (var item in id)
                {
                    checkID = item;   
                }

                if (checkID.Length > 6)
                    return true;
                return false;
            }
        }

        public string FetchTrackMBID(string hash)
        {
            using (var db = new TestContext())
            { 
                var Fp = from a in db.ID_SB
                         where a.Hash == hash
                         select a.Fingerprint;

                var Durn = from a in db.ID_SB
                           where a.Hash == hash
                           select a.Duration;

                foreach (var item in Fp)
                {
                    currentFP = item;
                }

                foreach (var item in Durn)
                {
                    currentDurn = item;
                }

                return look.LookUp(currentFP, currentDurn);

            }
        }

        public void FlushLocalInfo(string hash)
        {
            using (var db = new TestContext())
            {
                string trackID = "", artistID = "", albumID = "";

                var trackIDFlush = from a in db.ID_SB
                                   where a.Hash == hash
                                   select a.MB_Track_ID;

                foreach (var item in trackIDFlush)
                {
                    trackID = item;
                }

                var artistIDFlush = from a in db.Track_Artist_Reln
                                    where a.MB_Track_ID == trackID
                                    select a.MB_ArtistID;

                foreach (var item in artistIDFlush)
                {
                    artistID = item;
                }

                var albumIDFlush = from a in db.Track_Album_Reln
                                   where a.MB_Track_ID == trackID
                                   select a.MB_AlbumID;

                foreach (var item in albumIDFlush)
                {
                    albumID = item;
                }


                var flushIDRow = from a in db.ID_SB
                               where a.Hash == hash
                               select a;

                var trackFlush = from a in db.Track_SB
                                 where a.MB_TrackID == trackID
                                 select a;

                var artistFlush = from a in db.Artist_SB
                                  where a.MB_Artist_ID == artistID
                                  select a;

                var albumFlush = from a in db.Album_SB
                                 where a.MB_Release_ID == albumID
                                 select a;

                

                foreach (var item in flushIDRow)
                {
                    ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
                    oc.DeleteObject(item);
                }
                db.SaveChanges();
                foreach (var item in trackFlush)
                {
                    ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
                    oc.DeleteObject(item);
                }
                db.SaveChanges();
                foreach (var item in artistFlush)
                {
                    ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
                    oc.DeleteObject(item);
                }
                db.SaveChanges();
                foreach (var item in albumFlush)
                {
                    ObjectContext oc = ((IObjectContextAdapter)db).ObjectContext;
                    oc.DeleteObject(item);
                }


                db.SaveChanges();

            }
        }

        public void ID_StorageReload(string hash, string trackID)
        {
            using (var db = new TestContext())
            {
                var idTable = new ID_SB();
                idTable.Hash = hash;
                idTable.MB_Track_ID = trackID;
                idTable.Fingerprint = currentFP;
                idTable.Duration = currentDurn;
                db.ID_SB.Add(idTable);
                db.SaveChanges();
            }
        }


        #region OFFline Meta Storage

        public void Offline_Storage(string path, string hash, string fp, string duration)
        {
            currentHash = hash;
            currentFP = fp;

            using (var db = new TestContext())
            {
                tagger.CurrentTrack(path);


                var trackTable = new Track_SB();
                trackTable.MB_TrackID = TrackPK();
                trackTable.Title = tagger.GetTitle;
                trackTable.Counter = 1;
                db.Track_SB.Add(trackTable);
                db.SaveChanges();

                var albumTable = new Album_SB();
                albumTable.MB_Release_ID = AlbumPK();
                albumTable.Album_Name = tagger.GetAlbum;
                albumTable.Release_Year = tagger.GetYear.ToString();
                db.Album_SB.Add(albumTable);
                db.SaveChanges();

                var trackAlbumReln = new Track_Album_Reln();
                trackAlbumReln.id = TrackPK() + AlbumPK();
                trackAlbumReln.MB_AlbumID = AlbumPK();
                trackAlbumReln.MB_Track_ID = TrackPK();
                db.Track_Album_Reln.Add(trackAlbumReln);
                db.SaveChanges();

                var artistTable = new Artist_SB();
                artistTable.MB_Artist_ID = ArtistPK();
                artistTable.Artist_Name = tagger.GetFirstPerformer;
                if (tagger.GetFirstPerformer == null || tagger.GetFirstPerformer == "N/A")
                    artistTable.Artist_Name = stringArrayToString(tagger.GetArtist);
                db.Artist_SB.Add(artistTable);
                db.SaveChanges();

                var trackArtistReln = new Track_Artist_Reln();
                trackArtistReln.id = TrackPK() + ArtistPK();
                trackArtistReln.MB_ArtistID = ArtistPK();
                trackArtistReln.MB_Track_ID = TrackPK();
                db.Track_Artist_Reln.Add(trackArtistReln);
                db.SaveChanges();

                var idTable = new  ID_SB();
                idTable.Hash = hash;
                idTable.MB_Track_ID = TrackPK();
                idTable.Fingerprint = fp;
                idTable.Duration = duration;
                db.ID_SB.Add(idTable);
                db.SaveChanges();

            }
        }

        private string TrackPK()
        {
            char[] c1 = currentFP.ToCharArray();
            char[] c2 = currentHash.ToCharArray();
            StringBuilder s1 = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c2[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c1[i]);
            }
            return s1.ToString();
        }

        private string AlbumPK()
        {
            char[] c1 = currentFP.ToCharArray();
            char[] c2 = currentHash.ToCharArray();
            StringBuilder s1 = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c2[i]);
            }
            for (int i = 3; i < 6; i++)
            {
                s1.Append(c1[i]);
            }
            return s1.ToString();
        }

        private string ArtistPK()
        {
            char[] c1 = currentFP.ToCharArray();
            char[] c2 = currentHash.ToCharArray();
            StringBuilder s1 = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                s1.Append(c2[i]);
            }
            for (int i = 6; i < 9; i++)
            {
                s1.Append(c1[i]);
            }
            return s1.ToString();
        }

        private string ComposerPK()
        {
            char[] c1 = currentFP.ToCharArray();
            char[] c2 = currentHash.ToCharArray();
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

        private string ConductorPK()
        {
            char[] c1 = currentFP.ToCharArray();
            char[] c2 = currentHash.ToCharArray();
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

        private string stringArrayToString(string[] s)
        {
            StringBuilder t = new StringBuilder();
            int count = 0;
            foreach (string a in s)
            {
                if (count > 0)
                    t.Append(", ");

                t.Append(a);
                count++;
            }
            return t.ToString();
        }

        #endregion



        public void ActivateWiki(string hash)
        {
            using (var db = new TestContext())
            {
                var trackID = from a in db.ID_SB
                              where a.Hash == hash
                              select a.MB_Track_ID;
                foreach (var item in trackID)
                {
                    TrackID = item;
                }

                var trackName = from a in db.Track_SB
                                where a.MB_TrackID == TrackID
                                select a.Title;
                foreach (var item in trackName)
                {
                    TrackName = item;
                }

                var albumID = from a in db.Track_Album_Reln
                              where a.MB_Track_ID == TrackID
                              select a.MB_AlbumID;
                foreach (var item in albumID)
                {
                    AlbumID = item;
                }

                var albumName = from a in db.Album_SB
                                where a.MB_Release_ID == AlbumID
                                select a.Album_Name;
                foreach (var item in albumName)
                {
                    AlbumName = item;
                }

                var artistID = from a in db.Track_Artist_Reln
                               where a.MB_Track_ID == TrackID
                               select a.MB_ArtistID;
                foreach (var item in artistID)
                {
                    ArtistID = item;
                }

                var artistName = from a in db.Artist_SB
                                 where a.MB_Artist_ID == ArtistID
                                 select a.Artist_Name;
                foreach (var item in artistName)
                {
                    ArtistName = item;
                }

                keywords = new string []{TrackName, ArtistName};
                if (!titleWikiExists())
                {
                    wikiContent = wikiAgent.wikiContent(TrackName, keywords);
                    var trackTable = (from a in db.Track_SB
                                      where a.MB_TrackID == TrackID
                                      select a).Single();

                    trackTable.Track_Content = wikiContent;
                    db.SaveChanges();
                }

                keywords = new string[] {AlbumName, ArtistName,};
                if (!albumWikiExists())
                {
                    wikiContent = wikiAgent.wikiContent(AlbumName, keywords);
                    var albumTable = (from a in db.Album_SB
                                      where a.MB_Release_ID == AlbumID
                                      select a).Single();

                    albumTable.Album_Content = wikiContent;
                    db.SaveChanges();
                }

                keywords = new string[] {ArtistName};
                if (!artistWikiExists())
                {
                    wikiContent = wikiAgent.wikiContent(ArtistName, keywords);
                    var artistTable = (from a in db.Artist_SB
                                       where a.MB_Artist_ID == ArtistID
                                       select a).Single();
                    artistTable.Artist_Content = wikiContent;
                    db.SaveChanges();
                }
            }
                
        }




        public bool titleWikiExists()
        {
            using (var db = new TestContext())
            {
                var trackContent = from a in db.Track_SB
                                   where a.MB_TrackID == TrackID
                                   select a.Track_Content;


                foreach (var item in trackContent)
                {
                    Content = item;
                }

                if (db.Track_SB.Any(u=>u.Track_Content == Content))
                    return true;
                return false;
            }
        }

        public bool albumWikiExists()
        {
            using (var db = new TestContext())
            {
               
                var albumContent = from a in db.Album_SB
                                where a.MB_Release_ID == AlbumID
                                select a.Album_Content;
                foreach (var item in albumContent)
                {
                    Content = item;
                }


                if (db.Album_SB.Any(u=>u.Album_Content == Content))
                    return true;
                return false;
            }
        }

        public bool artistWikiExists()
        {
            using (var db = new TestContext())
            {
                var artistContent = from a in db.Artist_SB
                                 where a.MB_Artist_ID == ArtistID
                                 select a.Artist_Name;
                foreach (var item in artistContent)
                {
                    Content = item;
                }


                if (db.Artist_SB.Any(u=>u.Artist_Content == Content))
                    return true;
                return false;
            }
        }


    }
}
