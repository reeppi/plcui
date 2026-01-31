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
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;

namespace libPLC
{

    public static class plcSetup { 
        public static string defaultPLC="";
       // public static string defaultSetup = "";
    }

    public interface iPLC
    {
        void connect();
        void createPlcFromDataGrid(DataGrid datagrid);
        void createPlcFromFile(string plcFile);

        Dictionary<string, iTagObj> tags { get; set; }
        Dictionary<string, iTagObj> tagsParam { get; }
        void addTag(iTagObj tagObj_);

        void writeBOOL(iTagObj tagObj, bool val);
        void writeUDINT(iTagObj tagObj, UInt32 val);
        void writeDINT(iTagObj tagObj, Int32 val);
        void writeLREAL(iTagObj tagObj, Double val);
        void writeSTRING(iTagObj tagObj, string val);
        void writePLCData(plcdata plcData);

        bool readBOOL(iTagObj tagObj);
        UInt32 readUDINT(iTagObj tagObj);
        Int32 readDINT(iTagObj tagObj);
        Double readLREAL(iTagObj tagObj);
        string readSTRING(iTagObj tagObj);

        DataGrid DgVar { get; set; }

    }

    public class param
    {
        public AdsTransMode Mode { get; set; }
        public int CycleTime { get; set; }
        public int MaxDelay { get; set; }

        public param(int mode_, int cycleTime_, int maxDelay_)
        {
            Mode = (AdsTransMode)mode_;
            CycleTime = cycleTime_;
            MaxDelay = maxDelay_;
        }
    }


    public partial class classPLCTwinCat : iPLC
    {
   

        public Dictionary<string, iTagObj> tags { get; set; }
        string Address { get; set; }
        bool Online { get; set; }
        TcAdsClient tcAds { get; set; }
        public DataGrid DgVar { get; set; }


        public Dictionary<string, iTagObj> tagsParam
        {
            get
            {
                Dictionary<string, iTagObj> tagsParam = new Dictionary<string, iTagObj>();
                foreach (KeyValuePair<string, iTagObj> entry in tags)
                {
                   if ( entry.Value.Desc  != "" )
                    {
                        tagsParam.Add(entry.Key,entry.Value);
                    }
                }

                return tagsParam;
            }
        }

        public void addTag(iTagObj tagObj_)
        {
            tagObj_.Plc = this;
            tags.Add(tagObj_.Tag, tagObj_);
        }

        public classPLCTwinCat(bool online_, DataGrid dgVar_)
        {
            Online = online_;
            DgVar = dgVar_;
            tags = new Dictionary<string, iTagObj>();
        }

        public classPLCTwinCat(string address_, bool online_)
        {
            Address = address_;
            Online = online_;
            
            tags = new Dictionary<string, iTagObj>();
        }

        public void connect()
        {
            if (tcAds != null)
                tcAds.Dispose();

            if (Online)
            {
                tcAds = new TcAdsClient();
                Console.WriteLine("Connecting twincat");
                try
                {
                    tcAds.Connect(Address, 801);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PLC Connect Error:" + ex.Message);
                    Online = false;
                    return;
                }
            }
            
            if (!Online ) return;
            Console.WriteLine("Connected to PLC " + Address);
            AddAdsNotifications();
        }

    
        private void RemoveAdsNotifications()
        {
            foreach (KeyValuePair<string, iTagObj> entry in tags)
            {
                if (entry.Value.Online)
                {
                    param par = (param)entry.Value.Param;
                    tcAds.DeleteVariableHandle(entry.Value.Handle);
                    if (par != null)
                    {
                        tcAds.DeleteDeviceNotification(entry.Value.notifyHandle);
                    }
                }
            }
        }

        private void AddAdsNotifications()
        {
            List<string> errorTags = new List<string>();
            string errMsg = "";
            tcAds.AdsNotificationEx += new AdsNotificationExEventHandler(tcAds_notification);
            foreach (KeyValuePair<string, iTagObj> entry in tags)
            {
                try
                {
                    //   Console.WriteLine("AdsNotify tag (" + entry.Key +")");
                    param par = (param)entry.Value.Param;

                    if (entry.Value.Online)
                    {
                        entry.Value.Handle = tcAds.CreateVariableHandle(entry.Key);
                        if (par != null)
                            tcAds.AddDeviceNotificationEx(entry.Key, par.Mode, par.CycleTime, par.MaxDelay, entry.Key, entry.Value.OType);
                    }
                }
                catch (Exception err)
                {
                    errMsg = err.Message;
                    errorTags.Add(entry.Key);
                    break;
                }
            }

            if (errMsg != "")
            {
                string sTags = string.Join(Environment.NewLine, errorTags);
                MessageBox.Show("addAdsError : " + errMsg + Environment.NewLine+sTags);
            }

        }

