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
using libPLC;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;

namespace ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static Dictionary<string, Page> Pages;
        public static dataModel dm;

        public static Dictionary<string,dragPopup> popups;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dm = new dataModel();
            dm.plc = new Dictionary<string, iPLC>();
            Pages = new Dictionary<string, Page>();
            Pages.Add("auto", new pageAuto());
            Pages.Add("settings", new Page2());
            Pages.Add("plc", new Page3());
            Pages.Add("manual", new pageManual());

            pageAuto pageAuto = Pages["auto"] as pageAuto;
            Page2 pageSettings = Pages["settings"] as Page2;
            Page3 pagePlc = Pages["plc"] as Page3;

            dm.settingsCtrl = pageSettings.settingsC;
            dm.settingsDirCtrl = pageSettings.dirC;
            dm.recipeCtrl = pageAuto.recipeC;
            dm.Touch = true;
            this.DataContext = dm;

            dm.CurPage = "auto";
            frame.Navigate(Pages[dm.CurPage]);

            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string directory = System.IO.Path.GetDirectoryName(filePath);
            dm.setINI = new IniFile(directory+"\\settings.ini");
            string iniPLC = dm.setINI.IniReadValue("setup", "PLC");
            string iniSetup = dm.setINI.IniReadValue("setup", "settings");
            string iniAuto = dm.setINI.IniReadValue("setup", "auto");

            dm.Fullscreen = Convert.ToBoolean(dm.setINI.IniReadValue("setup", "fullscreen"));


            string defaultAuto;
            if (iniAuto != "")
                defaultAuto = pageAuto.dirC.Path + "\\" + iniAuto;
            else
                defaultAuto = pageAuto.dirC.files.First();
            if ( File.Exists(defaultAuto))
                pageAuto.dirC.list.SelectedItem = defaultAuto;


            string defaultSetup;
            if (iniSetup != "")
                defaultSetup = pageSettings.dirC.Path + "\\" + iniSetup;
            else
                defaultSetup = pageSettings.dirC.files.First();

            if (File.Exists(defaultSetup))
                pageSettings.dirC.list.SelectedItem = defaultSetup;

            string defaultPLC;
            if (iniPLC != "")
                defaultPLC = pagePlc.dirC.Path + "\\"+iniPLC;
            else
                defaultPLC = pagePlc.dirC.files.First(); 
            plcSetup.defaultPLC = System.IO.Path.GetFileNameWithoutExtension(defaultPLC);
            pagePlc.dirC.list.SelectedItem = defaultPLC;

            string fileName = "";
            iPLC plcEntry;
            plcEntry = new classPLCTwinCat(true, pagePlc.variablesC.dataGrid);
            fileName = pagePlc.dirC.Path + "\\1.xml";
            plcEntry.createPlcFromFile(fileName);
            param change = new param(4, 100, 0);
            plcEntry.addTag(new classTag<bool>(".TestiPLC", change, "Hopsista", false));
            plcEntry.connect();
            dm.plc.Add(System.IO.Path.GetFileNameWithoutExtension(fileName), plcEntry);

            /*
            plcEntry = new classPLCTwinCat(false, pagePlc.variablesC.dataGrid);
            fileName = pagePlc.dirC.Path + "\\2.xml";
            plcEntry.createPlcFromFile(fileName);
            plcEntry.connect();
            dm.plc.Add(System.IO.Path.GetFileNameWithoutExtension(fileName), plcEntry);*/

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         //   dm.plc["1"].tags["Gopo"].Val = !(bool)dm.plc1.tags["Gopo"].Val;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           // plc.tags["Gopo"].Val = !(bool)plc.tags["Gopo"].Val;
           // Console.WriteLine("--- : " + TagiButtoni.TagIn.Val);
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("ONGELMA");
        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("!!!!");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }

      
    }
}
