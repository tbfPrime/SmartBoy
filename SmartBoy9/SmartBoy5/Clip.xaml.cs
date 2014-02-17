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
        MainWindow main;
        DispatcherTimer dispatcherTimer;

        public Clip(object obj)
        {
            InitializeComponent();
            DataContext = new CurrentSongData();

            // Set Main Window object reference
            main = (MainWindow)obj;
        }

        private void ClipBehavior_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            // On single clicking the clip.
            this.Close();
            main.Show();
        }

        private void ClipBehavior_Loaded_1(object sender, RoutedEventArgs e)
        {
            // this method enables animation.
            this.Notify();

            // call to periodic notifier every 10 secs.
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(PeriodicNotifier);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();

        }

        private void PeriodicNotifier(object sender, EventArgs e)
        {
            try
            {
                this.Notify();
            }
            catch {
                Console.WriteLine("Clip | PeriodicNotifier | Clip Visibility Error.");
            }
        }
    }
}
