using System;
using System.Collections;
using System.Collections.Generic;

namespace Automix_ini.model
{
    public class key_collection : ICloneable, IEnumerable<key>
    {
        IEqualityComparer<string> _searchComparer;
        #region Initialization

        public key_collection()
            : this(EqualityComparer<string>.Default)
        { }

        public key_collection(IEqualityComparer<string> searchComparer)
        {
            _searchComparer = searchComparer;
            _keyData = new Dictionary<string, key>(_searchComparer);
        }

        public key_collection(key_collection ori, IEqualityComparer<string> searchComparer)
            : this(searchComparer)
        {
            foreach (key key in ori)
            {
                if (_keyData.ContainsKey(key.KeyName))
                {
                    _keyData[key.KeyName] = (key)key.Clone();
                }
                else
                {
                    _keyData.Add(key.KeyName, (key)key.Clone());
                }
            }
        }

        #endregion

        #region Properties

        public string this[string keyName]
        {
            get
            {
                if (_keyData.ContainsKey(keyName))
                    return _keyData[keyName].Value;

                return null;
            }

            set
            {
                if (!_keyData.ContainsKey(keyName))
                {
                    this.AddKey(keyName);
                }

                _keyData[keyName].Value = value;

            }
        }

        public int Count
        {
            get { return _keyData.Count; }
        }

        #endregion

        #region Operations

        public bool AddKey(string keyName)
        {
            if (!_keyData.ContainsKey(keyName))
            {
                _keyData.Add(keyName, new key(keyName));
                return true;
            }

            return false;
        }

        [Obsolete("Pottentially buggy method! Use AddKey(KeyData keyData) instead (See comments in code for an explanation of the bug)")]
        public bool AddKey(string keyName, key keyData)
        {
            if (AddKey(keyName))
            {
                _keyData[keyName] = keyData;
                return true;
            }

            return false;

        }

        public bool AddKey(key keyData)
        {
            if (AddKey(keyData.KeyName))
            {
                _keyData[keyData.KeyName] = keyData;
                return true;
            }

            return false;
        }
        public bool AddKey(string keyName, string keyValue)
        {
            if (AddKey(keyName))
            {
                _keyData[keyName].Value = keyValue;
                return true;
            }

            return false;

        }

        public void ClearComments()
        {
            foreach (var keydata in this)
            {
                keydata.Comments.Clear();
            }
        }

        public bool ContainsKey(string keyName)
        {
            return _keyData.ContainsKey(keyName);
        }

        public key GetKeyData(string keyName)
        {
            if (_keyData.ContainsKey(keyName))
                return _keyData[keyName];
            return null;
        }

        public void Merge(key_collection keyDataToMerge)
        {
            foreach (var keyData in keyDataToMerge)
            {
                AddKey(keyData.KeyName);
                GetKeyData(keyData.KeyName).Comments.AddRange(keyData.Comments);
                this[keyData.KeyName] = keyData.Value;
            }

        }

        public void RemoveAllKeys()
        {
            _keyData.Clear();
        }

  
        public bool RemoveKey(string keyName)
        {
            return _keyData.Remove(keyName);
        }
        public void SetKeyData(key data)
        {
            if (data == null) return;

            if (_keyData.ContainsKey(data.KeyName))
                RemoveKey(data.KeyName);

            AddKey(data);
        }

        #endregion

        #region IEnumerable<KeyData> Members

        public IEnumerator<key> GetEnumerator()
        {
            foreach (string key in _keyData.Keys)
                yield return _keyData[key];
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _keyData.GetEnumerator();
        }

        #endregion

        #endregion

        #region ICloneable Members


        public object Clone()
        {
            return new key_collection(this, _searchComparer);
        }

        #endregion

        #region Non-public Members
        internal key GetLast()
        {
            key result = null;
            if (_keyData.Keys.Count <= 0) return result;


            foreach (var k in _keyData.Keys) result = _keyData[k];
            return result;
        }

        private readonly Dictionary<string, key> _keyData;

        #endregion

    }
}