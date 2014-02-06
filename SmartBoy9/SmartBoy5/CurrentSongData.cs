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
        public static string dominantColor;
        public static string contrastColor = "#000000";

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
                return contrastColor;
            }
        }

        public static string DominantColor
        {
            get
            {
                return dominantColor;
            }
        }

        public static string Wiki {
            get {
                return artistWiki;
            }
        }

        public static void UpdateTags() {
            StaticPropertyChanged(null, new PropertyChangedEventArgs("Title"));
            StaticPropertyChanged(null, new PropertyChangedEventArgs("Picture"));
            StaticPropertyChanged(null, new PropertyChangedEventArgs("ContrastColor"));
            StaticPropertyChanged(null, new PropertyChangedEventArgs("DominantColor"));
            StaticPropertyChanged(null, new PropertyChangedEventArgs("Wiki"));
        }

    }
}
