using System;
using System.Collections;
using System.Collections.Generic;

namespace Automix_ini.model
{
    public class section_collection : ICloneable, IEnumerable<section>
    {
        IEqualityComparer<string> _searchComparer;
        #region Initialization

        public section_collection()
            : this(EqualityComparer<string>.Default)
        { }

        public section_collection(IEqualityComparer<string> searchComparer)
        {
            _searchComparer = searchComparer;

            _sectionData = new Dictionary<string, section>(_searchComparer);
        }

        public section_collection(section_collection ori, IEqualityComparer<string> searchComparer)
        {
            _searchComparer = searchComparer ?? EqualityComparer<string>.Default;

            _sectionData = new Dictionary<string, section>(_searchComparer);
            foreach (var sectionData in ori)
            {
                _sectionData.Add(sectionData.SectionName, (section)sectionData.Clone());
            };
        }

        #endregion

        #region Properties

        public key_collection this[string sectionName]
        {
            get
            {
                if (_sectionData.ContainsKey(sectionName))
                    return _sectionData[sectionName].Keys;

                return null;
            }
        }

        #endregion

        #region Public Members

       
        public bool AddSection(string keyName)
        {
            if (!ContainsSection(keyName))
            {
                _sectionData.Add(keyName, new section(keyName, _searchComparer));
                return true;
            }

            return false;
        }

        public void Add(section data)
        {
            if (ContainsSection(data.SectionName))
            {
                SetSectionData(data.SectionName, new section(data, _searchComparer));
            }
            else
            {
                _sectionData.Add(data.SectionName, new section(data, _searchComparer));
            }
        }
        public void Clear()
        {
            _sectionData.Clear();
        }


 
        public bool ContainsSection(string keyName)
        {
            return _sectionData.ContainsKey(keyName);
        }

  
        public section GetSectionData(string sectionName)
        {
            if (_sectionData.ContainsKey(sectionName))
                return _sectionData[sectionName];

            return null;
        }

        public void Merge(section_collection sectionsToMerge)
        {
            foreach (var sectionDataToMerge in sectionsToMerge)
            {
                var sectionDataInThis = GetSectionData(sectionDataToMerge.SectionName);

                if (sectionDataInThis == null)
                {
                    AddSection(sectionDataToMerge.SectionName);
                }

                this[sectionDataToMerge.SectionName].Merge(sectionDataToMerge.Keys);
            }
        }

    
        public void SetSectionData(string sectionName, section data)
        {
            if (data != null)
                _sectionData[sectionName] = data;
        }

  
        public bool RemoveSection(string keyName)
        {
            return _sectionData.Remove(keyName);
        }


        #endregion

        #region IEnumerable<SectionData> Members

        public IEnumerator<section> GetEnumerator()
        {
            foreach (string sectionName in _sectionData.Keys)
                yield return _sectionData[sectionName];
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new section_collection(this, _searchComparer);
        }

        #endregion

        #region Non-public Members

     
        private readonly Dictionary<string, section> _sectionData;

        #endregion

    }
}