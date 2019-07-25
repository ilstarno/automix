using Automix_ini.model;
using Automix_ini.model.config;
using IniParser.Model.Configuration;

namespace Automix_ini.parser
{

    public class concatenate_key_ini_parse : data_parser
    {
        public new concatenate_double_key Configuration
        {
            get
            {
                return (concatenate_double_key)base.Configuration;
            }
            set
            {
                base.Configuration = (ini_config) value;
            }
        }

        public concatenate_key_ini_parse()
            : this(new concatenate_double_key())
        { }

        public concatenate_key_ini_parse(concatenate_double_key parserConfiguration)
            : base(parserConfiguration)
        { }

        protected override void HandleDuplicatedKeyInCollection(string key, string value, key_collection keyDataCollection, string sectionName)
        {
            keyDataCollection[key] += Configuration.ConcatenateSeparator + value;
        }
    }

}