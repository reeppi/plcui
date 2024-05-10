using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace libPLC
{
    public class IniFile
    {
        public string path;

        [DllImport("kernel32", EntryPoint = "WritePrivateProfileString")]
        private static extern long WriteProfile(string section, string key, string val, string filePath);

        [DllImport("kernel32", EntryPoint = "WritePrivateProfileStringA")]
        private static extern int deleteKeyValue(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,string key, string def, StringBuilder retVal,int size, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WriteProfile(Section, Key, Value, this.path);
        }

        public void IniDeleteKey(string Section, string Key)
        {
            int del=deleteKeyValue(Section, Key, null, this.path);
            Console.WriteLine("Delete: "+Section+" Key:"+Key);
        }

        public void IniDeleteSection(string Section)
        {
            int del = deleteKeyValue(Section, null, null, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, this.path);
            return temp.ToString();

        }
    }
}