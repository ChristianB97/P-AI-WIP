using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Linq;
using LS.Exceptions;

namespace Translation
{
    public static class CsvIO
    {
        public static void DetectLanguages(Translator translator, string file, char delimiter)
        {
            try
            {
                var lines = FileHelper.ReadLines(file);

                if (lines.Length == 0)
                    throw new Exception("Empty language file");

                foreach (var lang in ExtractLanguages(lines[0], delimiter))
                {
                    translator.AddLanguageReference(lang, lang);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Error loading the CSV file. Details:");
                Debug.LogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Imports a dictionary from a CSV file.
        /// </summary>
        /// <param name="file">File path.</param>
        public static void ImportFromCSV(Translator translator, string file, char delimiter, string lang)
        {
            var lines = FileHelper.ReadLines(file);

            if (lines.Length == 0)
            {
                throw new ValidationException("Empty language file");
            }

            var langs = ExtractLanguages(lines[0], delimiter).ToArray();

            int index = -1;

            for (int i = 0; i < langs.Length; i++)
            {
                if (langs[i].Equals(lang))
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                throw new ValidationException("Language not found: " + lang);
            }

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]) || (lines[i].StartsWith("#") && lines[i].EndsWith(delimiter.ToString())))
                    continue;

                var words = SplitLine(lines[i], delimiter);
                var key = words[0];

                translator.AddWord(key, words[index + 1]);
            }

            translator.LangCulture = lang;
            translator.LangTitle = lang;
            translator.Format = ELangFormat.Csv;
            translator.LangName = file;
            translator.LangFile = file;
        }

        private static IEnumerable<string> ExtractLanguages(string line, char delimiter)
        {
            var langs = line.Split(delimiter);

            foreach (var lang in langs)
            {
                string parsed = lang.Trim(' ', '"');
                if (!string.IsNullOrEmpty(parsed))
                    yield return parsed;
            }
        }

        private static string[] SplitLine(string line, char delimiter)
        {
            var result = new List<string>();
            bool starting = true;
            bool startWithComilla = false;
            int comillaCount = 0;
            string accumulator = "";

            for (int i = 0; i < line.Length; i++)
            {
                if (starting)
                {
                    if (line[i] == '"')
                    {
                        startWithComilla = true;
                        starting = false;
                    }
                    else if (line[i] != ' ')
                    {
                        startWithComilla = false;
                        starting = false;
                        accumulator += line[i];
                    }
                }
                else
                {
                    if (line[i] == delimiter)
                    {
                        if (!startWithComilla)
                        {
                            result.Add(accumulator);
                            accumulator = "";
                            comillaCount = 0;
                            starting = true;
                            startWithComilla = false;
                        }
                        else if (comillaCount > 0)
                        {
                            //  Imposible
                            throw new Exception("Imposible");
                        }
                        else
                        {
                            accumulator += delimiter;
                        }
                    }
                    else if (line[i] == '"')
                    {
                        if (startWithComilla)
                        {
                            if (i + 1 < line.Length)
                            {
                                if (line[i + 1] == '"')
                                {
                                    accumulator += '"';
                                    i++;
                                }
                                else
                                {
                                    result.Add(accumulator);
                                    accumulator = "";
                                    comillaCount = 0;
                                    starting = true;
                                    startWithComilla = false;

                                    while (i < line.Length && line[i] != delimiter)
                                    {
                                        i++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            accumulator += line[i];
                        }
                    }
                    else
                    {
                        accumulator += line[i];
                    }
                }
            }

            if (!string.IsNullOrEmpty(accumulator))
            {
                result.Add(accumulator);
            }

            return result.ToArray();
        }
    }
}

