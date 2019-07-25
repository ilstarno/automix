using System;
using System.IO;
using Automix_ini.model;
using Automix_ini.model.format;
using Automix_ini.parser;

namespace Automix_ini
{

    public class stream_data_parser
    {
        public data_parser Parser { get; protected set; }

        public stream_data_parser() : this(new data_parser()) { }

        public stream_data_parser(data_parser parser)
        {
            Parser = parser;
        }
        #region Public Methods

        public data_parser ReadData(StreamReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            return Parser.Parse(reader.ReadToEnd());
        }

        public void WriteData(StreamWriter writer, data iniData)
        {
            if (iniData == null)
                throw new ArgumentNullException("iniData");
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.Write(iniData.ToString());
        }


        public void WriteData(StreamWriter writer, data iniData, IData formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException("formatter");
            if (iniData == null)
                throw new ArgumentNullException("iniData");
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.Write(iniData.ToString(formatter));
        }

        #endregion
    }
}