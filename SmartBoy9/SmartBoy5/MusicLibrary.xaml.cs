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
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace SmartBoy
{
    /// <summary>
    /// Interaction logic for MusicLibrary.xaml
    /// </summary>
    public partial class MusicLibrary : Window
    {
        SqlConnection sc = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\TBF\Desktop\SmartBoy\SmartBoy9\SmartBoy5\bin\Debug\SmartBoyDatabase1.mdf;Integrated Security=True;Connect Timeout=30");
        public MusicLibrary()
        {
            InitializeComponent();
        }


        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender,
                                                RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                  e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header 
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }


                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(ListView1.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            string str = "SELECT DISTINCT idr.Hash,t.Title,t.Counter,t.Track_length,al.Album_Name,ar.Artist_Name FROM Track_SB t JOIN Track_Album_Reln ta ON t.MB_TrackID = ta.MB_Track_ID JOIN Album_SB al ON ta.MB_AlbumID = al.MB_Release_ID JOIN Track_Artist_Reln tar ON t.MB_TrackID = tar.MB_Track_ID JOIN Artist_SB ar ON tar.MB_ArtistID = ar.MB_Artist_ID JOIN ID_SB idr ON idr.MB_Track_ID = t.MB_TrackID;";
            //"SELECT t.Title,t.Track_length,al.Album_Name FROM Track_SB t JOIN Track_Album_Rels ta ON t.MB_TrackID = ta.MB_Track_ID JOIN Album_SB al ON ta.MB_AlbumID = al.MB_Release_ID";
            cmd.CommandText = str;
            ad.SelectCommand = cmd;
            cmd.Connection = sc;
            DataTable dt = new DataTable();
            ad.Fill(dt);
            ListView1.DataContext = dt.DefaultView;
            sc.Close();
            TreeViewItem newChild = new TreeViewItem();
            newChild.Header = "Recently Played";
            TreeView1.Items.Add(newChild);
            newChild = new TreeViewItem();
            newChild.Header = "Most Played";
            TreeView1.Items.Add(newChild);
            newChild = new TreeViewItem();
            newChild.Header = "Album";
            TreeView1.Items.Add(newChild);
            sc.Open();
            cmd = new SqlCommand("select distinct Album_Name from Album_SB", sc);
            SqlDataReader myReader = null;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                newChild.Items.Add(myReader[0].ToString());
            }
            sc.Close();
            newChild = new TreeViewItem();
            newChild.Header = "Artist";
            TreeView1.Items.Add(newChild);
            sc.Open();
            cmd = new SqlCommand("select distinct Artist_Name from Artist_SB", sc);
            myReader = null;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                newChild.Items.Add(myReader[0].ToString());
            }
            sc.Close();
            newChild = new TreeViewItem();
            newChild.Header = "Year";
            TreeView1.Items.Add(newChild);
            sc.Open();
            cmd = new SqlCommand("select distinct Release_Year from Album_SB", sc);
            myReader = null;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                newChild.Items.Add(myReader[0].ToString());
            }
            sc.Close();
        }

        private void TreeView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TreeView1.SelectedItem != null)
            {
                if (TreeView1.SelectedItem.ToString() == "Recently Played") { }
                else if (TreeView1.SelectedItem.ToString() == "Most Played") { }
                else if (TreeView1.SelectedItem.ToString() == "Artist") { }
                else if (TreeView1.SelectedItem.ToString() == "Album") { }
                else if (TreeView1.SelectedItem.ToString() == "Year") { }
                else
                {
                    try
                    {
                        TreeViewItem tvi = TreeView1.Tag as TreeViewItem;
                        string x = TreeViewTools.GetParentItem(tvi).Header.ToString();
                        if (x == "Artist")
                        {
                            ListViewBySql("where al.Album_Name ='" + TreeView1.SelectedItem.ToString() + "'");
                        }
                        if (x == "Album")
                        {
                            ListViewBySql("where ar.Artist_Name ='" + TreeView1.SelectedItem.ToString() + "'");
                        }
                        if (x == "Year")
                        {
                            ListViewBySql("where al.Release_Year ='" + TreeView1.SelectedItem.ToString() + "'");
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        public void ListViewBySql(string sql)
        {
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            string str = "SELECT DISTINCT idr.Hash,t.Title,t.Counter,t.Track_length,al.Album_Name,ar.Artist_Name FROM Track_SB t JOIN Track_Album_Reln ta ON t.MB_TrackID = ta.MB_Track_ID JOIN Album_SB al ON ta.MB_AlbumID = al.MB_Release_ID JOIN Track_Artist_Reln tar ON t.MB_TrackID = tar.MB_Track_ID JOIN Artist_SB ar ON tar.MB_ArtistID = ar.MB_Artist_ID JOIN ID_SB idr ON idr.MB_Track_ID = t.MB_TrackID " + sql + ";";
            //"SELECT t.Title,t.Track_length,al.Album_Name FROM Track_SB t JOIN Track_Album_Rels ta ON t.MB_TrackID = ta.MB_Track_ID JOIN Album_SB al ON ta.MB_AlbumID = al.MB_Release_ID";
            cmd.CommandText = str;
            ad.SelectCommand = cmd;
            cmd.Connection = sc;
            DataTable dt = new DataTable();
            ad.Fill(dt);
            ListView1.DataContext = dt.DefaultView;
            sc.Close();
        }

        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView drv = ListView1.SelectedItem as DataRowView;
            if ((drv != null))
            {
                sc.Open();
                SqlCommand cmd = new SqlCommand("select filepath from ID_SB where Hash='" + drv[0].ToString() + "';", sc);
                SqlDataReader myReader = null;
                myReader = cmd.ExecuteReader();
                myReader.Read();
                int temp1x = myReader.GetOrdinal("filepath");
                string cog = myReader.GetValue(temp1x).ToString();
                sc.Close();
                System.Diagnostics.Process.Start("wmplayer", "\"" + cog + "\"");
            }
        }

        public void PlaySong()
        {
            DataRowView drv = ListView1.SelectedItem as DataRowView;
            if ((drv != null))
            {
                sc.Open();
                SqlCommand cmd = new SqlCommand("select filepath from ID_SB where Hash='" + drv[0].ToString() + "';", sc);
                SqlDataReader myReader = null;
                myReader = cmd.ExecuteReader();
                myReader.Read();
                int temp1x = myReader.GetOrdinal("filepath");
                string cog = myReader.GetValue(temp1x).ToString();
                sc.Close();
                System.Diagnostics.Process.Start("wmplayer", "\"" + cog + "\"");
            }
        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            ListView1.Width = this.ActualWidth - 165;
            if (e.WidthChanged)
            {
                GridView view = ListView1.View as GridView;

                double width = ListView1.ActualWidth / (view.Columns.Count - 1);
                foreach (GridViewColumn col in view.Columns)
                {
                    col.Width = width;
                }
                view.Columns[0].Width = 1;
            }
        }

        public void ShowLyrics()
        {
            DataRowView drv = ListView1.SelectedItem as DataRowView;
            if ((drv != null))
            {
                Showthelyrics sl = new Showthelyrics();
                sl.Stitle = drv[1].ToString();
                sl.Salbum = drv[4].ToString();
                sl.Sartist = drv[5].ToString();
                sl.Show();
            }
        }


        private void OnItemSelected(object sender, RoutedEventArgs e)
        {
            TreeView1.Tag = e.OriginalSource;
        }

        private void MenuItemPlay_Click(object sender, RoutedEventArgs e)
        {
            PlaySong();
        }

        private void MenuItemShowLyrics_Click(object sender, RoutedEventArgs e)
        {
            ShowLyrics();
        }
    }
}