        private void tcAds_notification(object sender, AdsNotificationExEventArgs e)
        {
            string eTagName = e.UserData.ToString();
            if (tags.ContainsKey(eTagName))
            {
                if (eTagName != ".bUpdateStr")
                {
                    tags[eTagName].readValue();
                 //   Console.WriteLine("ads notify " + eTagName);
                }
                else
                    updateSTRINGS(tags[eTagName]);
            }
        }

        private void updateSTRINGS(iTagObj tagObj)
        {
            bool b =readBOOL(tagObj);
            if (!b)
            {
                foreach (KeyValuePair<string, iTagObj> entry in tags)
                {
                    if (entry.Value.GetType() == typeof(classTag<string>))
                        if ( entry.Value.Online)
                        entry.Value.readValue();
                }
                writeBOOL(tagObj, false);
            }
        }

        public bool readBOOL(iTagObj tagObj)
        {
            bool b = (bool)tcAds.ReadAny(tagObj.Handle, typeof(bool));
            return b;
        }

        public UInt32 readUDINT(iTagObj tagObj)
        {
            UInt32 b = (UInt32)tcAds.ReadAny(tagObj.Handle, typeof(UInt32));
            return b;
        }

        public Int32 readDINT(iTagObj tagObj)
        {
            Int32 b = (Int32)tcAds.ReadAny(tagObj.Handle, typeof(Int32));
            return b;
        }


        public Double readLREAL(iTagObj tagObj)
        {
            Double b = (Double)tcAds.ReadAny(tagObj.Handle, typeof(Double));
            return b;
        }

        public string readSTRING(iTagObj tagObj)
        {
            int[] numbers = new int[] { 100 };
            string b = (string)tcAds.ReadAny(tagObj.Handle, typeof(string), numbers);
            return b;
        }

        public void writeBOOL(iTagObj tag, bool val)
        {
            writeAny(tag, val);
        }

        public void writeUDINT(iTagObj tag, UInt32 val)
        {
            writeAny(tag, val);
        }

        public void writeDINT(iTagObj tag, Int32 val)
        {
            writeAny(tag, val);
        }

        public void writeLREAL(iTagObj tag, Double val)
        {
            writeAny(tag, val);
        }

        public void  writeSTRING(iTagObj tag, string val)
        {
            if (Online && tag.Online)
            {
                Console.WriteLine("Writing to PLC (" + tag.Tag + ") " + val);
                int[] numbers = new int[] { 100 };
                tcAds.WriteAny(tag.Handle, val, numbers);
            } else
            {
                Console.WriteLine("Writing to OFFLINE PLC (" + tag.Tag + ") " + val);
            }

        }

        private void writeAny(iTagObj tag, object val)
        {
            if (Online && tag.Online)
            {
                Console.WriteLine("Writing to PLC (" + tag.Tag + ") " + val);
                tcAds.WriteAny(tag.Handle, val);
            }
            else
            {
                Console.WriteLine("Writing to OFFLINE PLC (" + tag.Tag + ") " + val);
            }
        }

        public void writePLCData(plcdata plcData)
        {
          
            Console.WriteLine("Send data to plc");
            foreach ( paramEntry parEntry in plcData.ParList )
            {
                if (  parEntry.Plc == Name )
                {
                    if (tags.ContainsKey(parEntry.Param))
                        tags[parEntry.Param].Val = parEntry.Value;
                }
            }

            if (plcData.DataPlc != Name) return;

            Console.WriteLine("Send binary data to plc " + Name);

            if (!Online) return;
    
            int dataSize = sizeof(Int16) + sizeof(double);
            int hPLCdata = tcAds.CreateVariableHandle(".workList");
            AdsStream dataStream = new AdsStream(dataSize * plcData.data.Count + sizeof(Int16));

            BinaryWriter binWrite = new BinaryWriter(dataStream);
            binWrite.Write(Convert.ToInt16(plcData.data.Count));

            foreach (plcDataEntry dataEntry in plcData.data)
            {
                binWrite.Write(dataEntry.pos);
                binWrite.Write(Convert.ToInt16(0));
                Console.WriteLine("binWrite " + dataEntry.pos);
            }
            try
            {
                tcAds.Write(hPLCdata, dataStream);
            }
            catch (Exception err)
            {
                MessageBox.Show("SendDataToPLC-error : " + err.Message);
            }
            binWrite.Close();
            dataStream.Close();

        }



    }
    







}
