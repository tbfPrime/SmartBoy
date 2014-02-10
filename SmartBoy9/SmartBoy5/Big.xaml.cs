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

namespace SmartBoy
{
    /// <summary>
    /// Interaction logic for Big.xaml
    /// </summary>
    public partial class Big : Window
    {
        MainWindow main;
        public Big(object obj)
        {
            main = (MainWindow)obj;
            InitializeComponent();
            DataContext = new CurrentSongData();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            main.Show();
        }

    }
}
