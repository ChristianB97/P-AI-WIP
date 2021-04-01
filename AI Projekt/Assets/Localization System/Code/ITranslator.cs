using System;
using System.Collections.Generic;
using UnityEngine;

namespace Translation
{
    public interface ITranslator
    {
        /// <summary>
        /// Title from loaded language.
        /// </summary
		string LangTitle { get; set; }

        /// <summary>
        /// Loaded language.
        /// </summary>
        string LangCulture { get; set; }

        /// <summary>
        /// Available languages.
        /// </summary>
        Dict Languages { get; set; }

        /// <summary>
        /// Loaded dictionary content.
        /// </summary>
        Dict Words { get; set; }

        /// <summary>
        /// Gets the context language.
        /// </summary>
        /// <value>
        /// The context language.
        /// </value>
        string ContextLanguage { get; }

        /// <summary>
        /// Gets a translated string.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string this[string name] { get; }

        /// <summary>
        /// Adds a language reference.
        /// </summary>
        /// <param name='code'>
        /// Language code.
        /// </param>
        /// <param name='filename'>
        /// Filename.
        /// </param>
        void AddLanguageReference(string code, string filename);

        /// <summary>
        /// Removes the language reference.
        /// </summary>
        /// <param name='code'>
        /// Code.
        /// </param>
        /// <exception cref='NotImplementedException'>
        /// Is thrown when the not implemented exception.
        /// </exception>
        void RemoveLanguageReference(string code);

        /// <summary>
        /// Loads a dicionary. If not exists, look for the dictionary country. If not exists (again), loads the first one.
        /// </summary>
        /// <param name="code">Language code. Example #1: en-US. Example #2: en.</param>
        void LoadDictionary(string code);

        /// <summary>
        /// Gets the available languages.
        /// </summary>
        /// <returns>
        /// The available languages, or null.
        /// </returns>
        string[] GetAvailableLanguages();
    }
}

