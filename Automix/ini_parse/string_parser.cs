using System;
using Automix_ini.model;
using Automix_ini.parser;

namespace Automix_ini
{

    [Obsolete("Use class data_parser instead. See remarks comments in this class.")]
    public class string_parser
    {
      
        public data_parser Parser { get; protected set; }

        public string_parser() : this(new data_parser()) { }

        
        public string_parser(data_parser parser)
        {
            Parser = parser;
        }


        public data_parser ParseString(string dataStr)
        {
            return Parser.Parse(dataStr);
        }

    
        public string WriteString(data iniData)
        {
            return iniData.ToString();
        }
    }
}