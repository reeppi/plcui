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

namespace libPLC
{
    public class dataModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String caller = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller)); }

        string curPage;
        public string CurPage { get { return curPage; } set { curPage = value; OnPropertyChanged(); } }
        bool touch;
        public bool Touch { get { return touch; } set { touch = value; OnPropertyChanged(); } }

        bool fullscreen;
        public bool Fullscreen
        {
            get { return fullscreen; }
            set
            {
                fullscreen = value;
                OnPropertyChanged();
                setINI.IniWriteValue("setup", "fullscreen", value.ToString());
                if (value == true)
                    WinState = WindowState.Maximized;
                else
                    WinState = WindowState.Normal;
            }
        }
        WindowState winState;
        public WindowState WinState { get { return winState; } set { winState = value; OnPropertyChanged(); } }

        string valuetext;
        public string ValueText { get { return valuetext; } set { valuetext = value; OnPropertyChanged(); } }
        double valueDouble;
        public double ValueDouble { get { return valueDouble; } set { valueDouble = value; OnPropertyChanged(); } }

        public IniFile setINI { get; set; }

        public recipeControl recipeCtrl { get; set; }
        public recipeControl settingsCtrl { get; set; }
        public dirControl settingsDirCtrl { get; set; }
        public Dictionary<string, iPLC> plc { get; set; }
    }
}
