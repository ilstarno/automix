using System;
using System.Collections.Generic;

namespace Automix_ini.model
{
    public class key : ICloneable, IEnumerable<key>
    {
        #region Initialization

        public key(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
                throw new ArgumentException("key name can not be empty");

            _comments = new List<string>();
            _value = string.Empty;
            _keyName = keyName;
        }

        public key(key ori)
        {
            _value = ori._value;
            _keyName = ori._keyName;
            _comments = new List<string>(ori._comments);
        }

        #endregion Constructors 

        #region Properties 

        public List<string> Comments
        {
            get { return _comments; }
            set { _comments = new List<string>(value); }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string KeyName
        {
            get
            {
                return _keyName;
            }

            set
            {
                if (value != string.Empty)
                    _keyName = value;
            }

        }

        #endregion Properties 

        #region ICloneable Members

        public object Clone()
        {
            return new key(this);
        }

        #endregion

        #region Non-public Members

        private List<string> _comments;

        private string _value;

        private string _keyName;

        #endregion

        public IEnumerator<key> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}