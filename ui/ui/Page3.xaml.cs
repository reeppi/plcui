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
using libPLC;

namespace ui
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
        }

        private void buttonReloadConnection_Click(object sender, RoutedEventArgs e)
        {
            string plcName = System.IO.Path.GetFileNameWithoutExtension(dirC.SelectedFile);
            dataModel dm = this.DataContext as dataModel;
            dm.plc[plcName].createPlcFromDataGrid(variablesC.dataGrid);
            dm.plc[plcName].connect();

           this.NavigationService.Refresh();            
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox textBox in helper.FindVisualChildren<TextBox>(variablesC.dataGrid))
            {
             MultiBindingExpression multiBindingExpression = BindingOperations.GetMultiBindingExpression(textBox, TextBox.TextProperty);
             multiBindingExpression.UpdateTarget();
            }


        }

        private void buttonAddWatch_Click(object sender, RoutedEventArgs e)
        {
            DataRowView  dr = variablesC.dataGrid.SelectedItem as DataRowView;
            if (dr == null) return;

            string tag = dr["param"].ToString();
            string type = dr["value"].ToString();
            string desc = dr["desc"].ToString();
            string plcName = variablesC.PlcName;
            string title = "";
            string bindStr = "";
            UserControl uc =null;
            if (type == "BOOL")
            {
                bindStr = "plc[" + plcName + "].tags[" + tag + "]";
                uc = new plcButton();
                ((plcButton)uc).CType = plcButton.clickType.toggle;
                Binding newBinding = new Binding(bindStr);
                uc.SetBinding(plcButton.inputProperty, newBinding);
                ((plcButton)uc).Text = tag;
                title = tag;
            } else
            {
                bindStr = "plc[" + plcName + "].tags[" + tag + "].Val";
                uc = new UserControl();
                editTextBox textbox = new editTextBox();
                uc.Content = textbox;
                Binding newBinding = new Binding(bindStr);
                textbox.SetBinding(editTextBox.TextProperty, newBinding);
                title = tag;
            }


            dragPopup popup = new dragPopup();
            popupControl popCtrl = new popupControl(uc);
            popCtrl.title.Text = title;
            popup.DataContext = this.DataContext as dataModel;
            popCtrl.Popup = popup;
            popup.Width = popCtrl.Width;
            popup.Height = popCtrl.Height;
            popup.Child = popCtrl;
            popup.Placement = PlacementMode.MousePoint;
            popup.IsOpen = true;
        }

        private void buttonAddChart_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dr = variablesC.dataGrid.SelectedItem as DataRowView;
            if (dr == null) return;
            string tag = dr["param"].ToString();
            string type = dr["value"].ToString();
            string desc = dr["desc"].ToString();
            string plcName = variablesC.PlcName;
            UserControl uc = null;


            string bindStr = "plc[" + plcName + "].tags[" + tag + "]";
            uc = new plcScope();
            Binding newBinding = new Binding(bindStr);
            uc.SetBinding(plcScope.inputProperty, newBinding);
            uc.Width = 300;
            uc.Height = 150;
            uc.Background = Brushes.White;

            Console.WriteLine("bindStr " + bindStr);
            dragPopup popup = new dragPopup();
            popupControl popCtrl = new popupControl(uc);
            popCtrl.title.Text = tag;
            popup.DataContext = this.DataContext as dataModel;
            popCtrl.Popup = popup;
            popup.Width = popCtrl.Width;
            popup.Height = popCtrl.Height;
            popup.Child = popCtrl;
            popup.Placement = PlacementMode.MousePoint;
            popup.IsOpen = true;
        }


        private void canvasBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("PAGE LOADED");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //  UserControl uC = new UserControl();
            
           
        }


    }
}
