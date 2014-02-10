using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace SmartBoy
{
    class Taggot
    {
        TagLib.File tagMe;
        string[] dummyString = { "N/A" };

        public Taggot() { }

        public Taggot(string track){
            tagMe = TagLib.File.Create(track);
        }

        public void CurrentTrack(string track)
        {
            tagMe = TagLib.File.Create(track);
        }

        #region Extract Tags

        public string GetAlbum
        {
            get
            {
                if (tagMe.Tag.Album != null)
                    return tagMe.Tag.Album;
                return "N/A";
            }
        }

        public string[] GetArtist
        {
            get
            {
                if (tagMe.Tag.AlbumArtists != null)
                    return tagMe.Tag.Artists;
                return dummyString;
            }
        }

        public string[] GetAlbumArtists
        {
            get
            {
                if (tagMe.Tag.AlbumArtists != null)
                    return tagMe.Tag.AlbumArtists;
                return dummyString;
            }
        }

        public string GetAmazonID
        {
            get
            {
                if (tagMe.Tag.AmazonId != null)
                    return tagMe.Tag.AmazonId;
                return "N/A";

            }
        }

        public uint GetBeatsPerMinute
        {
            get
            {
                if (tagMe.Tag.AlbumArtists != null)
                    return tagMe.Tag.BeatsPerMinute;
                return 3;

            }
        }

        public string GetComment
        {
            get
            {
                if (tagMe.Tag.Comment != null)
                    return tagMe.Tag.Comment;
                return "N/A";
            }
        }

        public string[] GetComposers
        {
            get
            {
                if (tagMe.Tag.Composers != null)
                    return tagMe.Tag.Composers;
                return dummyString;
            }
        }

        public string[] GetComposersSort
        {
            get
            {
                if (tagMe.Tag.ComposersSort != null)
                    return tagMe.Tag.ComposersSort;
                return dummyString;
            }
        }

        public string GetConductor
        {
            get
            {
                if (tagMe.Tag.Conductor != null)
                    return tagMe.Tag.Conductor;
                return "N/A";
            }
        }

        public string GetCopyright
        {
            get
            {
                if (tagMe.Tag.Copyright != null)
                    return tagMe.Tag.Copyright;
                return "N/A";
            }
        }

        public uint GetDisc
        {
            get
            {
                if (tagMe.Tag.Disc != null)
                    return tagMe.Tag.Disc;
                return 0;

            }
        }

        public uint GetDiscCount
        {
            get
            {
                if (tagMe.Tag.DiscCount != null)
                    return tagMe.Tag.DiscCount;
                return 3;

            }
        }

        public string GetFirstAlbumArtist
        {
            get
            {
                if (tagMe.Tag.FirstAlbumArtist != null)
                    return tagMe.Tag.FirstAlbumArtist;
                return "N/A";
            }
        }

        public string GetFirstAlbumArtistSort
        {
            get
            {
                if (tagMe.Tag.FirstAlbumArtistSort != null)
                    return tagMe.Tag.FirstAlbumArtistSort;
                return "N/A";
            }
        }

        public string GetFirstArtist
        {
            get
            {
                if (tagMe.Tag.FirstArtist != null)
                    return tagMe.Tag.FirstArtist;
                return "N/A";
            }
        }

        public string GetFirstComposer
        {
            get
            {
                if (tagMe.Tag.FirstComposer != null)
                    return tagMe.Tag.FirstComposer;
                return "N/A";
            }
        }

        public string GetFirstComposerSort
        {
            get
            {
                if (tagMe.Tag.FirstComposerSort != null)
                    return tagMe.Tag.FirstComposerSort;
                return "N/A";
            }
        }

        public string GetFirstGenre
        {
            get
            {
                if (tagMe.Tag.FirstGenre != null)
                    return tagMe.Tag.FirstGenre;
                return "N/A";
            }
        }

        public string GetFirstPerformer
        {
            get
            {
                if (tagMe.Tag.FirstPerformer != null)
                    return tagMe.Tag.FirstPerformer;
                return "N/A";
            }
        }

        public string GetFirstPerformerSort
        {
            get
            {
                if (tagMe.Tag.FirstPerformerSort != null)
                    return tagMe.Tag.FirstPerformerSort;
                return "N/A";
            }
        }

        public string[] GetGenres
        {
            get
            {
                if (tagMe.Tag.Genres != null)
                    return tagMe.Tag.Genres;
                return dummyString;
            }
        }

        public string GetGrouping
        {
            get
            {
                if (tagMe.Tag.Grouping != null)
                    return tagMe.Tag.Grouping;
                return "N/A";
            }
        }

        public string GetJoinedAlbumArtists
        {
            get
            {
                if (tagMe.Tag.JoinedAlbumArtists != null)
                    return tagMe.Tag.JoinedAlbumArtists;
                return "N/A";
            }
        }

        public string GetJoinedArtists
        {
            get
            {
                if (tagMe.Tag.JoinedArtists != null)
                    return tagMe.Tag.JoinedArtists;
                return "N/A";
            }
        }

        public string GetJoinedComposers
        {
            get
            {
                if (tagMe.Tag.JoinedComposers != null)
                    return tagMe.Tag.JoinedComposers;
                return "N/A";
            }
        }

        public string GetJoinedGenres
        {
            get
            {
                if (tagMe.Tag.JoinedGenres != null)
                    return tagMe.Tag.JoinedGenres;
                return "N/A";
            }
        }

        public string GetJoinedPerformers
        {
            get
            {
                if (tagMe.Tag.JoinedPerformers != null)
                    return tagMe.Tag.JoinedPerformers;
                return "N/A";
            }
        }

        public string GetJoinedPerformersSort
        {
            get
            {
                if (tagMe.Tag.JoinedPerformersSort != null)
                    return tagMe.Tag.JoinedPerformersSort;
                return "N/A";
            }
        }

        public string GetLyrics
        {
            get
            {
                if (tagMe.Tag.Lyrics != null)
                    return tagMe.Tag.Lyrics;
                return "N/A";
            }
        }

        public string GetMusicBrainzArtistId
        {
            get
            {
                if (tagMe.Tag.MusicBrainzArtistId != null)
                    return tagMe.Tag.MusicBrainzArtistId;
                return "N/A";
            }
        }

        public string GetMusicBrainzDiscId
        {
            get
            {
                if (tagMe.Tag.MusicBrainzDiscId != null)
                    return tagMe.Tag.MusicBrainzDiscId;
                return "N/A";
            }
        }

        public string GetMusicBrainzReleaseArtistId
        {
            get
            {
                if (tagMe.Tag.MusicBrainzReleaseArtistId != null)
                    return tagMe.Tag.MusicBrainzReleaseArtistId;
                return "N/A";
            }
        }

        public string GetMusicBrainzReleaseCountry
        {
            get
            {
                if (tagMe.Tag.MusicBrainzReleaseCountry != null)
                    return tagMe.Tag.MusicBrainzReleaseCountry;
                return "N/A";
            }
        }

        public string GetMusicBrainzReleaseId
        {
            get
            {
                if (tagMe.Tag.MusicBrainzReleaseId != null)
                    return tagMe.Tag.MusicBrainzReleaseId;
                return "N/A";
            }
        }

        public string GetMusicBrainzReleaseStatus
        {
            get
            {
                if (tagMe.Tag.MusicBrainzReleaseStatus != null)
                    return tagMe.Tag.MusicBrainzReleaseStatus;
                return "N/A";
            }
        }

        public string GetMusicBrainzReleaseType
        {
            get
            {
                if (tagMe.Tag.MusicBrainzReleaseType != null)
                    return tagMe.Tag.MusicBrainzReleaseType;
                return "N/A";
            }
        }

        public string GetMusicBrainzTrackId
        {
            get
            {
                if (tagMe.Tag.MusicBrainzTrackId != null)
                    return tagMe.Tag.MusicBrainzTrackId;
                return "N/A";
            }
        }

        public string GetMusicIpId
        {
            get
            {
                if (tagMe.Tag.MusicIpId != null)
                    return tagMe.Tag.MusicIpId;
                return "N/A";
            }
        }

        public string[] GetPerformers
        {
            get
            {
                if (tagMe.Tag.Performers != null)
                    return tagMe.Tag.Performers;
                return dummyString;
            }
        }

        public string[] GetPerformersSort
        {
            get
            {
                if (tagMe.Tag.PerformersSort != null)
                    return tagMe.Tag.PerformersSort;
                return dummyString;
            }
        }

        public Bitmap GetPictureBitmap
        {
            get
            {
                Bitmap b2 = new Bitmap(10, 10);
                if (tagMe.Tag.Pictures.Length >= 1)
                {

                    try
                    {
                        TagLib.IPicture pic = tagMe.Tag.Pictures[0];
                        MemoryStream ms = new MemoryStream(pic.Data.Data);
                        b2 = new Bitmap(ms);
                        float width = 32;
                        float height = 32;
                        var image = new Bitmap(b2);
                        float scale = Math.Min(width / image.Width, height / image.Height);
                        var bmp = new Bitmap((int)width, (int)height);
                        var graph = Graphics.FromImage(bmp);
                        var scaleWidth = (int)(image.Width * scale);
                        var scaleHeight = (int)(image.Height * scale);

                        graph.DrawImage(image, new Rectangle(((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight));
                        //bmp.Save(ImageFormat.Bmp);

                        return bmp;
                    }
                    catch { }
                }
                return b2;
            }
        }

        public BitmapSource GetBitmapSource
        {
            get
            {
                return CreateBitmapSourceFromBitmap1(GetPictureBitmap);
            }
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource CreateBitmapSourceFromBitmap1(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            IntPtr hBitmap = bitmap.GetHbitmap();

            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        public static BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            if (Application.Current.Dispatcher == null)
                return null; // Is it possible?

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // You need to specify the image format to fill the stream. 
                    // I'm assuming it is PNG
                    bitmap.Save(memoryStream, ImageFormat.Bmp);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Make sure to create the bitmap in the UI thread
                    if (InvokeRequired)
                        return (BitmapSource)Application.Current.Dispatcher.Invoke(
                            new Func<Stream, BitmapSource>(CreateBitmapSourceFromBitmap),
                            DispatcherPriority.Normal,
                            memoryStream);

                    return CreateBitmapSourceFromBitmap(memoryStream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool InvokeRequired
        {
            get { return Dispatcher.CurrentDispatcher != Application.Current.Dispatcher; }
        }

        private static BitmapSource CreateBitmapSourceFromBitmap(Stream stream)
        {
            BitmapDecoder bitmapDecoder = BitmapDecoder.Create(
                stream,
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.OnLoad);

            // This will disconnect the stream from the image completely...
            WriteableBitmap writable = new WriteableBitmap(bitmapDecoder.Frames.Single());
            writable.Freeze();

            return writable;
        }


        public BitmapImage GetPictures
        {
            get
            {
                if (tagMe.Tag.Pictures.Length >= 1)
                {
                    BitmapImage b1 = new BitmapImage();
                    try
                    {
                        TagLib.IPicture pic = tagMe.Tag.Pictures[0];
                        MemoryStream ms = new MemoryStream(pic.Data.Data);
                        b1.BeginInit();
                        b1.StreamSource = ms;
                        b1.EndInit();
                        return b1;

                    }
                    catch
                    {
                        return new BitmapImage();
                    }
                }
                return new BitmapImage();


            }
        }

        public string GetTitle
        {
            get
            {
                if (tagMe.Tag.Title != null)
                    return tagMe.Tag.Title;
                return "N/A";
            }
        }

        public string GetTitleSort
        {
            get
            {
                if (tagMe.Tag.TitleSort != null)
                    return tagMe.Tag.TitleSort;
                return "N/A";
            }
        }

        public uint GetTrack
        {
            get
            {
                if (tagMe.Tag.Track != null)
                    return tagMe.Tag.Track;
                return 0;

            }
        }

        public uint GetTrackCount
        {
            get
            {
                if (tagMe.Tag.TrackCount != null)
                    return tagMe.Tag.TrackCount;
                return 0;

            }
        }

        public uint GetYear
        {
            get
            {
                if (tagMe.Tag.Year != null)
                    return tagMe.Tag.Year;
                return 0;

            }
        }


        #endregion



    }
}
