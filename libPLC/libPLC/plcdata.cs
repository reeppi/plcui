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


namespace libPLC
{

    public class plcDataEntry
    {
        public double pos { get; set; }
        public double rel { get; set; }

        public int index { get; set; }
        public double textWidth { get; set; }
        public int textLevel { get; set; }
        public TextBlock textblock { get; set; }
    }

    public class paramEntry
    {
        public string Param { get; set; }
        public string Value { get; set; }
        public string Plc { get; set; }
    }

    public class plcdata
    {

        public List<paramEntry> ParList { get; set; }
        public List<plcDataEntry> data { get; set; }
        public string DataPlc { get; set; }

        public plcdata(DataGrid datagrid, List<paramEntry> setupList)
        {
            data = new List<plcDataEntry>();

            if (setupList == null)
                ParList = new List<paramEntry>();
            else
            {
                ParList = setupList;
            }
           
          

            DataPlc = "";

            if (datagrid.ItemsSource == null) return;

            bool bData = false;
            double curPos = 0;
            int index = 0;
            foreach (DataRowView dr in datagrid.ItemsSource)
            {
                string param = dr.Row["param"].ToString();
                bool isStart = param.Equals("start", StringComparison.InvariantCultureIgnoreCase);
                if (isStart)
                {
                    bData = true;

                    
                    if (dr.Row["plc"].ToString() == "")
                        DataPlc = plcSetup.defaultPLC;
                    else
                        DataPlc = dr.Row["plc"].ToString();

                }
                if (!bData)
                {
                    paramEntry parEntry = new paramEntry();
                    parEntry.Param = param;
                    parEntry.Value = dr.Row["value"].ToString();

                    if (dr.Row["plc"].ToString() == "")
                        parEntry.Plc = plcSetup.defaultPLC;
                    else
                        parEntry.Plc = dr.Row["plc"].ToString();

                    paramEntry paramP = null;
                    paramP = ParList.Find(x => x.Param.Equals(parEntry.Param) && x.Plc.Equals(parEntry.Plc));
                    if (paramP == null)
                    {
                        //Console.WriteLine("Lisätään parametri " + parEntry.Param);
                        ParList.Add(parEntry);
                    }
                    else
                    {
                       // Console.WriteLine("Ajetaan yli parametri " + paramP.Param + " arvolla " + paramP.Value);
                        paramP.Value = parEntry.Value;
                    }

                }
                else
                {
                    if (!isStart)
                    {
                        double sV = 0;
                      //  string sT = dr.Row["param"].ToString();
                        if (dr.Row["value"].ToString() == "") continue;
                        double.TryParse(dr.Row["value"].ToString(), out sV);
                        int count = 1;
                        if (param.Count() > 1)
                        {
                            if (param[0] == 'x' || param[0] == 'X')
                            {
                                string ct = param.Remove(0, 1);
                                Int32.TryParse(ct, out count);
                            }
                        }
                        if (param.Count() == 1)
                        {
                            if (param[0] == 'a' || param[0] == 'A')
                                curPos = 0;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            plcDataEntry plcD = new plcDataEntry();
                            plcD.pos = curPos + sV;
                            plcD.index = index;
                            data.Add(plcD);
                            curPos += sV;
                        }
                        index++;
                    }
                }
            }

            data.Sort(delegate (plcDataEntry p1, plcDataEntry p2) { return p1.pos.CompareTo(p2.pos); });

            for (int i = 0; i < data.Count; i++)
            {
                plcDataEntry plcEntry = data[i];
                if (i >= 1)
                {
                    plcDataEntry plcEntryPrev = data[i - 1];
                    plcEntry.rel = plcEntry.pos - plcEntryPrev.pos;
                }
                else
                {
                    plcEntry.rel = plcEntry.pos;
                }
            }



        }
    }
}
