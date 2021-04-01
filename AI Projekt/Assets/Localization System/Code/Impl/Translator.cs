using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Translation
{
    public sealed class Translator : ITranslator
    {
        #region Attributes
        string langTitle;
        /// <summary>
        /// Title from loaded language.
        /// </summary>
        public string LangTitle
        {
            get { return langTitle; }
            set { langTitle = value; }
        }

        string langCulture;
        /// <summary>
        /// Loaded language code.
        /// </summary>
        public string LangCulture
        {
            get { return langCulture; }
            set { langCulture = value; }
        }

        string langName;
        /// <summary>
        /// Language name.
        /// </summary>
        public string LangName
        {
            get { return langName; }
            set { langName = value; }
        }

        string langFile;
        public string LangFile
        {
            get
            {
                return this.langFile;
            }
            set
            {
                langFile = value;
            }
        }
        Dict languages = new Dict();
        /// <summary>
        /// Available languages.
        /// </summary>
        public Dict Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        Dict words = new Dict();
        /// <summary>
        /// Loaded dictionary content.
        /// </summary>
        public Dict Words
        {
            get { return words; }
            set { words = value; }
        }

        ELangFormat format;
        public ELangFormat Format
        {
            get
            {
                return format;
            }

            set
            {
                format = value;
            }
        }

        object[] formatArgs;

        List<FileReference> references = new List<FileReference>();
        List<string> additionalCsvFilesSaved = new List<string>();

        public bool Verbose { get; set; }

        /// <summary>
        /// Gets the selected language.
        /// </summary>
        public string SelectedLanguage
        {
            get { return TranslationEngine.Instance.Language; }
        }

        /// <summary>
        /// Gets the OS default language. Same function as ContextLanguage.
        /// </summary>
        /// <value>
        /// The context language.
        /// </value>
        public string SystemLanguage
        {
            get { return ContextLanguage; }
        }

        /// <summary>
        /// Gets the context's default language.
        /// </summary>
        /// <value>
        /// The context language.
        /// </value>
        public string ContextLanguage
        {
            get { return ContextHelper.GetContextLanguage(); }
        }

        /// <summary>
        /// Gets a translated string.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name]
        {
            get
            {
                string _return;
                if (!string.IsNullOrEmpty(name) && Words.ContainsKey(name))
                {
                    _return = Words.Get(name);
                }
                else
                {
                    _return = string.Empty;
#if UNITY_EDITOR
                    if (Verbose)
                    {
                        Debug.LogWarning("Cannot find the word: \"" + name + "\" in your dictionary.");
                    }
#endif
                }

                return _return.Replace("{\\n}", "\n");
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public Translator(bool init)
        {
            if (!init) return;
            Verbose = false;
            Configure();
            LoadDictionary(ContextLanguage);
        }

        public Translator()
            : this(true)
        {
        }

        public Translator(bool init, ELangFormat format, params object[] args)
        {
            if (init)
                Init(format, args);
        }

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name='format'>File format.</param>
        /// <param name='args'>Extra arguments. For CSV: [file, delimiter].</param>
        public Translator(ELangFormat format, params object[] args)
        {
            Configure(format, args);
            LoadDictionary(ContextLanguage);
        }

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name='format'>File format.</param>
        /// <param name='args'>Extra arguments. For CSV: [file, delimiter].</param>
        public Translator(ELangFormat format, bool autoLoadDictionary, params object[] args)
        {
            Configure(format, args);
            if (autoLoadDictionary)
            {
                LoadDictionary(ContextLanguage);
            }
        }
        #endregion

        #region Private methods

        private void Configure()
        {
            Configure(ELangFormat.Xml, null);
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name='config'>Config.</param>
        /// <param name='args'>Extra arguments. For CSV: [file, delimiter, aditionalFiles].</param>
        private void Configure(ELangFormat format, params object[] args)
        {
            this.format = format;
            this.formatArgs = args;

            switch (format)
            {
                case ELangFormat.Xml:
                    {
                        if (references != null && references.Count > 0)
                        {
                            Debug.LogWarning("You can use FileReference script only with CSV format. XML works only with local files.");
                        }

                        XmlIO.DetectLanguages(this);
                    }
                    break;

                case ELangFormat.Csv:
                    {
                        if (args.Length < 2)
                        {
                            throw new Exception("You must specify: filename, delimiter");
                        }

                        additionalCsvFilesSaved.Clear();

                        if (args.Length >= 3)
                        {
                            var additionalCsvFiles = (string[])args[2];

                            if (additionalCsvFiles != null)
                            {
                                foreach (var additionalFile in additionalCsvFiles)
                                {
                                    additionalCsvFilesSaved.Add(additionalFile);
                                }
                            }
                        }

                        string defaultFile = args[0].ToString();
                        char delimiter = args[1].ToString()[0];

                        string file = FileHelper.GetFile(this.references, defaultFile);
                        CsvIO.DetectLanguages(this, file, delimiter);

                    }
                    break;

            }
        }

        /// <summary>
        /// Imports a dictionary from a file.
        /// </summary>
        /// <param name="code"></param>
        private void ImportDictionary(string code)
        {
            if (format == ELangFormat.Xml)
            {
                string reference = Languages.Get(code);
                ImportFromXML(reference);
            }
            else if (format == ELangFormat.Csv)
            {
                string path = FileHelper.GetFile(this.references, formatArgs[0].ToString());

                foreach (var file in this.additionalCsvFilesSaved)
                {
                    ImportFromCSV(file, formatArgs[1].ToString()[0], code);
                }

                ImportFromCSV(path, formatArgs[1].ToString()[0], code);
            }
            else
            {
                throw new Exception("Invalid file format.");
            }

#if UNITY_EDITOR
            if (Verbose)
            {
                if (Words.GetCount() == 0)
                {
                    Debug.LogWarning("The dictionary is empty.");
                }
            }
#endif
        }

        /// <summary>
        /// Imports a dictionary from a XML file.
        /// </summary>
        /// <param name="file">File path.</param>
        private void ImportFromXML(string file)
        {
            XmlIO.ImportFromXML(this, file);
        }

        /// <summary>
        /// Imports a dictionary from a CSV file.
        /// </summary>
        /// <param name="file">File path.</param>
        /// <param name="delimiter">File delimiter. Default: ';'.</param>
        private void ImportFromCSV(string file, char delimiter, string lang)
        {
            CsvIO.ImportFromCSV(this, file, delimiter, lang);
        }

        /// <summary>
        /// Process the special characters.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private string ProcessWord(string word)
        {
            if (word.IndexOf('\\') == -1)
            {
                return word;
            }
            else
            {
                StringBuilder result = new StringBuilder();
                bool command = false;

                for (int i = 0; i < word.Length; i++)
                {
                    char letter = word[i];

                    if (!command)
                    {
                        if (letter == '\\')
                        {
                            command = true;
                        }
                        else
                        {
                            result.Append(letter);
                        }
                    }
                    else
                    {
                        if (letter == 'n')
                        {
                            result.Append('\n');
                        }
                        else if (letter == 'r')
                        {
                            result.Append('\r');
                        }
                        else if (letter == 'l')
                        {
                            result.Append(Environment.NewLine);
                        }
                        else if (letter == 't')
                        {
                            result.Append('\t');
                        }
                        else if (letter == '\\')
                        {
                            result.Append(letter);
                        }
                        command = false;
                    }
                }

                return result.ToString();
            }
        }

        #endregion

        #region Public methods

        public void Init(ELangFormat format, params object[] args)
        {
            Configure(format, args);
            LoadDictionary(ContextLanguage);
        }

        public void AddFileReference(FileReference file)
        {
            references.Add(file);
        }

        public void ClearFileReferences()
        {
            references.Clear();
        }

        public void AddWord(string key, string val)
        {
            this.words.Set(key, ProcessWord(val));
        }

        /// <summary>
        /// Adds a language reference.
        /// </summary>
        /// <param name='code'>
        /// Language code.
        /// </param>
        /// <param name='filename'>
        /// Filename.
        /// </param>
        public void AddLanguageReference(string code, string filename)
        {
            Languages.Set(code, filename);
        }

        /// <summary>
        /// Removes the language reference.
        /// </summary>
        /// <param name='code'>
        /// Code.
        /// </param>
        /// <exception cref='NotImplementedException'>
        /// Is thrown when the not implemented exception.
        /// </exception>
        public void RemoveLanguageReference(string code)
        {
            Languages.Remove(code);
        }

        /// <summary>
        /// Gets a pair language name/code, using an index.
        /// </summary>
        /// <returns>
        /// The language data from index.
        /// </returns>
        /// <param name='index'>
        /// Index.
        /// </param>
        public string GetLanguageDataFromIndex(int index)
        {

            foreach (string key in this.Languages.GetKeys())
            {
                if (index-- == 0)
                {
                    string name = this.Languages.Get(key);
                    return name + "," + key;
                }
            }

            return string.Empty;
        }

        public string GetLanguageNameFromIndex(int index)
        {
            return this.Languages.Get(GetLanguageCodeFromIndex(index));
        }

        public string GetLanguageCodeFromIndex(int index)
        {
            var keys = this.Languages.GetKeys();
            if (index > keys.Length)
            {
                return string.Empty;
            }
            return keys[index];
        }

        /// <summary>
        /// Loads a dicionary. If not exists, look for the dictionary country. If not exists (again), loads the first one.
        /// </summary>
        /// <param name="code">Language code. Example #1: en-US. Example #2: en.</param>
        public void LoadDictionary(string code)
        {
            var backup = words;
            try
            {
                words = new Dict();

                if (Languages.ContainsKey(code))
                {
                    ImportDictionary(code);
                }
                else if (this.GetAvailableLanguages().Length > 0)
                {
                    if (!this.GetAvailableLanguages()[0].Equals(code))
                    {
                        //Debug.LogWarning("Translation: the language \"" + code + "\" does not exists. Loading \"" + this.GetAvailableLanguages()[0] + "\"...");
                        LoadDictionary(this.GetAvailableLanguages()[0]);
                    }
                    else
                    {
                        Debug.LogError("Translation: the language \"" + code + "\" does not exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading " + code);
                words = backup;
                throw ex;
            }
        }

        /// <summary>
        /// Loads a dicionary. If not exists, look for the dictionary country. If not exists (again), loads the first one.
        /// </summary>
        /// <param name="name">Language name.</param>
        public void LoadDictionaryFromValue(string name)
        {
            var backup = words;
            try
            {
                words = new Dict();
                ImportDictionary(name);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading " + name);
                words = backup;
                throw ex;
            }
        }

#if UNITY_EDITOR

        public void SaveDictionary()
        {
            if (string.IsNullOrEmpty(LangTitle))
            {
                LangTitle = LangCulture;
            }

            XmlIO.ExportToXML(this, LangFile);
        }

#endif

        /// <summary>
        /// Gets the available languages.
        /// </summary>
        /// <returns>
        /// The available languages, or null.
        /// </returns>
        public string[] GetAvailableLanguages()
        {
            if (Languages.GetCount() == 0)
                return null;

            var list = new string[Languages.GetCount()];
            int i = 0;

            foreach (string lang in Languages.GetValues())
            {
                list[i++] = lang;
            }

            return list;
        }

        /// <summary>
        /// Gets the available languages.
        /// </summary>
        /// <returns>
        /// The available languages (code), or null.
        /// </returns>
        public string[] GetCodeAvailableLanguages()
        {
            if (Languages.GetCount() == 0)
                return null;
            var list = new string[Languages.GetCount()];
            int i = 0;
            foreach (string lang in Languages.GetKeys())
            {
                list[i++] = lang;
            }
            return list;
        }

        public Translator Clone()
        {
            return new Translator(true, this.format, this.formatArgs);
        }

        #endregion

        #region Other methods
        internal void LoadFromXML(XML.XmlLangFileInfo info, bool clear)
        {
            this.LangName = info.File;
            this.LangFile = info.File;
            this.LangTitle = info.Title;
            this.LangCulture = info.Code;

            if (clear || this.Words == null)
                this.Words = new Dict();

            foreach (var key in info.Words.GetKeys())
            {
                this.AddWord(key, info.Words.Get(key));
            }
        }

        #endregion
    }
}