using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Globalization;

namespace SmartBoy
{
    class SmartBoyViewModel : INotifyPropertyChanged
    {
        Taggot tagger = new Taggot();
        GenerateColorCode gen = new GenerateColorCode();
        public event PropertyChangedEventHandler PropertyChanged;
        ColorPicker col = new ColorPicker();
        ViewModelDataBinder dataview = new ViewModelDataBinder();
        string hash;

        public void UpdateTags(string path,string hsh)
        {

            tagger.CurrentTrack(path);
            hash = hsh;
            RaisePropertyChanged("Album");
            RaisePropertyChanged("Artist");
            RaisePropertyChanged("AlbumArtists");
            RaisePropertyChanged("AmazonID");
            RaisePropertyChanged("BeatsPerMinute");
            RaisePropertyChanged("Comment");
            RaisePropertyChanged("Composers");
            RaisePropertyChanged("Picture");
            RaisePropertyChanged("Performers");
            RaisePropertyChanged("DominantColor");
            RaisePropertyChanged("Title");
            RaisePropertyChanged("Red");
            RaisePropertyChanged("Green");
            RaisePropertyChanged("Blue");
            RaisePropertyChanged("Code");
            RaisePropertyChanged("Genre");
            RaisePropertyChanged("ContrastColor");
        }

        public void WikiUpdater(int i)
        {
            dataview.DisplayWiki(i);
            RaisePropertyChanged("Wiki"); 
        }


        private void RaisePropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler Handler = PropertyChanged;
            if (Handler != null)
            {
                Handler(this, new PropertyChangedEventArgs(PropertyName));
            }
        }



        public string Title
        {
            get
            {
                dataview.Usehash = hash;
                dataview.SpillCurrentInfo();
                return dataview.Title;
            }
        }

        public string Wiki
        {
            get
            {
                dataview.Usehash = hash;
                dataview.SpillCurrentInfo();
                return dataview.Wiki;
            }
        }

        public BitmapImage Picture
        {
            get
            {
                return tagger.GetPictures;
            }
        }

        public string DominantColor
        {
            get
            {
                return gen.CalculateDominantColor(tagger.GetBitmapSource);
            }
        }


        public string ContrastColor
        {
            get
            {
                //string ccode = gen.CalculateDominantColor(tagger.GetBitmapSource);
                //int argb = Int32.Parse(ccode.Replace("#", ""), NumberStyles.HexNumber);
                //Color clr = Color.FromArgb(argb);
                //return gen.ContrastColor(clr);
                return gen.AvgColorCode(tagger.GetPictureBitmap);
            }
        }
/*
        #region Tag Binders

        public string Album
        {
            get
            {
                return tagger.GetAlbum;
            }
        }

        public StringBuilder Artist
        {
            get
            {
                StringBuilder t = new StringBuilder();
                int count = 0;
                foreach (string a in tagger.GetArtist)
                {
                    if (count > 0)
                        t.Append(", ");

                    t.Append(a);
                    count++;
                }
                return t;
            }
        }

        public StringBuilder AlbumArtists
        {
            get
            {
                StringBuilder t = new StringBuilder();
                int count = 0;
                foreach (string a in tagger.GetAlbumArtists)
                {
                    if (count > 0)
                        t.Append(", ");

                    t.Append(a);
                    count++;
                }
                return t;
            }
        }

        public string AmazonID
        {
            get
            {
                return tagger.GetAmazonID;
            }
        }

        public string BeatsPerMinute
        {
            get
            {
                return tagger.GetBeatsPerMinute.ToString();
            }
        }

        public string Comment
        {
            get
            {
                return tagger.GetComment;
            }
        }

        public StringBuilder Composers
        {
            get
            {
                StringBuilder t = new StringBuilder();
                int count = 0;
                foreach (string a in tagger.GetComposers)
                {
                    if (count > 0)
                        t.Append(", ");

                    t.Append(a);
                    count++;
                }
                return t;
            }
        }

        public string Genre
        {
            get
            {
                StringBuilder t = new StringBuilder();
                int count = 0;
                foreach (string a in tagger.GetGenres)
                {
                    if (count > 0)
                        t.Append(", ");

                    t.Append(a);
                    count++;
                }
                string x = t.ToString();
                return x.ToUpper();
            }
        }

        

        public StringBuilder Performers
        {
            get
            {
                StringBuilder t = new StringBuilder();
                int count = 0;
                foreach (string a in tagger.GetPerformers)
                {
                    if (count > 0)
                        t.Append(", ");

                    t.Append(a);
                    count++;
                }
                return t;
            }
        }

        
        public string Title
        {
            get
            {
                return tagger.GetTitle;
                //return col.GetDominantColor(tagger.GetBitmapSource).ToString();
                //return DominantColor;
            }
        }

        public string Red
        {
            get
            {
                return gen.GetRed;
            }
        }

        public string Green
        {
            get
            {
                return gen.GetGreen;
            }
        }

        public string Blue
        {
            get
            {
                return gen.GetBlue;
            }
        }

        public string Code
        {
            get
            {
                return gen.getCode;
            }
        }

        #endregion

*/
    }
}
