using System;
using System.Text.RegularExpressions;
using Automix_ini.model.config;
using Automix_ini.parser;
namespace IniParser.Model.Configuration
{
    public class concatenate_double_key : ini_configs
    {
        public new bool AllowDuplicateKeys { get { return true; } }
        public concatenate_double_key()
            : base()
        {
            this.ConcatenateSeparator = ";";
        }

        public concatenate_double_key(concatenate_double_key ori)
            : base(ori)
        {
            this.ConcatenateSeparator = ori.ConcatenateSeparator;
        }

        public string ConcatenateSeparator { get; set; }
    }

}