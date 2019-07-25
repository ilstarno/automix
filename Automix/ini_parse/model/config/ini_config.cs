using System;
using System.Text.RegularExpressions;
using IniParser.Model.Configuration;

namespace Automix_ini.model.config
{
    [Obsolete("Kept for backward compatibility, just use IniParserConfiguration class")]
    public class ini_config : concatenate_double_key { }

    public class ini_configs : ICloneable
    {
        #region Initialization
       
        public ini_configs()
        {
            CommentString = ";";
            SectionStartChar = '[';
            SectionEndChar = ']';
            KeyValueAssigmentChar = '=';
            AssigmentSpacer = " ";
            NewLineStr = Environment.NewLine;
            ConcatenateDuplicateKeys = false;
            AllowKeysWithoutSection = true;
            AllowDuplicateKeys = false;
            AllowDuplicateSections = false;
            AllowCreateSectionsOnFly = true;
            ThrowExceptionsOnError = true;
            SkipInvalidLines = false;
        }

      
        public ini_configs(ini_configs ori)
        {
            AllowDuplicateKeys = ori.AllowDuplicateKeys;
            OverrideDuplicateKeys = ori.OverrideDuplicateKeys;
            AllowDuplicateSections = ori.AllowDuplicateSections;
            AllowKeysWithoutSection = ori.AllowKeysWithoutSection;
            AllowCreateSectionsOnFly = ori.AllowCreateSectionsOnFly;

            SectionStartChar = ori.SectionStartChar;
            SectionEndChar = ori.SectionEndChar;
            CommentString = ori.CommentString;
            ThrowExceptionsOnError = ori.ThrowExceptionsOnError;

        }
        #endregion

        #region IniParserConfiguration

        public Regex CommentRegex { get; set; }

        public Regex SectionRegex { get; set; }

   
        public char SectionStartChar
        {
            get { return _sectionStartChar; }
            set
            {
                _sectionStartChar = value;
                RecreateSectionRegex(_sectionStartChar);
            }
        }

        public char SectionEndChar
        {
            get { return _sectionEndChar; }
            set
            {
                _sectionEndChar = value;
                RecreateSectionRegex(_sectionEndChar);
            }
        }

       
        public bool CaseInsensitive { get; set; }

        [Obsolete("Please use the CommentString property")]
        public char CommentChar
        {
            get { return CommentString[0]; }
            set { CommentString = value.ToString(); }
        }

     
        public string CommentString
        {

            get { return _commentString ?? string.Empty; }
            set
            {
                foreach (var specialChar in _strSpecialRegexChars)
                {
                    value = value.Replace(new String(specialChar, 1), @"\" + specialChar);
                }

                CommentRegex = new Regex(string.Format(_strCommentRegex, value));
                _commentString = value;
            }
        }

        public string NewLineStr
        {
            get; set;
        }
       
        public char KeyValueAssigmentChar { get; set; }

      
        public string AssigmentSpacer { get; set; }

       
        public bool AllowKeysWithoutSection { get; set; }

      
        public bool AllowDuplicateKeys { get; set; }

       
        public bool OverrideDuplicateKeys { get; set; }

       
        public bool ConcatenateDuplicateKeys { get; set; }

      
        public bool ThrowExceptionsOnError { get; set; }

       
        public bool AllowDuplicateSections { get; set; }

        public bool AllowCreateSectionsOnFly { get; set; }

        public bool SkipInvalidLines { get; set; }

        #endregion

        #region Fields
        private char _sectionStartChar;
        private char _sectionEndChar;
        private string _commentString;
        #endregion

        #region Constants
        protected const string _strCommentRegex = @"^{0}(.*)";
        protected const string _strSectionRegexStart = @"^(\s*?)";
        protected const string _strSectionRegexMiddle = @"{1}\s*[\p{L}\p{P}\p{M}_\""\'\{\}\#\+\;\*\%\(\)\=\?\&\$\,\:\/\.\-\w\d\s\\\~]+\s*";
        protected const string _strSectionRegexEnd = @"(\s*?)$";
        protected const string _strKeyRegex = @"^(\s*[_\.\d\w]*\s*)";
        protected const string _strValueRegex = @"([\s\d\w\W\.]*)$";
        protected const string _strSpecialRegexChars = @"[]\^$.|?*+()";
        #endregion

        #region Helpers
        private void RecreateSectionRegex(char value)
        {
            if (char.IsControl(value)
                || char.IsWhiteSpace(value)
                || CommentString.Contains(new string(new[] { value }))
                || value == KeyValueAssigmentChar)
                throw new Exception(string.Format("Invalid character for section delimiter: '{0}", value));

            string builtRegexString = _strSectionRegexStart;

            if (_strSpecialRegexChars.Contains(new string(_sectionStartChar, 1)))
                builtRegexString += "\\" + _sectionStartChar;
            else builtRegexString += _sectionStartChar;

            builtRegexString += _strSectionRegexMiddle;

            if (_strSpecialRegexChars.Contains(new string(_sectionEndChar, 1)))
                builtRegexString += "\\" + _sectionEndChar;
            else
                builtRegexString += _sectionEndChar;

            builtRegexString += _strSectionRegexEnd;

            SectionRegex = new Regex(builtRegexString);
        }
        #endregion

        public override int GetHashCode()
        {
            var hash = 27;
            foreach (var property in GetType().GetProperties())
            {
                hash = (hash * 7) + property.GetValue(this, null).GetHashCode();
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            var copyObj = obj as ini_config;
            if (copyObj == null) return false;

            var oriType = this.GetType();
            try
            {
                foreach (var property in oriType.GetProperties())
                {
                    if (property.GetValue(copyObj, null).Equals(property.GetValue(this, null)))
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        #region ICloneable Members
      
        public ini_config Clone()
        {
            return this.MemberwiseClone() as ini_config;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}