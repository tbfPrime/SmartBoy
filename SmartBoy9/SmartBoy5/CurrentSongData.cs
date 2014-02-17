using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace SmartBoy
{
    class CurrentSongData
    {
        public static string filePath, filePathHash, fingerprint, duration;

        private static int arraySize = 60;
        private static int lineSpacing = 10;

        public static string trackTitle = "Sample File Name";
        public static string trackMBID, trackLength;
        private static string[] releaseID = new string[arraySize];
        private static string[] albumTitle = new string[arraySize];
        private static string[] albumStatus = new string[arraySize];
        private static string[] albumQuality = new string[arraySize];
        private static string[] albumDate = new string[arraySize];
        private static string[] albumYear = new string[arraySize];
        private static string[] albumLanguage = new string[arraySize];
        private static string[] albumScript = new string[arraySize];
        private static string[] albumCountry = new string[arraySize];
        private static string[] albumBarcode = new string[arraySize];
        private static string[] albumPackaging = new string[arraySize];

        public static string artistMBID, artistName, artistGender, artistCountry, artistDOB, artistType, artistBegin = "N/A";
        public static string contentTrim = "N/A";
        public static int releaseCount, trackCounter, lyricsLineCount;
        
        public static string trackWiki, albumWiki, artistWiki;

        public static BitmapImage albumArt;
        public static string dominantColor = "#529FD2";
        public static string contrastColor = "#403D3D";

        public static string lyrics;

        public static bool defaultAlbumArt = true;

        public static TestContext db;

        public static event PropertyChangedEventHandler StaticPropertyChanged;

        #region Array Setter / Getter

        public static string[] ReleaseID 
        {
            get 
            {
                return releaseID;
            }
            set 
            {
                releaseID = value;
            }
        }

        public static string[] AlbumTitle
        {
            get
            {
                return albumTitle;
            }
            set
            {
                albumTitle = value;
            }
        }

        public static string[] AlbumStatus
        {
            get
            {
                return albumStatus;
            }
            set
            {
                albumStatus = value;
            }
        }

        public static string[] AlbumQuality
        {
            get
            {
                return albumQuality;
            }
            set
            {
                albumQuality = value;
            }
        }

        public static string[] AlbumDate
        {
            get
            {
                return albumDate;
            }
            set
            {
                albumDate = value;
            }
        }

        public static string[] AlbumYear
        {
            get
            {
                return albumYear;
            }
            set
            {
                albumYear = value;
            }
        }

        public static string[] AlbumLanguage
        {
            get
            {
                return albumLanguage;
            }
            set
            {
                albumLanguage = value;
            }
        }

        public static string[] AlbumScript
        {
            get
            {
                return albumScript;
            }
            set
            {
                albumScript = value;
            }
        }

        public static string[] AlbumCountry
        {
            get
            {
                return albumCountry;
            }
            set
            {
                albumCountry = value;
            }
        }

        public static string[] AlbumBarcode
        {
            get
            {
                return albumBarcode;
            }
            set
            {
                albumBarcode = value;
            }
        }

        public static string[] AlbumPackaging
        {
            get
            {
                return albumPackaging;
            }
            set
            {
                albumPackaging = value;
            }
        }

#endregion

        #region Getter Properties for Bindings

        public static string Title {
            get {
                return trackTitle;
            }
        }

        public static BitmapImage Picture {
            get {
                return albumArt;
            }
        }

        public static string ContrastColor
        {
            get
            {
                if (contrastColor != null)
                {
                    return contrastColor;
                }
                else
                {
                    return "#000000";
                }
            }
        }

        public static string DominantColor
        {
            get
            {
                if (dominantColor != null)
                {
                    return dominantColor;
                }
                else
                {
                    return "#444444";
                }
            }
        }

        public static string Wiki {
            get {
                if (artistWiki != null)
                {
                    return artistWiki;
                }
                else
                {
                    return "Artist Info Not Found.";
                }
            }
        }

        public static string Lyrics {
            get {
                if (lyrics != null)
                {
                    return lyrics;
                }
                else
                {
                    return "Lyrics Not Found.";
                }
            }
        }

        public static int LyricsLineCount
        {
            get
            {
                return lineSpacing * lyricsLineCount;
            }
        }

        public static string ArtistWiki
        {
            get
            {
                if (artistWiki != null)
                {
                    return artistWiki;
                }
                else
                {
                    return "Artist Info Not Found.";
                }
            }
        }

        public static string AlbumWiki
        {
            get
            {
                if (albumWiki != null)
                {
                    return albumWiki;
                }
                else
                {
                    return "Album Info Not Found.";
                }
            }
        }

        public static string TrackWiki
        {
            get
            {
                if (trackWiki != null)
                {
                    return trackWiki;
                }
                else
                {
                    return "Track Info Not Found.";
                }
            }
        }

        #endregion

        #region Functions

        public static void UpdateTags() {
            Console.WriteLine("CurrentSongData | UpdateTags | Initializing...");

            try
            {
                Console.WriteLine("CurrentSongData | UpdateTags");
                albumArt.Freeze();

                StaticPropertyChanged(null, new PropertyChangedEventArgs("Title"));
                StaticPropertyChanged(null, new PropertyChangedEventArgs("Picture"));
                StaticPropertyChanged(null, new PropertyChangedEventArgs("ContrastColor"));
                StaticPropertyChanged(null, new PropertyChangedEventArgs("DominantColor"));
                StaticPropertyChanged(null, new PropertyChangedEventArgs("Wiki"));
                StaticPropertyChanged(null, new PropertyChangedEventArgs("Lyrics"));
                StaticPropertyChanged(null, new PropertyChangedEventArgs("ArtistWiki"));
                StaticPropertyChanged(null, new PropertyChangedEventArgs("AlbumWiki"));
                StaticPropertyChanged(null, new PropertyChangedEventArgs("TrackWiki"));
            }
            catch (Exception e)
            {
                Console.WriteLine("CurrentSongData | UpdateTags | catch | Exception: " + e);
            }

            Console.WriteLine("CurrentSongData | UpdateTags | Finalizing...");
        }

        public static void resetVariables() {
            Console.WriteLine("CurrentSongData | resetVariables | Initailizing...");

            filePathHash = fingerprint = duration = null;
            trackTitle = trackMBID = trackLength = contentTrim = null;
            artistMBID = artistName = artistGender = artistCountry = artistDOB = artistType = artistBegin = null;

            for (int i = 0; i < releaseCount; i++) {
                releaseID[i] = albumTitle[i] = albumStatus[i] = albumQuality[i] = albumDate[i] = albumYear[i] = albumLanguage[i] = albumScript[i] = albumCountry[i] = albumBarcode[i] = albumPackaging[i] = null;
            }

            releaseCount = trackCounter = 0;
            lyricsLineCount = 10;
            trackWiki = albumWiki = artistWiki = null;
            albumArt = null;

            dominantColor = "#529FD2";
            contrastColor = "#403D3D";
            lyrics = null;

            defaultAlbumArt = true;

            Console.WriteLine("CurrentSongData | resetVariables | Finalizing...");
        }

        public static void Commit() {
            Console.WriteLine("CurrentSongData | Commit | to Database");
            Console.WriteLine("CurrentSongData | Commit | Initializing...");

            // Commit code inside try catch block to catch DB write exceptions.
            try
            {
                // Check if Track has been already stored before.
                if (!db.Track_SB.Any(u => u.MB_TrackID == trackMBID))
                {
                    // Begin with Track Data storage.
                    var track_table = new Track_SB();
                    track_table.MB_TrackID = trackMBID;
                    track_table.Title = trackTitle;
                    track_table.Track_Content = trackWiki;
                    track_table.Lyrics = lyrics;
                    db.Track_SB.Add(track_table);
                    db.SaveChanges();

                    for (int i = 0; i < releaseCount; i++)
                    {
                        string temp = releaseID[i];

                        // Check if Album Data already exists.
                        if (!db.Album_SB.Any(u => u.MB_Release_ID == temp))
                        {
                            var album_table = new Album_SB();
                            album_table.MB_Release_ID = releaseID[i];
                            album_table.Album_Name = albumTitle[i];

                            // Check if date is in proper format.
                            if (checkDate(albumDate[i]))
                            {
                                album_table.Release_Date = DateTime.Parse(albumDate[i]);
                            }

                            // Extract Year from date.
                            albumYear[i] = cleanDate(albumDate[i]);

                            album_table.Release_Year = albumYear[i];
                            album_table.Album_Content = albumWiki;
                            album_table.Album_Status = albumStatus[i];
                            album_table.Album_Quality = albumQuality[i];
                            album_table.Album_Packaging = albumPackaging[i];
                            album_table.Album_Language = albumLanguage[i];
                            album_table.Album_Country = albumCountry[i];
                            album_table.Album_Barcode = albumBarcode[i];
                            album_table.Album_Script = albumScript[i];
                            db.Album_SB.Add(album_table);
                            db.SaveChanges();
                        }


                        string albumTemp = trackMBID + releaseID[i];

                        Console.WriteLine("Its time to check for Album Track Relationship..");
                        // Check if Album - Track Relationship already recorded.
                        if (!db.Track_Album_Reln.Any(u => u.id == albumTemp))
                        {
                            Console.WriteLine("Album Track Relationship not found. Creating one.");
                            var albumTrackReln_table = new Track_Album_Reln();
                            albumTrackReln_table.id = trackMBID + releaseID[i];
                            albumTrackReln_table.MB_Track_ID = trackMBID;
                            albumTrackReln_table.MB_AlbumID = releaseID[i];
                            db.Track_Album_Reln.Add(albumTrackReln_table);
                            db.SaveChanges();
                        }
                    }


                    // Store Hash and track Musicbrainz ID foreign key.
                    var id_table = new ID_SB();
                    id_table.Hash = filePathHash;
                    id_table.MB_Track_ID = trackMBID;
                    id_table.Fingerprint = fingerprint;
                    id_table.Duration = duration;
                    id_table.FilePath = filePath;
                    db.ID_SB.Add(id_table);
                    db.SaveChanges();

                    // Check if Artist Data already exists.
                    if (!db.Artist_SB.Any(u => u.MB_Artist_ID == artistMBID) && (artistMBID.Length != 0))
                    {
                        var artist_table = new Artist_SB();
                        artist_table.MB_Artist_ID = artistMBID;
                        artist_table.Artist_Name = artistName;
                        artist_table.Artist_Type = artistType;
                        if (checkDate(artistDOB))
                        {
                            artist_table.Artist_DOB = DateTime.Parse(artistDOB);
                        }
                        if (checkDate(artistBegin))
                        {
                            artist_table.Artist_Begin = cleanDate(artistBegin);
                        }
                        artist_table.Artist_Gender = artistGender;
                        artist_table.Artist_Country = artistCountry;
                        artist_table.Artist_Content = artistWiki;
                        db.Artist_SB.Add(artist_table);
                        db.SaveChanges();

                    }

                    string artistTemp = trackMBID + artistMBID;

                    // Check if Artist - Track Relationship already recorded.
                    if (!db.Track_Artist_Reln.Any(u => u.id == artistTemp) && (artistMBID.Length != 0))
                    {
                        var artistTractReln_table = new Track_Artist_Reln();
                        artistTractReln_table.id = trackMBID + artistMBID;
                        artistTractReln_table.MB_Track_ID = trackMBID;
                        artistTractReln_table.MB_ArtistID = artistMBID;
                        db.Track_Artist_Reln.Add(artistTractReln_table);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("CurrentSongData | Commit | Catch | Exception: " + e);
            }
            Console.WriteLine("CurrentSongData | Commit | Finalizing...");
        }

        public static void CommitOffline() {
            Console.WriteLine("CurrentSongData | CommitOffline | to Database");
            Console.WriteLine("CurrentSongData | CommitOffline | Initializing...");

            try
            {
                // Begin with Storing track data.
                var track_table = new Track_SB();
                track_table.MB_TrackID = trackMBID;
                track_table.Title = trackTitle;
                db.Track_SB.Add(track_table);
                db.SaveChanges();

                // Store Album data.
                var album_table = new Album_SB();
                album_table.MB_Release_ID = releaseID[0];
                album_table.Album_Name = albumTitle[0];
                album_table.Release_Year = albumYear[0];
                db.Album_SB.Add(album_table);
                db.SaveChanges();

                // Store Album track relation
                var albumTrackReln = new Track_Album_Reln();
                albumTrackReln.id = trackMBID + releaseID[0];
                albumTrackReln.MB_Track_ID = trackMBID;
                albumTrackReln.MB_AlbumID = releaseID[0];
                db.Track_Album_Reln.Add(albumTrackReln);
                db.SaveChanges();

                // Store Artist data.
                var artist_table = new Artist_SB();
                artist_table.MB_Artist_ID = artistMBID;
                artist_table.Artist_Name = artistName;
                db.Artist_SB.Add(artist_table);
                db.SaveChanges();

                // Store Artist track relation
                var artistTrackReln = new Track_Artist_Reln();
                artistTrackReln.id = trackMBID + artistMBID;
                artistTrackReln.MB_Track_ID = trackMBID;
                artistTrackReln.MB_ArtistID = artistMBID;
                db.Track_Artist_Reln.Add(artistTrackReln);
                db.SaveChanges();

                // Finally store hashes.
                var id_table = new ID_SB();
                id_table.Hash = filePathHash;
                id_table.MB_Track_ID = trackMBID;
                id_table.Fingerprint = fingerprint;
                id_table.Duration = duration;
                id_table.FilePath = filePath;
                db.ID_SB.Add(id_table);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("CurrentSongData | CommitOffline | Catch | Exception: " + e);
            }
            Console.WriteLine("CurrentSongData | CommitOffline | Finalizing...");
        }

        public static void PullFromDB() {

            try
            {
                Console.WriteLine("CurrentSongData | PullFromDB | catch | Initializing...");

                // Pre-requisite to pulling wiki.
                // Pull Album MBID
                string tempAlbumRelnID = "";
                string tempAlbumID = "";

                var albumRelnIDTemp = from a in db.Track_Album_Reln
                                where a.MB_Track_ID == trackMBID
                                select a.MB_AlbumID;

                foreach (var item in albumRelnIDTemp)
                {
                    tempAlbumRelnID = item;
                }

                tempAlbumID = tempAlbumRelnID;

                // Pull Artist MBID
                string tempArtistRelnID = "";
                string tempArtistID = "";

                var artistRelnIDTemp = from a in db.Track_Artist_Reln
                                      where a.MB_Track_ID == trackMBID
                                      select a.MB_ArtistID;

                foreach (var item in artistRelnIDTemp)
                {
                    tempArtistRelnID = item;
                }

                tempArtistID = tempArtistRelnID;

                //Pull Title
                string tempTitle = "";
                var titleTemp = from a in db.Track_SB
                                       where a.MB_TrackID == trackMBID
                                       select a.Title;

                foreach (var item in titleTemp)
                {
                    tempTitle = item;
                }

                string tempLyrics = "";
                var lyricsTemp = from a in db.Track_SB
                                where a.MB_TrackID == trackMBID
                                select a.Lyrics;

                foreach (var item in lyricsTemp)
                {
                    tempLyrics = item;
                }

                // Pull Wiki Info.
                // Pull Track wiki
                var trackTemp = from a in db.Track_SB
                                where a.MB_TrackID == trackMBID
                                select a.Track_Content;

                string tempTrackWiki = "";
                foreach (var item in trackTemp)
                {
                    tempTrackWiki = item;
                }

                
                // Pull Album Wiki
                string tempAlbumWiki = "";

                var wikiAlbum = from a in db.Album_SB
                                       where a.MB_Release_ID == tempAlbumID
                                       select a.Album_Content;

                foreach (var item in wikiAlbum)
                {
                    tempAlbumWiki = item;
                }

                // Pull Artist Wiki
                string tempArtistWiki = "";
                var wikiArtist = from a in db.Artist_SB
                                where a.MB_Artist_ID == tempArtistID
                                select a.Artist_Content;

                foreach (var item in wikiArtist)
                {
                    tempArtistWiki = item;
                }

                // store pulled values in static variables.
                releaseID[0] = tempAlbumID;
                artistMBID = tempArtistID;
                trackWiki = tempTrackWiki;
                albumWiki = tempAlbumWiki;
                artistWiki = tempArtistWiki;
                trackTitle = tempTitle;
                lyrics = tempLyrics;
            }
            catch (Exception e)
            {
                Console.WriteLine("CurrentSongData | PullFromDB | catch | Exception: " + e);
            }
            Console.WriteLine("CurrentSongData | PullFromDB | catch | Finalizing...");
        }

#endregion

        #region Utility functions

        private static bool checkDate(string date)
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

        private static string cleanDate(string date)
        {
            try
            {
                char[] c = date.ToCharArray();
                StringBuilder s = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    s.Append(c[i]);
                }
                return s.ToString();
            }
            catch {
                return "N/A";
            }
        }

        #endregion

    }
}
