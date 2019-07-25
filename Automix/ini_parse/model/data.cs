using System;
using Automix_ini.model.config;
using Automix_ini.model.format;

namespace Automix_ini.model
{
    public class data : ICloneable
    {
        #region Non-Public Members
        private section_collection _sections;
        #endregion

        #region Initialization

        public data()
            : this(new section_collection())
        { }

        public data(section_collection sdc)
        {
            _sections = (section_collection)sdc.Clone();
            Global = new key_collection();
            SectionKeySeparator = '.';
        }

        public data(data ori) : this((section_collection)ori.Sections)
        {
            Global = (key_collection)ori.Global.Clone();
            Configuration = ori.Configuration.Clone();
        }
        #endregion

        #region Properties

        public ini_config Configuration
        {
            get
            {
                if (_configuration == null)
                    _configuration = new ini_config();

                return _configuration;
            }

            set { _configuration = value.Clone(); }
        }

        public key Global { get; protected set; }

        public key_collection this[string sectionName]
        {
            get
            {
                if (!_sections.ContainsSection(sectionName))
                    if (Configuration.AllowCreateSectionsOnFly)
                        _sections.AddSection(sectionName);
                    else
                        return null;

                return _sections[sectionName];
            }
        }

        public section_collection Sections
        {
            get { return _sections; }
            set { _sections = value; }
        }

        public char SectionKeySeparator { get; set; }
        #endregion

        #region Object Methods
        public override string ToString()
        {
            return ToString(new data_format(Configuration));
        }

        public virtual string ToString(IData formatter)
        {
            return formatter.IniDataToString(this);
        }
        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new data(this);
        }

        #endregion

        #region Fields
        private ini_config _configuration;
        #endregion

        public void ClearAllComments()
        {
            Global.ClearComments();

            foreach (var section in Sections)
            {
                section.ClearComments();
            }
        }

        public void Merge(data toMergeIniData)
        {

            if (toMergeIniData == null) return;

            Global.Merge(toMergeIniData.Global);

            Sections.Merge(toMergeIniData.Sections);

        }


        public bool TryGetKey(string key, out string value)
        {
            value = string.Empty;
            if (string.IsNullOrEmpty(key))
                return false;

            var splitKey = key.Split(SectionKeySeparator);
            var separatorCount = splitKey.Length - 1;
            if (separatorCount > 1)
                throw new ArgumentException("key contains multiple separators", "key");

            if (separatorCount == 0)
            {
                if (!Global.ContainsKey(key))
                    return false;

                value = Global[key];
                return true;
            }

            var section = splitKey[0];
            key = splitKey[1];

            if (!_sections.ContainsSection(section))
                return false;
            var sectionData = _sections[section];
            if (!sectionData.ContainsKey(key))
                return false;

            value = sectionData[key];
            return true;
        }


        public string GetKey(string key)
        {
            string result;
            return TryGetKey(key, out result) ? result : null;
        }

        private void MergeSection(section otherSection)
        {
            if (!Sections.ContainsSection(otherSection.SectionName))
            {
                Sections.AddSection(otherSection.SectionName);
            }

            Sections.GetSectionData(otherSection.SectionName).Merge(otherSection);
        }

        private void MergeGlobal(key_collection globals)
        {
            foreach (var globalValue in globals)
            {
                Global[globalValue.KeyName] = globalValue.Value;
            }
        }
    }
}