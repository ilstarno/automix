using System;
using System.Collections.Generic;

namespace Automix_ini.model
{
    public class section : ICloneable
    {
        IEqualityComparer<string> _searchComparer;
        #region Initialization

        public section(string sectionName)
            : this(sectionName, EqualityComparer<string>.Default)
        {

        }
        public section(string sectionName, IEqualityComparer<string> searchComparer)
        {
            _searchComparer = searchComparer;

            if (string.IsNullOrEmpty(sectionName))
                throw new ArgumentException("section name can not be empty");

            _leadingComments = new List<string>();
            _keyDataCollection = new key_collection(_searchComparer);
            SectionName = sectionName;
        }


   
        public section(section ori, IEqualityComparer<string> searchComparer = null)
        {
            SectionName = ori.SectionName;

            _searchComparer = searchComparer;
            _leadingComments = new List<string>(ori._leadingComments);
            _keyDataCollection = new key_collection(ori._keyDataCollection, searchComparer ?? ori._searchComparer);
        }

        #endregion

        #region Operations

        public void ClearComments()
        {
            LeadingComments.Clear();
            TrailingComments.Clear();
            Keys.ClearComments();
        }

		public void ClearKeyData()
        {
            Keys.RemoveAllKeys();
        }

     
        public void Merge(section toMergeSection)
        {
            foreach (var comment in toMergeSection.LeadingComments)
                LeadingComments.Add(comment);

            Keys.Merge(toMergeSection.Keys);

            foreach (var comment in toMergeSection.TrailingComments)
                TrailingComments.Add(comment);
        }

        #endregion

        #region Properties

        
        public string SectionName
        {
            get
            {
                return _sectionName;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                    _sectionName = value;
            }
        }


        [Obsolete("Do not use this property, use property Comments instead")]
        public List<string> LeadingComments
        {
            get
            {
                return _leadingComments;
            }

            internal set
            {
                _leadingComments = new List<string>(value);
            }
        }

        public List<string> Comments
        {
            get
            {
                return _leadingComments;
            }


        }

        [Obsolete("Do not use this property, use property Comments instead")]
        public List<string> TrailingComments
        {
            get
            {
                return _trailingComments;
            }

            internal set
            {
                _trailingComments = new List<string>(value);
            }
        }
        public key Keys
        {
            get
            {
                return _keyDataCollection;
            }

            set
            {
                _keyDataCollection = value;
            }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new section(this);
        }

        #endregion

        #region Non-public members

        private List<string> _leadingComments;
        private List<string> _trailingComments = new List<string>();

        private key_collection _keyDataCollection;

        private string _sectionName;
        #endregion



    }
}