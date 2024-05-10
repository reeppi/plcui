using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data;
using System.Globalization;
using System.Collections;
using TwinCAT.Ads;
using System.Windows.Controls;
using System.Windows.Data;

namespace libPLC
{
    public interface iTagObj
    {
        object Val { get; set; }
        object MinVal { get; }
        object MaxVal { get; } 
        string Tag { get; set; }
        int Handle { get; set; }
        int notifyHandle { get; set; }
        object Param { get; set; }
        iPLC Plc { set; }
        Type OType { get; set; }
        void readValue();

        string Desc { get; set; }

        string ValStr { get; }
        bool Online { get; set; }
    }


    public class classTag<T> : iTagObj, INotifyPropertyChanged
    {
        T val;
        T minVal { get; set; }
        T maxVal { get; set; }
        public object Param { get; set; }
        public int Handle { get; set; }
        public int notifyHandle { get; set;  }
        public Type OType { get; set; }
        string tag;
        public string Desc { get; set; }
        public bool Online { get; set; }
        public string Tag { get { return tag; } set { tag = value; } }
        public iPLC Plc { get; set; }

        public string ValStr {
            get
            {
                return val.ChangeType<string>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String caller = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller)); }

        public object MinVal { get { return minVal; } }
        public object MaxVal { get { return maxVal; } }

        public object Val
        {
            get {
                return (object)val;
            }
            set
            {
                if (typeof(T) == typeof(string))
                {
                    val = value.ChangeType<T>();
                    Plc.writeSTRING(this, val.ChangeType<string>());
                }
                else if (value.ToString().Is<T>())
                {
                    val = value.ChangeType<T>();
                    if (typeof(T) == typeof(bool))
                        Plc.writeBOOL(this, val.ChangeType<bool>());
                    else if (typeof(T) == typeof(UInt32))
                        Plc.writeUDINT(this, val.ChangeType<UInt32>());
                    else if (typeof(T) == typeof(Int32))
                        Plc.writeDINT(this, val.ChangeType<Int32>());
                    else if (typeof(T) == typeof(double))
                        Plc.writeLREAL(this, val.ChangeType<Double>());
                }

                

                if (Plc.DgVar.IsVisible)
                {
                    /*
                     *   foreach (TextBox textBox in helper.FindVisualChildren<TextBox>(variablesC.dataGrid))
                            {
                            MultiBindingExpression multiBindingExpression = BindingOperations.GetMultiBindingExpression(textBox, TextBox.TextProperty);
                            multiBindingExpression.UpdateTarget();
                        }
                     * */


                    foreach (TextBox textBox in PLChelper.FindVisualChildren<TextBox>(Plc.DgVar))
                    {
                        if (textBox.Tag != null)
                        {
                            if (textBox.Tag.ToString() == tag)
                            {
                                MultiBindingExpression multiBindingExpression = BindingOperations.GetMultiBindingExpression(textBox, TextBox.TextProperty);
                                multiBindingExpression.UpdateTarget();
                            }
                        }
                        
                    }
                }
                OnPropertyChanged("Val");
            }

  
        }

        public void readValue()
        {
            if (typeof(T) == typeof(bool))
                val = Plc.readBOOL(this).ChangeType<T>();
            else if (typeof(T) == typeof(double))
                val = Plc.readLREAL(this).ChangeType<T>();
            else if (typeof(T) == typeof(UInt32))
                val = Plc.readUDINT(this).ChangeType<T>();
            else if (typeof(T) == typeof(Int32))
                val = Plc.readDINT(this).ChangeType<T>();
            else if (typeof(T) == typeof(string))
                val = Plc.readSTRING(this).ChangeType<T>();
            OnPropertyChanged("Val");

            if (Plc.DgVar.IsVisible)
            {
                foreach (TextBox textBox in PLChelper.FindVisualChildren<TextBox>(Plc.DgVar))
                {
                    if (textBox.Tag != null)
                    {
                        if (textBox.Tag.ToString() == tag)
                        {
                            MultiBindingExpression multiBindingExpression = BindingOperations.GetMultiBindingExpression(textBox, TextBox.TextProperty);
                            multiBindingExpression.UpdateTarget();
                        }
                    }

                }
            }
        }

        public classTag(string tag_, object param_, string desc_, bool online_)
        {
            Param = param_;
            Desc = desc_;
            Online = online_;
            tag = tag_;
            if (typeof(T) == typeof(string))
                OType = typeof(bool);
            else 
                OType = typeof(T);
        }

        public classTag(string tag_, object param_, string desc_, bool online_, T minVal_, T maxVal_)
        {
            minVal = minVal_;
            maxVal = maxVal_;
            Param = param_;
            Desc = desc_;
            Online = online_;
            tag = tag_;
            if (typeof(T) == typeof(string))
                OType = typeof(bool);
            else
                OType = typeof(T);
        }


    }



    public static class help
    {
        public static T ChangeType<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static bool Is<T>(this string input)
        {
            var type = typeof(T);
            var temp = default(T);
            var method = type.GetMethod( "TryParse",  new[]{ typeof (string),Type.GetType(string.Format("{0}&", type.FullName))  });
            return (bool)method.Invoke(null, new object[] { input, temp });
        }
    }


}
