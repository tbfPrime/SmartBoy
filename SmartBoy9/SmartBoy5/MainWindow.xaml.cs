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
//        SmartBoyViewModel VM;
        Planner planner;

//        Clip c1;
//        System.Threading.Timer Timer;
//        DispatcherTimer dispatcherTimer;
//        int i = 0;

        public MainWindow()
        {
            InitializeComponent();
//            VM = (SmartBoyViewModel)base.DataContext;
            planner = new Planner();
            DataContext = new CurrentSongData();
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.Grid1.Children.Add(planner.Host);
            //updateGUI(0);
            //GUITimer();
            //dispatcherTimer = new System.Windows.Threading.DispatcherTimer(DispatcherPriority.ApplicationIdle);
            //dispatcherTimer.Tick += new EventHandler(UpdateWiki);
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 8);
            //dispatcherTimer.Start();
        }

        //public void GUITimer() {
        //    TimerCallback updateTCB = updateGUI;
        //    Timer = new System.Threading.Timer(updateTCB, 0, 1000, 1000);
        //}

        //private void updateGUI(object state)
        //{
        //    //Timer = new System.Threading.Timer(updateGUI, null, 500, 500);
        //    if (planner.SongChanged)
        //    {
        //        UpdateDB();
        //        VM.UpdateTags(planner.CurrentPath(), planner.CurrentHash);
        //        planner.SongChanged = false;
        //        planner.WikiPlan();
        //        //dispatcherTimer = new System.Windows.Threading.DispatcherTimer(DispatcherPriority.ApplicationIdle);
        //        //dispatcherTimer.Tick += new EventHandler(UpdateWiki);
        //        //dispatcherTimer.Interval = new TimeSpan(0, 0, 8);
        //        //dispatcherTimer.Start();
        //    }
        //}

        //private void UpdateDB()
        //{
        //    planner.SongPlan();
        //}

        //private void UpdateWiki(object sender, EventArgs e)
        //{
        //    //planner.WikiPlan();
        //    VM.WikiUpdater(i);
        //    if (i == 4)
        //        i = 0;
        //    else
        //        i++;
        //    CommandManager.InvalidateRequerySuggested();
        //}

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void HorizontalToggleSwitch_Checked_1(object sender, RoutedEventArgs e)
        {
            //this.c1 = new Clip();
            //c1.Show();
            //this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //MusicLibrary ml = new MusicLibrary();
            //ml.Show();
            Console.WriteLine("This is a click.");
        }
    }
}
