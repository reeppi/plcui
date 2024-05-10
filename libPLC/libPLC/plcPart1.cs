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
    public partial class classPLCTwinCat : iPLC
    {
        public string Name { get; set;  }

        public void createPlcFromFile(string fullPath)
        {
            tags.Clear();

            DataSet ds = new DataSet();
            ds.ReadXml(fullPath);
            DataTable dt = ds.Tables[0];
            readPlcFromDataTable(dt);
            Name = System.IO.Path.GetFileNameWithoutExtension(fullPath);
            ds.Dispose();
        }

        public void createPlcFromDataGrid(DataGrid datagrid)
        {
            if (datagrid.ItemsSource == null) return;
            tags.Clear();
            Console.WriteLine("Creating plc from datagrid ");
            DataView dv = (DataView)(datagrid.ItemsSource);
            DataTable dt = dv.Table;
            //  param change = new param(4, 100, 0);
            readPlcFromDataTable(dt);
            Console.WriteLine("Address  : " + Address);
        }

        private void readPlcFromDataTable(DataTable dt)
        {
            param change = new param(4, 100, 0);
            param cyclic = new param(3, 1000, 0);

            foreach (DataRow dr in dt.Rows)
            {
                string strMaxVal = "0";
                string strMinVal = "0";
                string param = dr["param"].ToString();
                if (param == "") continue;
                string value = dr["value"].ToString();
                string mode = dr["mode"].ToString();
                string desc = dr["desc"].ToString();
                bool plc = false;


                if (dr.Table.Columns.Contains("min"))
                { 
                    strMinVal = dr["min"].ToString();
                    if (strMinVal == "")
                        strMinVal = "0";
                }

                if (dr.Table.Columns.Contains("max"))
                {
                    strMaxVal = dr["max"].ToString();
                    if (strMaxVal == "")
                        strMaxVal = "0" ;
                }
                else
                    strMaxVal = "0";
               
             //   Console.WriteLine("min : " + strMinVal + " - max " + strMaxVal);


                Boolean.TryParse(dr["plc"].ToString(), out plc);
                param parSetup = null;
                if (mode == "change")
                    parSetup = change;
                if (mode == "cyclic")
                    parSetup = cyclic;
                if (param == "ADDRESS")
                    Address = desc;
                else
                {
                    switch (dr["value"].ToString())
                    {
                        case "BOOL": addTag(new classTag<Boolean>(param, parSetup, desc, plc)); break;
                        case "LREAL": addTag(new classTag<double>(param, parSetup, desc, plc, strMinVal.ChangeType<double>(), strMaxVal.ChangeType<double>())); break;
                        case "UDINT": addTag(new classTag<UInt32>(param, parSetup, desc, plc, strMinVal.ChangeType<UInt32>(), strMaxVal.ChangeType<UInt32>())); break;
                        case "DINT": addTag(new classTag<Int32>(param, parSetup, desc, plc, strMinVal.ChangeType<Int32>(), strMaxVal.ChangeType<Int32>())); break;
                        case "STRING": addTag(new classTag<string>(param, parSetup, desc, plc)); break;
                    }
                }
            }
        }

    

  
    }
    /*
    static class DataRowExtensions
    {
        public static object GetValue(this DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) ? row[column] : null;
        }
    }*/
}
