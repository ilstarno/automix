using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using  System.Text;

namespace Automix.helpers.ini
{
    /// <summary>
    /// class needed for reading and writting ini files
    /// could give some unicode problems on linux/unix systems
    /// </summary>
    class ini
    {
        #region declarations
        public string Path;
        public string automix_executable = Assembly.GetExecutingAssembly().GetName().Name;
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
        #endregion

        #region methods
        /// <summary>
        /// public ini
        /// </summary>
        /// <param name="ini_path"></param>
        public ini(string ini_path = null)
        {
            Path = new FileInfo(ini_path ?? automix_executable + ".ini").FullName.ToString();
        }

        /// <summary>
        /// read method
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        /// <returns></returns>
        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? automix_executable, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        /// <summary>
        /// write method
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="Section"></param>
        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? automix_executable, Key, Value, Path);
        }

        /// <summary>
        /// deletes an ini key 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? automix_executable);
        }

        /// <summary>
        /// deletes an ini section
        /// </summary>
        /// <param name="Section"></param>
        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? automix_executable);
        }

        /// <summary>
        /// method checks if key exists
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        /// <returns></returns>
        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
        #endregion
    }
}
