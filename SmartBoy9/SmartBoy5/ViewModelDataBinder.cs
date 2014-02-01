using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class ViewModelDataBinder
    {

        string title, WikiContent, TrackID, TrackName, AlbumID, AlbumName, ArtistID, ArtistName, Content, Hash, AlbumReleaseYear, ArtistYear;

        Random rnd = new Random();

        public string Usehash
        {
            set
            {
                Hash = value;
            }

                
        }

        public string Title
        {
            get
            {
                DataFetch();
                return title;
            }
        }

        public string Wiki
        {
            get
            {
                if (WikiContent == "N/A" || WikiContent == null)
                    return LocalWiki();
                return WikiContent;
            }
        }
        
        protected int GetRandomInt(int min, int max)
        {
            return rnd.Next(min, max);
        }

        private string LocalWiki()
        {
            int i = GetRandomInt(1,4);
            string local1 = "This song features in the Album - " + AlbumName;
            string local2 = "The Album - " + AlbumName + " was released in the year " + AlbumReleaseYear;
            string local3 = ArtistName + " began singing career in the year " + ArtistYear;
            string local4 = "This Song is Sung by " + ArtistName;
            //string local5 = ""
        
            switch (i)
	        {
                case 1:
                    return local1;
                case 2:
                    return local2;
                case 3:
                    return local3;
                case 4:
                    return local4;
		        default:
                    return "Foo1";
	        }
            
        }


        public void SpillCurrentInfo()
        {
            using (var db = new TestContext())
            {
                var trackID = from a in db.ID_SB
                              where a.Hash == Hash
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

                var albumYear = from a in db.Album_SB
                                where a.MB_Release_ID == AlbumID
                                select a.Release_Year;
                foreach (var item in albumYear)
                {
                    AlbumReleaseYear = item;
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

                var artistYear = from a in db.Artist_SB
                                 where a.MB_Artist_ID == ArtistID
                                 select a.Artist_Begin;
                foreach (var item in artistYear)
                {
                    ArtistYear = item;
                }
            }
        }


        private void DataFetch()
        {
            using (var db = new TestContext())
            {
                db.Database.Connection.Open();
                var temp = from a in db.Track_SB
                           where a.MB_TrackID == TrackID
                           select a.Title;
                        

                foreach (var item in temp)
                {
                    title = item;
                }
            }
        }


        public void DisplayWiki(int i)
        {
            switch (i)
            {
                case 1:
                    TitleWikiFetcher();
                    break;
                case 2:
                    AlbumWikiFetcher();
                    break;
                case 3:
                    ArtistWikiFetcher();
                    break;

                default:
                    break;
            }
        }


        private void TitleWikiFetcher()
        {
            using (var db = new TestContext())
            {
                db.Database.Connection.Open();
                var temp = from a in db.Track_SB
                           where a.MB_TrackID == TrackID
                           select a.Track_Content;

                foreach (var item in temp)
                {
                    WikiContent = item;
                }
            }
        }

        private void AlbumWikiFetcher()
        {
            using (var db = new TestContext())
            {
                db.Database.Connection.Open();
                var adata = from a in db.Album_SB
                            where a.MB_Release_ID == AlbumID
                            select a.Album_Content;

                foreach (var item in adata)
                {
                    WikiContent = item;
                }


            }
        }

        private void ArtistWikiFetcher()
        {
            using (var db = new TestContext())
            {
                var adata = from a in db.Artist_SB
                            where a.MB_Artist_ID == ArtistID
                            select a.Artist_Content;

                foreach (var item in adata)
                {
                    WikiContent = item;
                }


            }
        }

    }
}       
