using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Net;
using System.ComponentModel;

namespace SmartBoy
{
    /// <summary>
    /// Interaction logic for ShowLyrics.xaml
    /// </summary>

    public partial class ShowLyrics : Window
    {
        LyricsCode lyrics;
        public ShowLyrics()
        {
            InitializeComponent();
            lyrics = (LyricsCode)base.DataContext;
        }


    }


}
