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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Threading;
using System.Threading;

namespace SmartBoy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Planner planner;
        Big b;
        Clip c;
        LyricsViewer l;

        public MainWindow()
        {
            Console.WriteLine("\nStage------------------ 1 ------------------\n");
            Console.WriteLine("MainWindow | Constructor");

            InitializeComponent();

            // Core working code begins here
            planner = new Planner();

            // Data binding to GUI
            DataContext = new CurrentSongData();

            l = new LyricsViewer();
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            // WMP component in window.
            this.Grid1.Children.Add(planner.Host);
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            // For click hold and drag.
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void HorizontalToggleSwitch_Checked_1(object sender, RoutedEventArgs e)
        {
            // Clip Toggle Switch
            c = new Clip(this);
            c.Show();
            this.Hide();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // More Info Button Click
            b = new Big(this);
            b.Show();
            b.Focus();
            this.Hide();
        }

        private void lyricsButton_Click(object sender, RoutedEventArgs e)
        {
            // Lyrics Button Click
            if (!l.IsLoaded)
            {
                l = new LyricsViewer();
            }
            l.Show();
            l.Focus();
        }
    }
}
