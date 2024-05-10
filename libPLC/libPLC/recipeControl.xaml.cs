using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Text.RegularExpressions;

namespace libPLC
{
    /// <summary>
    /// Interaction logic for recipeControl.xaml
    /// </summary>
    /// 
    /*
     *            <Binding Path="param"/>
                        <Binding Path="plc"/>
                        <Binding Path="DataContext.plc" ElementName="ctrl"/>*/

    public class getDescFromVariableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null && values.Length >= 3)
            {
                var myKey = values[0] as string;
                var myPlc = values[1] as string;
                var myDict = values[2] as Dictionary<string, iPLC>;

                if (myPlc == null || myPlc =="" ) myPlc = plcSetup.defaultPLC;
  
                if (myDict != null && myDict.ContainsKey(myPlc))
                {
                    if (myKey != null && myPlc != null && myDict[myPlc].tags.ContainsKey(myKey))
                        return myDict[myPlc].tags[myKey].Desc;
                } else
                {
                    return "";
                }
            }
            return "";
           // return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


    public class getOnlineValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            if (values != null && values.Length >= 3)
            {
                
                var myKey = values[0] as string;
                var myDict = values[1] as Dictionary<string, iPLC>;
                var myPlc = values[2] as string;

                if (myDict == null) return "";
                if (myDict[myPlc] == null) return "";

                if (myDict.ContainsKey(myPlc))
                {
                    if (myDict[myPlc].tags != null)
                    {
                        if (myKey != null && myDict[myPlc].tags.ContainsKey(myKey))
                        {
                            return myDict[myPlc].tags[myKey].ValStr;
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                return "";
            }
            
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
      
           throw new NotSupportedException();
        }
    }




    public partial class recipeControl : UserControl
    {


        //public event PropertyChangedEventHandler PropertyChanged;
        //private void OnPropertyChanged([CallerMemberName] String caller = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller)); }

        //bool dataHasChanged;
        //public bool DataHasChanged { get { return dataHasChanged; } set { dataHasChanged = value; } }

        public static readonly DependencyProperty dgvTypeProperty = DependencyProperty.Register("DgvType", typeof(dgvType), typeof(recipeControl), new FrameworkPropertyMetadata(dgvTypeHasChanged));
        public dgvType DgvType { get { return (dgvType)GetValue(dgvTypeProperty); } set { SetValue(dgvTypeProperty, value); } }

        private static void dgvTypeHasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            recipeControl ctrl = d as recipeControl;
            ctrl.createLayout((dgvType)e.NewValue);
        }

        public static readonly DependencyProperty plcProperty = DependencyProperty.Register("Plc", typeof(iPLC), typeof(recipeControl));
        public iPLC Plc { get { return (iPLC)GetValue(plcProperty); } set { SetValue(plcProperty, value); } }


        public static readonly DependencyProperty dataHasChangedProperty = DependencyProperty.Register("DataHasChanged", typeof(bool), typeof(recipeControl));
        public bool DataHasChanged { get { return (bool)GetValue(dataHasChangedProperty); } set { SetValue(dataHasChangedProperty, value); } }


        public static readonly DependencyProperty filePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(recipeControl), new FrameworkPropertyMetadata(currentFileChanged));
        public string FilePath  { get { return (string)GetValue(filePathProperty); } set { SetValue(filePathProperty, value); }}

        public static readonly DependencyProperty dirControlProperty = DependencyProperty.Register("DirControl", typeof(dirControl), typeof(recipeControl));
        public dirControl DirControl { get { return (dirControl)GetValue(dirControlProperty); } set { SetValue(dirControlProperty, value); } }

        public enum dgvType { none,recipe,variables }


        public DataSet DataSet { get; set; }
        public string RecipePath { get; set; }

        public string PlcName { get; set; }



        private static void currentFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        { 
            recipeControl ctrl = d as recipeControl;
            ctrl.openFile((string)e.NewValue);
        }

        private void createLayout(dgvType type)
        {
            if (type == dgvType.recipe)
            {

                dataGrid.AutoGenerateColumns = false;
                DataGridTextColumn colPlc = new DataGridTextColumn();
                colPlc.Header = "plc";
                colPlc.Binding = new Binding("plc");
                dataGrid.Columns.Add(colPlc);

                DataGridTextColumn colParam = new DataGridTextColumn();
                colParam.Header = "Parameter";
                colParam.Binding = new Binding("param");
                dataGrid.Columns.Add(colParam);

                DataGridTextColumn colValue = new DataGridTextColumn();
                colValue.Header = "Value";
                colValue.Width = 50;
                //colValue.CellStyle = (Style)this.FindResource("ValueStyle");
                colValue.Binding = new Binding("value");
                dataGrid.Columns.Add(colValue);

                DataGridTemplateColumn colDesc = new DataGridTemplateColumn();
                colDesc.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                colDesc.CellTemplate = (DataTemplate)this.FindResource("descTemplate");
                dataGrid.Columns.Add(colDesc);
            }
            else if ( type == dgvType.variables)
            {
                
                dataGrid.AutoGenerateColumns = true;
                DataGridTemplateColumn colOnline = new DataGridTemplateColumn();
                //  colOnline.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                colOnline.Width = 50;
                colOnline.CellTemplate = (DataTemplate)this.FindResource("onlineTemplate");
                dataGrid.Columns.Add(colOnline);

            }
            else
            {
                dataGrid.AutoGenerateColumns = true;
            }

        }
        private void openFile(string filePath)
        {

            string openFile;
            if (!File.Exists(filePath))
                openFile = RecipePath + "\\" + filePath;
            else
                openFile = filePath;

            if (File.Exists(openFile))
            {
                DataSet = new DataSet();
                DataSet.ReadXml(openFile);
                DataView dataView = new DataView(DataSet.Tables[0]);
                dataGrid.ItemsSource = dataView;
     
                fileTextBlock.Text = System.IO.Path.GetFileNameWithoutExtension(openFile);
                DataSet.Tables[0].RowChanged += RecipeControl_RowChanged;
                DataSet.Tables[0].RowDeleted += RecipeControl_RowDeleted;
                DataHasChanged = true;
                PlcName = System.IO.Path.GetFileNameWithoutExtension(openFile);
            }
            else
            {
               fileTextBlock.Text = "NOT " + System.IO.Path.GetFileNameWithoutExtension(openFile);
            }

        }

        private void RecipeControl_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
           // Console.WriteLine("Action Datasetissa : " + e.Action);
          //  DataHasChanged = true;
        }

        private void RecipeControl_RowChanged(object sender, DataRowChangeEventArgs e)
        {
           // Console.WriteLine("Action Datasetissa : "+e.Action);

            if ( e.Action == DataRowAction.Change)
                DataHasChanged = true;
            // DataHasChanged = true;
        }

        public recipeControl()
        {
            InitializeComponent();
            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string directory = System.IO.Path.GetDirectoryName(filePath);
            RecipePath = System.IO.Directory.GetParent(directory).FullName + "\\recipes";
        }

        private void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            Console.WriteLine("BEGINNING EDIT");
           // if (!MainWindow.dm.Touch) return;;
            var cellInfo = dataGrid.CurrentCell;
            //Console.WriteLine("Count :; " + DataSet.Tables[0].Rows.Count);
            //int index = MainWindow.dm.DataSet.Tables[0].Rows.IndexOf(dataRow.Row);
            if (cellInfo.IsValid)
            {
                var content = cellInfo.Column.GetCellContent(cellInfo.Item);

                if (cellInfo.Column.DisplayIndex == 3 && DgvType == dgvType.recipe) return;
                DataRowView row = (DataRowView)content.DataContext;

                int index = DataSet.Tables[0].Rows.IndexOf(row.Row);
                if (index == -1)
                {
                    dataGrid.CommitEdit();

                    if (dataGrid.Items.Count > 0)
                    {
                        var border = VisualTreeHelper.GetChild(dataGrid, 0) as Decorator;
                        if (border != null)
                        {
                            var scroll = border.Child as ScrollViewer;
                            if (scroll != null) scroll.ScrollToEnd();
                        }
                    }
                }

                iInputPad numPad = null;
                if ( DgvType == dgvType.recipe)
                {
                    switch (cellInfo.Column.DisplayIndex)
                    {
                        case 0: numPad = new inputAlpha(); break;
                        case 1: numPad = new inputParam(); break;
                        case 2: numPad = new inputNumeric(); break;
                    }
                } else 
                {
                    if (cellInfo.Column.DisplayIndex < 5)
                        numPad = new inputAlpha();
                    else
                        numPad = new inputNumeric();
                }
                
                TextBlock textblock = content as TextBlock;
                if (textblock == null) return;
                Binding myBinding = BindingOperations.GetBinding(textblock, TextBlock.TextProperty);

                if (myBinding == null) return;

                Binding newBinding = new Binding(myBinding.Path.Path);
                newBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
                numPad.TextBoxVal.SetBinding(TextBox.TextProperty, newBinding);
                numPad.TextBoxVal.DataContext = textblock.DataContext;
                numPad.DataContext = this.DataContext;

                string title = myBinding.Path.Path;
                Regex rgx = new Regex("\\[(.*?)\\]", RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(title);
                foreach (Match match in matches)
                {
                    foreach (Capture capture in match.Groups[1].Captures)
                        title = capture.ToString();
                }


                dragPopup popup = new dragPopup();
                popupControl popCtrl = new popupControl(numPad.ctrl);
                popCtrl.title.Text = title;
                popCtrl.Popup = popup;
                numPad.Popup = popup;
                popup.Width = popCtrl.Width;
                popup.Height = popCtrl.Height;
                popup.Child = popCtrl;
                popup.Placement = PlacementMode.MousePoint;
                popup.IsOpen = true;

                e.Cancel = true;
                //numPad.Focus();
            }
        }

        private void buttonDelRow_Click(object sender, RoutedEventArgs e)
        {
            List<DataRowView> drvList = new List<DataRowView>();
            var cellInfos = dataGrid.SelectedCells;

            /*
            DataGridTemplateColumn colOnline = null;
            if (DgvType == dgvType.variables)
            {
                colOnline = dataGrid.Columns[0] as DataGridTemplateColumn;
                colOnline.CellTemplate = null;
            }*/

            Console.WriteLine("dataGrid.SelectedIndex " + dataGrid.SelectedIndex);

            bool removeSuccess = false;
            int index = 0;
            if (dataGrid.SelectedCells.Count > 0)
            {
                if (cellInfos.First().IsValid)
                {
                    var contentFirst = cellInfos.First().Column.GetCellContent(cellInfos.First().Item);
                    DataRowView rowView = (DataRowView)contentFirst.DataContext;
                    index = DataSet.Tables[0].Rows.IndexOf(rowView.Row);
                    removeSuccess = true;
                }
            }
    
            foreach (DataGridCellInfo cellInfo in cellInfos)
            {
                if (cellInfo.IsValid)
                {
                    var content = cellInfo.Column.GetCellContent(cellInfo.Item);
                    DataRowView row = (DataRowView)content.DataContext;
                    drvList.Add(row);
                }
            }
            foreach (DataRowView drv in drvList.Distinct())
            {
                //object[] obj = drv.Row.ItemArray;
                DataSet.Tables[0].Rows.Remove(drv.Row);
            }
            if (removeSuccess)
            {
                dataGrid.SelectedIndex = index;
                dataGrid.Focus();
            }
            /*
            if (DgvType == dgvType.variables)
                colOnline.CellTemplate = (DataTemplate)this.FindResource("onlineTemplate");*/
        }

        private void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("    ONGELMA "+dataGrid.SelectedIndex);
            DataGridTemplateColumn colOnline = null;

            if (DgvType == dgvType.variables)
                {
                colOnline = dataGrid.Columns[0] as DataGridTemplateColumn;
                colOnline.CellTemplate = null;
                }

            if (dataGrid.SelectedIndex == -1)
                DataSet.Tables[0].Rows.Add(DataSet.Tables[0].NewRow());
            else
                DataSet.Tables[0].Rows.InsertAt(DataSet.Tables[0].NewRow(), dataGrid.SelectedIndex);

            if (DgvType == dgvType.variables)
                colOnline.CellTemplate = (DataTemplate)this.FindResource("onlineTemplate");

        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            //   BindingExpression be = fileTextBlock.GetBindingExpression(TextBox.TextProperty);
            //  be.UpdateSource();
            string dir = System.IO.Path.GetDirectoryName(FilePath);
            string ext = System.IO.Path.GetExtension(FilePath);
            string saveFile = dir + "\\" + fileTextBlock.Text + ext;
            Console.WriteLine("save :" + saveFile);

            if ( DirControl != null )
                DirControl.ChangeFile = true;

            Console.WriteLine("dir : "+dir);
            if (dir == "")
            {
                DataSet.Tables[0].DefaultView.ToTable().WriteXml(System.IO.Path.GetFileName(saveFile));
            }
            else
            {
                Console.WriteLine("Saving file " + saveFile);
                DataSet.Tables[0].DefaultView.ToTable().WriteXml(saveFile);
            }
            /*
            string saveFile = fileTextBlock.Text;
            if (!Directory.Exists( System.IO.Path.GetDirectoryName(FilePath)))
               saveFile = RecipePath + "\\" + FilePath;
            Console.WriteLine("Saving File "+ saveFile);
            DataSet.Tables[0].DefaultView.ToTable().WriteXml(saveFile);
            Console.WriteLine("Save file : "+saveFile);*/

}

    private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            openFile(FilePath);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }

        private void UserControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
          //  Console.WriteLine("Double Click");
        }

        private void dataGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           // Console.WriteLine("Data context changed");
        }

        /*
         * 
        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {

            dataGrid.SelectionChanged -= dataGrid_SelectionChanged;

            bool first = true;
            int selIndex = 0;
            List<int> lst = new List<int>();
            List<DataRowView> items =dataGrid.SelectedItems.Cast<DataRowView>().ToList();
            foreach (DataRowView item in items.OrderBy(x => DataSet.Tables[0].Rows.IndexOf(x.Row)))
            {
                int index = DataSet.Tables[0].Rows.IndexOf(item.Row);
                if (index < 1 ) return;
                if ( first )
                {
                    first = false;
                    selIndex = index-1;
                }
       
                DataRow itemTmp = DataSet.Tables[0].NewRow();
                itemTmp.ItemArray = item.Row.ItemArray;
                DataSet.Tables[0].Rows.RemoveAt(index);
                DataSet.Tables[0].Rows.InsertAt(itemTmp, index - 1);
                lst.Add(index - 1);
            }

            dataGrid.UpdateLayout();
            var rows = GetDataGridRows(dataGrid);
            int i = 0;
            foreach (DataGridRow dr in rows)
            {
                if (lst.Contains(i))
                {
                    dr.IsSelected = true;
                    dr.Focusable = true;
                }
                i++;
            }

            //dataGrid.SelectedIndex = selIndex;
            dataGrid.SelectionChanged += dataGrid_SelectionChanged;
            dataGridSelectionChange();

            dataGrid.Focus();

        }*/


      
        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {

            dataGrid.SelectionChanged -= dataGrid_SelectionChanged;

            bool first = true;
            int selIndex = 0;
            List<DataRow> seldr = new List<DataRow>();
            List<DataRowView> items = dataGrid.SelectedItems.OfType<DataRowView>().Cast<DataRowView>().ToList();


            DataGridTemplateColumn colOnline = null;
            if (DgvType == dgvType.variables)
            {
                colOnline = dataGrid.Columns[0] as DataGridTemplateColumn;
                colOnline.CellTemplate = null;
            }

            if (items.Count == 0) goto jump;

            foreach (DataRowView item in items.OrderBy(x => DataSet.Tables[0].Rows.IndexOf(x.Row)))
            {
                int index = DataSet.Tables[0].Rows.IndexOf(item.Row);
                if (index < 1 ) break;
                if ( first )
                {
                    first = false;
                    selIndex = index-1;
                }
                DataRow itemTmp = DataSet.Tables[0].NewRow();
                itemTmp.ItemArray = item.Row.ItemArray;
                DataSet.Tables[0].Rows.RemoveAt(index);
                DataSet.Tables[0].Rows.InsertAt(itemTmp, index - 1);
                seldr.Add(DataSet.Tables[0].Rows[index-1]);
            }

            dataGrid.UpdateLayout();
            updateSelectionDataGrid(seldr);

            jump:
            dataGrid.SelectionChanged += dataGrid_SelectionChanged;
            dataGridSelectionChange();

            if (DgvType == dgvType.variables)
                colOnline.CellTemplate = (DataTemplate)this.FindResource("onlineTemplate");

            dataGrid.Focus();
        }

        private void updateSelectionDataGrid(List<DataRow> seldr)
        {
            List<DataRowView> itemsAll = dataGrid.ItemsSource.Cast<DataRowView>().ToList();
            foreach (DataRowView item in itemsAll)
            {
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    if (seldr.Contains(item.Row))
                    {
                        row.IsSelected = true;
                        row.Focusable = true;
                    }
                } else
                {
                    if (seldr.Contains(item.Row))
                    {
                        dataGrid.ScrollIntoView(item);
                        DataGridRow rw = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                        if ( rw != null)
                        {
                            rw.IsSelected = true;
                            rw.Focusable = true;
                        }
                    }
                }
            }
        }

        public IEnumerable<int> selIndexes { get; set; }

        private IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            bool first = true;
            int selIndex = 0;

            DataGridTemplateColumn colOnline = null;
            if (DgvType == dgvType.variables)
            {
                colOnline = dataGrid.Columns[0] as DataGridTemplateColumn;
                colOnline.CellTemplate = null;
            }

            dataGrid.SelectionChanged -= dataGrid_SelectionChanged;
            if (dataGrid.SelectedItems.Count == 0) goto jump;

            List<DataRowView> items = dataGrid.SelectedItems.OfType<DataRowView>().Cast<DataRowView>().ToList();
            if (items.Count == 0 ) goto jump;

            DataView dv = (DataView)(dataGrid.ItemsSource);
            DataTable dt = dv.Table;
            List<DataRowView> itemsList = items.OrderByDescending(x => dt.Rows.IndexOf(x.Row)).ToList();
          //  int ee = itemsList.Min(x => dt.Rows.IndexOf(x.Row));
            int lowest = dt.Rows.IndexOf(itemsList.First().Row);
            if (lowest == dt.Rows.Count -1 ) goto jump;

            List<DataRow> seldr = new List<DataRow>();
            foreach (DataRowView item in itemsList)
            {
                int index = dt.Rows.IndexOf(item.Row);
                if (first)
                {
                    first = false;
                    selIndex = index + 1;
                }
                DataRow itemTmp = dt.NewRow();
                itemTmp.ItemArray = item.Row.ItemArray;
                dt.Rows.RemoveAt(index);
                dt.Rows.InsertAt(itemTmp, index + 1);

                if  ( index+1 <  dt.Rows.Count)
                seldr.Add(DataSet.Tables[0].Rows[index + 1]);
            }
        
            dataGrid.UpdateLayout();
            updateSelectionDataGrid(seldr);

            jump: 
            // dataGrid.SelectedIndex = selIndex;
            dataGrid.SelectionChanged += dataGrid_SelectionChanged;
            dataGridSelectionChange();
            dataGrid.Focus();

            if (DgvType == dgvType.variables)
                colOnline.CellTemplate = (DataTemplate)this.FindResource("onlineTemplate");
        }



        private void dataGridSelectionChange()
        {
            Console.WriteLine("Datagrid selection changed");
            List<DataRowView> items = dataGrid.SelectedItems.OfType<DataRowView>().Cast<DataRowView>().ToList();
            List<DataRowView> itemsAll = dataGrid.ItemsSource.Cast<DataRowView>().ToList();
            int startIndex = itemsAll.FindIndex(x => x["param"].ToString().Equals("start", StringComparison.InvariantCultureIgnoreCase));
            items.RemoveAll(x => DataSet.Tables[0].Rows.IndexOf(x.Row) <= startIndex);
            selIndexes = (from c in items select DataSet.Tables[0].Rows.IndexOf(c.Row) - startIndex - 1);

              DataHasChanged = true;
            
           foreach (int index in selIndexes)
               Console.WriteLine("--> " + index);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGridSelectionChange();
        }

        private void dataGrid_LayoutUpdated(object sender, EventArgs e)
        {
        

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
         
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (e.Key != Key.Enter) return;

            dataModel dm = this.DataContext as dataModel;

            dm.plc[PlcName].tags[tb.Tag.ToString()].Val = tb.Text;
            Console.WriteLine("Keydown =" + tb.Text);
        }
    }
}
