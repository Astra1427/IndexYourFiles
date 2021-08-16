using IndexYourFiles.Expands;
using IndexYourFiles.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace IndexYourFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public string currentFolder { get; set; }

        public string CurrentFolder {
            get => currentFolder;
            set {
                currentFolder = value;
                lCurrentPath.Content = value;
            }
        }
        public Process OpenedFileExplorer { get; set; }
        public void Init()
        {
            var drives = SearchHelper.GetAllDriveInfo();

            foreach (var drive in drives)
            {
                cbDisks.Items.Add(new ComboBoxItem
                {
                    FontSize = 20,
                    Content = $"{drive.Drive.Name}({drive.Drive.DriveType})"
                });
            }


            cbDisks.SelectedIndex = 0;
            dgResult.MouseDoubleClick += DgResult_MouseDoubleClick;
            dgResult.SelectionChanged += DgResult_SelectionChanged;
            //dgResult.ContextMenu = new ContextMenu() ;
            //dgResult.ContextMenu.Items.Add("Open in File Explorer");

            //MessageBox.Show(Utilities.Uti.BasePath);

        }

        private void DgResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void DgResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgResult.SelectedItem == null)
            {
                return;
            }
            if (cbDisks.SelectedIndex == 0)
            {
                CurrentFolder = (dgResult.SelectedItem as DriveInfoExpand)?.Drive.Name;
                cbDisks.SelectedIndex = dgResult.SelectedIndex + 1;
            }
            else
            {
                var fsi = dgResult.SelectedItem as FileSystemInfo;
                if (fsi.Attributes == FileAttributes.Directory)
                {
                    AccessFolder(fsi?.FullName);
                }
                else
                {
                    var psi = new ProcessStartInfo(fsi.FullName);
                    Process.Start(psi);
                }

            }
        }

        private void btnSearch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            tvSearchResult.Items.Clear();
            string SearchWord = txtSearch.Text;

            if (SearchWord.Trim() == "")
            {
                return;
            }


            var folders = SearchHelper.SearchDirectories(CurrentFolder, txtSearch.Text);
            var files = SearchHelper.SearchFiles(CurrentFolder, txtSearch.Text);

            dgResult.Visibility = Visibility.Hidden;
            tvSearchResult.Visibility = Visibility.Visible;
            foreach (var item in folders)
            {
                tvSearchResult.Items.Add(ShowGreedyTreeViewItemFolder(item,new TreeViewItem() {Header = item.Directory.Name }));
                
            }

            foreach (var item in files)
            {
                tvSearchResult.Items.Add(new TreeViewItem() { Header = item?.Name });
            }
        }


        public TreeViewItem ShowStrictTreeViewItemFolder(DirectoryInfoExpand die,TreeViewItem treeView)
        {
            if (die == null)
            {
                return null;
            }
            //the Folders of this Directory
            var dis = die.GetDirectories();
            //the Files of this Directory
            var files = SearchHelper.SearchFiles(die.Directory.FullName, txtSearch.Text);
            if (dis == null)
            {
                return null;
            }
            if (dis.Count <= 0)
            {
                return treeView;
            }
            foreach (var item in dis)
            {
                TreeViewItem tvt = new TreeViewItem() { Header = item.Directory.Name};
                if (item.GetDirectories().Count > 0)
                {
                    tvt = ShowStrictTreeViewItemFolder(item,tvt);
                }
                treeView.Items.Add(tvt);
            }
            if (files.Count > 0)
            {
                foreach (var item in files)
                {
                    treeView.Items.Add(new TreeViewItem { Header = item.Name});
                }
            }
            return treeView;
        }

        public TreeViewItem ShowGreedyTreeViewItemFolder(DirectoryInfoExpand die, TreeViewItem treeView)
        {
            if (die == null)
            {
                return null;
            }
            //the Folders of this Directory
            var dis = die.GetAllDirectories();
            //the Files of this Directory
            var files = SearchHelper.SearchFiles(die.Directory.FullName, txtSearch.Text);
            if (dis == null)
            {
                return null;
            }
            foreach (var item in dis)
            {
                TreeViewItem tvt = new TreeViewItem() { Header = item.Directory.Name };
                if (item.Directory.GetDirectories().Length > 0 || item.Directory.GetFiles().Length > 0)
                {
                    tvt = ShowGreedyTreeViewItemFolder(item, tvt);
                }
                else if(!item.Directory.Name.ToLower().Contains(item.SearchWord.ToLower()) && item.Directory.GetFiles().Length == 0)
                {
                    continue;
                }

                treeView.Items.Add(tvt);
            }

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    treeView.Items.Add(new TreeViewItem { Header = file.Name });
                }
            }

            if ((treeView == null || treeView.Items.Count == 0))//&& 
            {
                return null;
            }
            return treeView;
        }


        private void cbDisks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgResult.ItemsSource = null;
            CurrentFolder = "";
            if (cbDisks.SelectedIndex == 0)
            {
                var drives = SearchHelper.GetAllDriveInfo();
                for (int i = 0; i < dgResult.Columns.Count; i++)
                {
                    dgResult.Columns[i].Visibility = Visibility.Visible;
                }
                dgResult.AutoGenerateColumns = false;
                dgResult.ItemsSource = drives;
            }
            else
            {
                AccessFolder(CurrentFolder);
            }
            dgResult.Visibility = Visibility.Visible;
            tvSearchResult.Visibility = Visibility.Hidden;
        }

        public void AccessFolder(string path)
        {

            path = path.Length == 0 || path == "All" ?
                (cbDisks.SelectedItem as ComboBoxItem)?.Content.ToString().Substring(0, 3)
                : path;

            var folders = SearchHelper.ListFolders(path);
            var files = SearchHelper.ListFiles(path);
            if (folders == null || files == null)
            {
                return;
            }
            for (int i = 0; i < dgResult.Columns.Count; i++)
            {
                dgResult.Columns[i].Visibility = Visibility.Hidden;
            }
            List<FileSystemInfo> UFiles = new List<FileSystemInfo>();
            foreach (var item in folders)
            {
                UFiles.Add(item);
            }
            foreach (var item in files)
            {
                UFiles.Add(item);
            }

            dgResult.AutoGenerateColumns = true;
            dgResult.ItemsSource = UFiles;
            CurrentFolder = path;
        }

        private void btnOpenInFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (dgResult.SelectedItem == null)
            {
                return;
            }
            if (dgResult.SelectedItem is FileSystemInfo fsi )
            {
                OpenedFileExplorer = SearchHelper.OpenInFileExplorer(fsi .FullName);
            }
            else if (dgResult.SelectedItem is DriveInfoExpand die)
            {
                OpenedFileExplorer = SearchHelper.OpenInFileExplorer(die.Drive.Name);
            }

        }

    }
}