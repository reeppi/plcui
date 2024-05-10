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
using System.Data;
using System.Windows.Controls.Primitives;
using libPLC;


namespace ui
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class pageAuto : Page
    {
        public pageAuto()
        {
            InitializeComponent();
            KeepAlive = true;
        }

        private void buttonSendData_Click(object sender, RoutedEventArgs e)
        {
            dataModel dm = this.DataContext as dataModel;
            plcdata setupData = null;
            plcdata workData = null;
            if ((bool)checkSettings.IsChecked)
            {
                setupData = new plcdata(dm.settingsCtrl.dataGrid, null);
                workData = new plcdata(recipeC.dataGrid, setupData.ParList);
            } else
            {
                workData = new plcdata(recipeC.dataGrid, null);
            }
            dm.plc["1"].writePLCData(workData);
            //dm.plc["2"].writePLCData(workData);

            dm.plc["1"].tags[".sProgram"].Val = dirC.SelectedFileName;
            Console.WriteLine(":"+dirC.SelectedFileName);
        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            double x=Convert.ToDouble(MainWindow.dm.plc["1"].tags[".lVar1"].Val);
            x +=20;
            MainWindow.dm.plc["1"].tags[".lVar1"].Val = (object)x;

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            
        }
    }
}
