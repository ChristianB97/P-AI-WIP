using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Translation;
using Translation.XML;
using UnityEditor;
using UnityEngine;

namespace LS.Editor
{
    public static class XMLEditorManager
    {
        private static List<XmlLanguageDefinition> definitions = null;

        public static XmlLanguageDefinition GetLanguage(string code)
        {
            if (definitions == null)
                definitions = XmlIO.DetectLanguages();

            var last = definitions.FindLastIndex(m => m.Code == code);

            if (last != -1)
            {
                return definitions[last];
            }
            else
            {
                return null;
            }
        }

        public static void EditLanguage(string oldCode, string file, string code)
        {
            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(oldCode) || string.IsNullOrEmpty(code))
            {
                throw new Exception("The filename and code cannot be empty.");
            }

            definitions = XmlIO.DetectLanguages();
            if (code != oldCode && definitions.Any(m => m.Code == code))
            {
                throw new Exception("The language '" + code + "' already exists");
            }

            List<string> keys = GetAllKeys(definitions);

            var oldDefinition = GetLanguage(oldCode);
            definitions.RemoveAll(m => m.Code == oldCode);

            definitions.Add(new XmlLanguageDefinition()
            {
                Code = code,
                File = file
            });

            //  Create new language file using old values and delete old file

            if (oldDefinition.File != file)
            {
                string xmlFilename = GetLangFilename(file);
                XmlLangFileInfo oldInfo = XmlIO.ImportFromXML(oldDefinition.File);
                SaveLanguageFile(xmlFilename, code, keys, oldInfo.Words);
            }

            //  Save references

            SaveLanguageReferences();

            //  Delete the old filename
            try
            {
                string oldXmlFilename = GetLangFilename(oldDefinition.File);
                File.Delete(oldXmlFilename);
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Error deleting " + oldDefinition.File + ": " + ex.Message);
            }
        }

        public static void AddLanguage(string file, string code)
        {
            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(code))
            {
                throw new Exception("The filename and code cannot be empty.");
            }

            definitions = XmlIO.DetectLanguages();
            if (definitions.Any(m=>m.Code == code))
            {
                throw new Exception("The language '" + code + "' already exists");
            }

            definitions.Add(new XmlLanguageDefinition()
            {
                Code = code,
                File = file
            });

            //  Add new language file

            string xmlFilename = GetLangFilename(file);

            List<string> keys;
            try
            {
                keys = GetAllKeys(definitions);
            }
            catch
            {
                keys = new List<string>();
            }

            SaveLanguageFile(xmlFilename, code, keys);

            //  Save references

            SaveLanguageReferences();
        }

        private static void SaveLanguageFile(string filename, string code, List<string> keys)
        {
            SaveLanguageFile(filename, code, keys, null);
        }

        private static void SaveLanguageFile(string filename, string code, List<string> keys, Dict words)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
                sw.WriteLine("<root>");
                sw.WriteLine("\t<spec-version>1</spec-version>");
                sw.WriteLine(string.Format("\t<language name='{0}' title='{0}'>", code));
                if (keys.Count > 0)
                {
                    foreach (string key in keys)
                    {
                        string val = (words == null || !words.ContainsKey(key)) ? key : words.Get(key);
                        sw.WriteLine("\t\t<word name='" + key + "'>" + val + "</word>");
                    }
                }
                else
                {
                    sw.WriteLine("\t\t<word name='hello'>hello</word>");
                }
                sw.WriteLine("\t</language>");
                sw.WriteLine("</root>");
                sw.Close();
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Obtiene absolutamente todas las KEYS
        /// </summary>
        /// <param name="defs"></param>
        /// <returns></returns>
        private static List<string> GetAllKeys(List<XmlLanguageDefinition> defs)
        {
            List<string> result = new List<string>();

            foreach (var def in defs)
            {
                var info = XmlIO.ImportFromXML(def.File);
                foreach (var key in info.Words.GetKeys())
                {
                    if(!result.Contains(key))
                        result.Add(key);
                }
            }

            return result;
        }

        public static void DeleteLanguage(string code)
        {
            definitions = XmlIO.DetectLanguages();
            var last = definitions.FindLastIndex(m => m.Code == code);
            if (last != -1)
            {
                var definition = definitions[last];
                var filename = GetLangFilename(definition.File);
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                definitions.RemoveAt(last);
                SaveLanguageReferences();
            }
        }

        private static void SaveLanguageReferences()
        {
            if (definitions == null)
                return;

            try
            {
                using (StreamWriter sw = new StreamWriter(MakePath("translator.cfg.xml"), false, Encoding.UTF8))
                {
                    sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
                    sw.WriteLine("<root>");
                    sw.WriteLine("\t<!-- Document format -->");
                    sw.WriteLine("\t<spec-version>1</spec-version>");
                    sw.WriteLine("\t<!-- References -->");
                    sw.WriteLine("\t<languages>");
                    foreach (var def in definitions)
                    {
                        sw.Write("\t\t");
                        sw.WriteLine(string.Format("<language file='{0}' code='{1}' />", def.File, def.Code));
                    }
                    sw.WriteLine("\t</languages>");
                    sw.WriteLine("</root>");
                    sw.Close();
                }

                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            finally
            {
                definitions = null;
            }
        }

        private static string GetLangFilename(string langName)
        {
            return MakePath(langName + ".xml");
        }

        private static string MakePath(string path)
        {
            string final = Path.Combine(Application.dataPath, "Localization System");
            final = Path.Combine(final, "Resources");
            final = Path.Combine(final, path);
            return final;
        }

        public static void Sync()
        {
            definitions = XmlIO.DetectLanguages();

            List<string> keys = GetAllKeys(definitions);

            int total = keys.Count;
            int counter = 0;

            foreach (var def in definitions)
            {
                if (SyncLang(def, keys))
                {
                    counter++;
                }
            }

            EditorUtility.DisplayDialog("Sync Keys", "Languages synchronized: " + counter + " changes of " + total + " languages", "OK");

            AssetDatabase.Refresh();
        }

        private static bool SyncLang(XmlLanguageDefinition def, List<string> keys)
        {
            bool save = false;

            XmlLangFileInfo info = XmlIO.ImportFromXML(def.File);

            foreach (string key in keys)
            {
                if (!info.Words.ContainsKey(key))
                {
                    save = true;
                    info.Words.Set(key, string.Empty);
                }
            }

            if (save)
            {
                SaveLanguageFile(GetLangFilename(def.File), def.Code, keys, info.Words);
            }

            return save;
        }

        public static void SaveLanguage(XmlLangFileInfo info)
        {
            SaveLanguageFile(GetLangFilename(info.File), info.Code, info.Words.GetKeys().ToList(), info.Words);
        }
    }
}