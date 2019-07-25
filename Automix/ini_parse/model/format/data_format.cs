using System;
using System.Collections.Generic;
using System.Text;
using Automix_ini.model.config;

namespace Automix_ini.model.format
{

    public class data_format : IData
    {
        ini_config _configuration;

        #region Initialization
        public data_format() : this(new ini_config()) { }

        public data_format(ini_config configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");
            this.Configuration = configuration;
        }
        #endregion

        public virtual string IniDataToString(data iniData)
        {
            var sb = new StringBuilder();

            if (Configuration.AllowKeysWithoutSection)
            {
                WriteKeyValueData(iniData.Global, sb);
            }

            foreach (section section in iniData.Sections)
            {
                WriteSection(section, sb);
            }

            return sb.ToString();
        }

       
        public ini_config Configuration
        {
            get { return _configuration; }
            set { _configuration = value.Clone(); }
        }

        #region Helpers

        private void WriteSection(section section, StringBuilder sb)
        {
            if (sb.Length > 0) sb.Append(Configuration.NewLineStr);

            WriteComments(section.LeadingComments, sb);

            sb.Append(string.Format("{0}{1}{2}{3}",
                Configuration.SectionStartChar,
                section.SectionName,
                Configuration.SectionEndChar,
                Configuration.NewLineStr));

            WriteKeyValueData(section.Keys, sb);

            WriteComments(section.TrailingComments, sb);
        }

        private void WriteKeyValueData(key keyDataCollection, StringBuilder sb)
        {

            foreach (key keyData in keyDataCollection)
            {
                if (keyData.Comments.Count > 0) sb.Append(Configuration.NewLineStr);

                WriteComments(keyData.Comments, sb);

                sb.Append(string.Format("{0}{3}{1}{3}{2}{4}",
                    keyData.KeyName,
                    Configuration.KeyValueAssigmentChar,
                    keyData.Value,
                    Configuration.AssigmentSpacer,
                    Configuration.NewLineStr));
            }
        }

        private void WriteComments(List<string> comments, StringBuilder sb)
        {
            foreach (string comment in comments)
                sb.Append(string.Format("{0}{1}{2}", Configuration.CommentString, comment, Configuration.NewLineStr));
        }
        #endregion

    }

}