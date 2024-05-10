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
using System.Security.Permissions;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace libPLC
{
    /// <summary>
    /// Interaction logic for dirControl.xaml
    /// </summary>
    /// 
    public class fileWithoutExtensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.IO.Path.GetFileNameWithoutExtension(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public partial class dirControl : UserControl
    {
        public static readonly DependencyProperty pathProperty = DependencyProperty.Register("Path", typeof(string), typeof(dirControl), new FrameworkPropertyMetadata(currentPathChanged));
        public string Path { get { return (string)GetValue(pathProperty); } set { SetValue(pathProperty, value); } }

        public static readonly DependencyProperty dirProperty = DependencyProperty.Register("Dir", typeof(string), typeof(dirControl), new FrameworkPropertyMetadata(currentDirChanged));
        public string Dir { get { return (string)GetValue(dirProperty); } set { SetValue(dirProperty, value); } }

        public static readonly DependencyProperty selectedItemProperty = DependencyProperty.Register("SelectedFile", typeof(string), typeof(dirControl));
        public string SelectedFile { get { return (string)GetValue(selectedItemProperty); } set { SetValue(selectedItemProperty, value); } }

        public string IniName { get; set; }

        // public static readonly DependencyProperty selFileProperty = DependencyProperty.Register("Path", typeof(string), typeof(dirControl));
        // public string Path { get { return (string)GetValue(pathProperty); } set { SetValue(pathProperty, value); } }

        public bool ChangeFile { get; set; }

        FileSystemWatcher watcher;
        public ObservableCollection<string> files { get; set; }

        public ListView dirListView 
        {
            get { return list; }
            set { list = value; }
        }


        public string SelectedFileName
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(SelectedFile); }
        }

        private static void currentDirChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            dirControl ctrl = d as dirControl;
            ctrl.setDir((string)e.NewValue);
        }


        private static void currentPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            dirControl ctrl = d as dirControl;
            ctrl.readPath((string)e.NewValue);
        }

        private void setDir(string dir)
        {
            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string directory = System.IO.Path.GetDirectoryName(filePath);
            if (Directory.Exists(System.IO.Directory.GetParent(directory).FullName + "\\"+dir))
                    Path = System.IO.Directory.GetParent(directory).FullName + "\\"+dir;
        }


        private void readPath(string path)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            DirectoryInfo dinfo = new DirectoryInfo(path);
            FileInfo[] Files = dinfo.GetFiles("*.xml");
            files = new ObservableCollection<string>();
            foreach (FileInfo file in Files)
                files.Add(file.FullName);
            list.ItemsSource = files;
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.xml";
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                  files.Add(e.FullPath);
                    if (ChangeFile)
                    {
                        list.SelectedItem = e.FullPath;

                        Console.WriteLine("Change file requested");
                        ChangeFile = false;
                    }
                }));
                return;
            }
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    files.Remove(e.FullPath);
                }));
                return;
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                //var file = files.Single(x => x == oldFile);
                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].Equals(e.OldFullPath))
                    {
                        files[i] = e.FullPath;
                        return;
                    }
                }
            }));
        }

        public dirControl()
        {
            InitializeComponent();
         }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want really delete "+ System.IO.Path.GetFileNameWithoutExtension(list.SelectedItem.ToString()), "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (list.SelectedItem != null)
                {
                    System.IO.File.Delete((string)list.SelectedItem);
                    Console.WriteLine("selectedItem " + list.SelectedItem.ToString());
                }
            }
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( list.SelectedItem != null)
            SelectedFile = list.SelectedItem.ToString();

            dataModel dm = this.DataContext as dataModel;
            if (IniName != null)
            {
                string fileName = System.IO.Path.GetFileName(SelectedFile);
                Console.WriteLine("INI: " + IniName + " -> " + fileName);
                if ( dm != null )
                dm.setINI.IniWriteValue("setup", IniName, fileName);
  
            }
        }

        private void list_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            recipeControl recipCtrl = new recipeControl();
            recipCtrl.FilePath = (string)list.SelectedItem;
            recipCtrl.DgvType = recipeControl.dgvType.recipe;
            dragPopup popup = new dragPopup();
            popupControl popCtrl = new popupControl(recipCtrl);
            popCtrl.Popup = popup;
            dataModel dm = this.DataContext as dataModel;
            recipCtrl.Width = dm.recipeCtrl.Width;
            recipCtrl.Height = dm.recipeCtrl.Height;
            recipCtrl.DataContext = this.DataContext;
            recipCtrl.Plc = dm.plc["1"];
            popup.Width = popCtrl.Width;
            popup.Height = popCtrl.Height;
            popup.Child = popCtrl;
            popup.Placement = PlacementMode.MousePoint;
            popup.IsOpen = true;
        }
    }
}
