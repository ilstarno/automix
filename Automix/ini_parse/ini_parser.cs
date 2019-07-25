using System;
using System.IO;
using System.Text;
using Automix_ini.exceptions;
using Automix_ini.model;
using Automix_ini.parser;

namespace Automix_ini
{
    public class FileIniDataParser : stream_data_parser
    {
        public FileIniDataParser() { }

        public FileIniDataParser(data_parser parser) : base(parser)
        {
            Parser = parser;
        }

        #region Deprecated methods

        [Obsolete("Please use ReadFile method instead of this one as is more semantically accurate")]
        public data_parser LoadFile(string filePath)
        {
            return ReadFile(filePath);
        }

        [Obsolete("Please use ReadFile method instead of this one as is more semantically accurate")]
        public data_parser LoadFile(string filePath, Encoding fileEncoding)
        {
            return ReadFile(filePath, fileEncoding);
        }
        #endregion

        public data_parser ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.ASCII);
        }

        public data_parser ReadFile(string filePath, Encoding fileEncoding)
        {
            if (filePath == string.Empty)
                throw new ArgumentException("Bad filename.");

            try
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs, fileEncoding))
                    {
                        return ReadData(sr);
                    }
                }
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ParsingException(String.Format("Could not parse file {0}", filePath), ex);
            }

        }

        [Obsolete("Please use WriteFile method instead of this one as is more semantically accurate")]
        public void SaveFile(string filePath, data parsedData)
        {
            WriteFile(filePath, parsedData, Encoding.UTF8);
        }

        public void WriteFile(string filePath, data parsedData, Encoding fileEncoding = null)
        {
            if (fileEncoding == null)
                fileEncoding = Encoding.UTF8;

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Bad filename.");

            if (parsedData == null)
                throw new ArgumentNullException("parsedData");

            using (FileStream fs = File.Open(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sr = new StreamWriter(fs, fileEncoding))
                {
                    WriteData(sr, parsedData);
                }
            }
        }
    }
}