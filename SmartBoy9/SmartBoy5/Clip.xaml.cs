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
using Clip1;
using System.Threading;
using System.Windows.Threading;

namespace SmartBoy
{
    /// <summary>
    /// Interaction logic for Clip.xaml
    /// </summary>
    public partial class Clip : ClipBehavior
    {
        MainWindow m1;
        DispatcherTimer dispatcherTimer;
        SmartBoyViewModel VM;
        Planner planner;

        System.Threading.Timer Timer;
        int i = 0;
        public Clip()
        {
            InitializeComponent();
            VM = (SmartBoyViewModel)base.DataContext;
            planner = new Planner();
        }

        private void ClipBehavior_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
        }

        private void ClipBehavior_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            m1 = new MainWindow();
            m1.Show();
            dispatcherTimer.Stop();
            this.Close();
        }

        private void ClipBehavior_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.Grid1.Children.Add(planner.Host);
            updateGUI(0);
            this.Notify();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(PeriodicNotifier);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();
            
        }

        private void updateGUI(object state)
        {
            Timer = new System.Threading.Timer(updateGUI, null, 500, 500);
            if (planner.SongChanged)
            {
                UpdateDB();
                VM.UpdateTags(planner.CurrentPath(), planner.CurrentHash);
                planner.SongChanged = false;
                planner.WikiPlan();
                //dispatcherTimer = new System.Windows.Threading.DispatcherTimer(DispatcherPriority.ApplicationIdle);
                //dispatcherTimer.Tick += new EventHandler(UpdateWiki);
                //dispatcherTimer.Interval = new TimeSpan(0, 0, 8);
                //dispatcherTimer.Start();
            }
        }
        private void UpdateDB()
        {
            planner.SongPlan();
        }


        private void PeriodicNotifier(object sender, EventArgs e)
        {

            VM.WikiUpdater(i);
            this.Notify();
            if (i == 3)
                i = 0;
            else
                i++;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
