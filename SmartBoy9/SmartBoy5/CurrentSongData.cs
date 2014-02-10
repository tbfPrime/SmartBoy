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

        public static string trackTitle = "Sample File Name";
        public static string trackMBID, trackLength, releaseID, albumTitle, status, quality, date, albumYear, language, script, country, barcode, packaging, contentTrim = "N/A";
        public static string artistMBID, artistName, artistGender, artistCountry, artistDOB, artistType, artistBegin = "N/A";

        public static int releaseCount, trackCounter;
        
        public static string trackWiki, albumWiki, artistWiki;

        public static BitmapImage albumArt;
        public static string dominantColor = "#444444";
        public static string contrastColor = "#000000";

        public static string lyrics;   

        public static TestContext db;

        public static event PropertyChangedEventHandler StaticPropertyChanged;

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

        public static void UpdateTags() {
            Console.WriteLine("CurrentSongData | UpdateTags");
            if (albumArt != null)
            {
                albumArt.Freeze();
            }

            StaticPropertyChanged(null, new PropertyChangedEventArgs("Title"));
            StaticPropertyChanged(null, new PropertyChangedEventArgs("Picture"));
            StaticPropertyChanged(null, new PropertyChangedEventArgs("ContrastColor"));
            StaticPropertyChanged(null, new PropertyChangedEventArgs("DominantColor"));
            StaticPropertyChanged(null, new PropertyChangedEventArgs("Wiki"));
            StaticPropertyChanged(null, new PropertyChangedEventArgs("Lyrics"));
        }

        public static void resetVariables() {
            Console.WriteLine("CurrentSongData | resetVariables | Initailizing...");

            filePathHash = fingerprint = duration = null;
            trackTitle = trackMBID = trackLength = releaseID = albumTitle = status = quality = date = albumYear = language = script = country = barcode = packaging = contentTrim = null;
            artistMBID = artistName = artistGender = artistCountry = artistDOB = artistType = artistBegin = null;
            releaseCount = trackCounter = 0;
            trackWiki = albumWiki = artistWiki = null;
            albumArt = null;
            
            dominantColor = "#444444";
            contrastColor = "#000000";
            lyrics = null;

            Console.WriteLine("CurrentSongData | resetVariables | Finalizing...");
        }

    }
}
