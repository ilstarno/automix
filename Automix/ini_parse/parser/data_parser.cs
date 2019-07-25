using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Automix_ini.exceptions;
using Automix_ini.model;
using Automix_ini.model.config;

namespace Automix_ini.parser
{
    public class data_parser
    {
        #region Private
        private List<Exception> _errorExceptions;
        #endregion

        #region Initializations
        public data_parser(IniParser.Model.Configuration.concatenate_double_key parserConfiguration)
            : this(new ini_config())
        { }

        public data_parser(ini_config parserConfiguration)
        {
            if (parserConfiguration == null)
                throw new ArgumentNullException("parserConfiguration");

            Configuration = parserConfiguration;

            _errorExceptions = new List<Exception>();
        }

        public data_parser()
        {
        }

        #endregion

        #region State
        public virtual ini_config Configuration { get; protected set; }

        public bool HasError { get { return _errorExceptions.Count > 0; } }


        public ReadOnlyCollection<Exception> Errors { get { return _errorExceptions.AsReadOnly(); } }
        #endregion

        #region Operations

       
        public data Parse(string iniDataString)
        {

            data iniData = Configuration.CaseInsensitive ? new non_sense_data() : new data();
            iniData.Configuration = this.Configuration.Clone();

            if (string.IsNullOrEmpty(iniDataString))
            {
                return iniData;
            }

            _errorExceptions.Clear();
            _currentCommentListTemp.Clear();
            _currentSectionNameTemp = null;

            try
            {
                var lines = iniDataString.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
                for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
                {
                    var line = lines[lineNumber];

                    if (line.Trim() == String.Empty) continue;

                    try
                    {
                        ProcessLine(line, iniData);
                    }
                    catch (Exception ex)
                    {
                        var errorEx = new ParsingException(ex.Message, lineNumber + 1, line, ex);
                        if (Configuration.ThrowExceptionsOnError)
                        {
                            throw errorEx;
                        }
                        else
                        {
                            _errorExceptions.Add(errorEx);
                        }

                    }
                }

                if (_currentCommentListTemp.Count > 0)
                {
                    if (iniData.Sections.Count > 0)
                    {
                        iniData.Sections.GetSectionData(_currentSectionNameTemp).TrailingComments
                            .AddRange(_currentCommentListTemp);
                    }
                    else if (iniData.Global.Count > 0)
                    {
                        iniData.Global.GetLast().Comments
                            .AddRange(_currentCommentListTemp);
                    }


                    _currentCommentListTemp.Clear();
                }

            }
            catch (Exception ex)
            {
                _errorExceptions.Add(ex);
                if (Configuration.ThrowExceptionsOnError)
                {
                    throw;
                }
            }


            if (HasError) return null;
            return (data) iniData.Clone();
        }
        #endregion

        #region Template Method Design Pattern 
       
        protected virtual bool LineContainsAComment(string line)
        {
            return !string.IsNullOrEmpty(line)
                && Configuration.CommentRegex.Match(line).Success;
        }

       
        protected virtual bool LineMatchesASection(string line)
        {
            return !string.IsNullOrEmpty(line)
                && Configuration.SectionRegex.Match(line).Success;
        }

       
        protected virtual bool LineMatchesAKeyValuePair(string line)
        {
            return !string.IsNullOrEmpty(line) && line.Contains(Configuration.KeyValueAssigmentChar.ToString());
        }

       
        protected virtual string ExtractComment(string line)
        {
            string comment = Configuration.CommentRegex.Match(line).Value.Trim();

            _currentCommentListTemp.Add(comment.Substring(1, comment.Length - 1));

            return line.Replace(comment, "").Trim();
        }

       
        protected virtual void ProcessLine(string currentLine, data currentIniData)
        {
            currentLine = currentLine.Trim();

            if (LineContainsAComment(currentLine))
            {
                currentLine = ExtractComment(currentLine);
            }

        
            if (currentLine == String.Empty)
                return;

            if (LineMatchesASection(currentLine))
            {
                ProcessSection(currentLine, currentIniData);
                return;
            }

            if (LineMatchesAKeyValuePair(currentLine))
            {
                ProcessKeyValuePair(currentLine, currentIniData);
                return;
            }

            if (Configuration.SkipInvalidLines)
                return;

            throw new ParsingException(
                "Unknown file format. Couldn't parse the line: '" + currentLine + "'.");
        }

        protected virtual void ProcessSection(string line, data currentIniData)
        {
            string sectionName = Configuration.SectionRegex.Match(line).Value.Trim();

            sectionName = sectionName.Substring(1, sectionName.Length - 2).Trim();

            if (sectionName == string.Empty)
            {
                throw new ParsingException("Section name is empty");
            }

            _currentSectionNameTemp = sectionName;

            if (currentIniData.Sections.ContainsSection(sectionName))
            {
                if (Configuration.AllowDuplicateSections)
                {
                    return;
                }

                throw new ParsingException(string.Format("Duplicate section with name '{0}' on line '{1}'", sectionName, line));
            }


            currentIniData.Sections.AddSection(sectionName);

            currentIniData.Sections.GetSectionData(sectionName).LeadingComments = _currentCommentListTemp;
            _currentCommentListTemp.Clear();

        }

        protected virtual void ProcessKeyValuePair(string line, data currentIniData)
        {
            string key = ExtractKey(line);

            if (string.IsNullOrEmpty(key) && Configuration.SkipInvalidLines) return;

            string value = ExtractValue(line);

            if (string.IsNullOrEmpty(_currentSectionNameTemp))
            {
                if (!Configuration.AllowKeysWithoutSection)
                {
                    throw new ParsingException("key value pairs must be enclosed in a section");
                }

                AddKeyToKeyValueCollection(key, value, currentIniData.Global, "global");
            }
            else
            {
                var currentSection = currentIniData.Sections.GetSectionData(_currentSectionNameTemp);

                AddKeyToKeyValueCollection(key, value, currentSection.Keys, _currentSectionNameTemp);
            }
        }

        protected virtual string ExtractKey(string s)
        {
            int index = s.IndexOf(Configuration.KeyValueAssigmentChar, 0);

            return s.Substring(0, index).Trim();
        }

        protected virtual string ExtractValue(string s)
        {
            int index = s.IndexOf(Configuration.KeyValueAssigmentChar, 0);

            return s.Substring(index + 1, s.Length - index - 1).Trim();
        }

        protected virtual void HandleDuplicatedKeyInCollection(string key, string value, key_collection keyDataCollection, string sectionName)
        {
            if (!Configuration.AllowDuplicateKeys)
            {
                throw new ParsingException(string.Format("Duplicated key '{0}' found in section '{1}", key, sectionName));
            }
            else if (Configuration.OverrideDuplicateKeys)
            {
                keyDataCollection[key] = value;
            }
        }
        #endregion

        #region Helpers
       
        private void AddKeyToKeyValueCollection(string key, string value, key_collection keyDataCollection, string sectionName)
        {
            if (keyDataCollection.ContainsKey(key))
            {
                HandleDuplicatedKeyInCollection(key, value, keyDataCollection, sectionName);
            }
            else
            {
                keyDataCollection.AddKey(key, value);
            }

            keyDataCollection.GetKeyData(key).Comments = _currentCommentListTemp;
            _currentCommentListTemp.Clear();
        }
        #endregion

        #region Fields

        private readonly List<string> _currentCommentListTemp = new List<string>();

        private string _currentSectionNameTemp;
        #endregion
    }
}