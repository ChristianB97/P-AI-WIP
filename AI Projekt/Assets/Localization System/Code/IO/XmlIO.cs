using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using Translation.XML;
#if UNITY_WINRT
using EX = Translation.WindowsSDK;
#else
using EX = System;
#endif

namespace Translation
{
    public static class XmlIO
    {
        public static object OpenXML(string filename)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(FileHelper.Read(filename));
            return xDoc;
        }

        public static List<XmlLanguageDefinition> DetectLanguages()
        {
            List<XmlLanguageDefinition> result = new List<XmlLanguageDefinition>();
            XmlDocument xDoc = (XmlDocument)OpenXML("translator.cfg");

            var root = (XmlElement)xDoc.GetElementsByTagName("root")[0];
            int version = int.Parse(root.GetElementsByTagName("spec-version")[0].InnerText);

            switch (version)
            {
                case 1:
                    {
                        var languagesNode = (XmlElement)root.GetElementsByTagName("languages")[0];
                        var languagesNodeList = languagesNode.GetElementsByTagName("language");
                        foreach (XmlElement pNode in languagesNodeList)
                        {
                            string nodeFile = pNode.GetAttribute("file");
                            string nodeCode = pNode.GetAttribute("code");
                            result.Add(new XmlLanguageDefinition()
                            {
                                File = nodeFile,
                                Code = nodeCode
                            });
                        }
                    }
                    break;
                default:
                    throw new EX.InvalidProgramException("@str/ex.admin-invalid-version.msg");
            }

            return result;
        }

        public static void DetectLanguages(Translator translator)
        {
            try
            {
                foreach (var definition in DetectLanguages())
                {
                    translator.AddLanguageReference(definition.Code, definition.File);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Translator NOT configured. Details:");
                Debug.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Imports a dictionary from a XML file.
        /// </summary>
        /// <param name="file">File path.</param>
        public static XmlLangFileInfo ImportFromXML(string file)
        {
            XmlDocument xDoc = (XmlDocument)OpenXML(file);
            XmlLangFileInfo info = new XmlLangFileInfo();

            var root = (XmlElement)xDoc.GetElementsByTagName("root")[0];
            int version = int.Parse(root.GetElementsByTagName("spec-version")[0].InnerText);

            switch (version)
            {
                case 1:
                    {
                        var languageNode = (XmlElement)root.GetElementsByTagName("language")[0];
                        info.File = file;
                        info.Code = languageNode.GetAttribute("name");
                        info.Title = languageNode.GetAttribute("title");

                        var wordsNodeList = languageNode.GetElementsByTagName("word");
                        foreach (XmlElement pNode in wordsNodeList)
                        {
                            var wordName = pNode.GetAttribute("name");
                            var wordText = pNode.InnerText;
                            switch (pNode.GetAttribute("type").ToLower())
                            {
                                case "menu":
                                    wordText = wordText.Replace('_', '&');
                                    break;
                                default:
                                    break;
                            }

                            info.Words.Set(wordName, wordText);
                        }
                    }
                    break;
                default:
                    throw new EX.InvalidProgramException("@str/ex.admin-invalid-version.msg");
            }

            return info;
        }

        /// <summary>
        /// Imports a dictionary from a XML file.
        /// </summary>
        /// <param name="file">File path.</param>
        public static void ImportFromXML(Translator translator, string file)
        {
            translator.LoadFromXML(ImportFromXML(file), false);
        }

#if UNITY_EDITOR

        public static void ExportToXML(Translator translator, string file)
        {
            XmlDocument xml = new XmlDocument();
            xml.CreateXmlDeclaration("1.0", "utf-8", null);

            XmlNode root = xml.CreateNode(XmlNodeType.Element, "root", null);
            xml.AppendChild(root);

            XmlNode child = xml.CreateNode(XmlNodeType.Element, "spec-version", null);
            child.InnerText = "1";
            root.AppendChild(child);

            var atName = xml.CreateAttribute("name");
            var atTitle = xml.CreateAttribute("title");
            atName.Value = translator.LangCulture;
            atTitle.Value = translator.LangTitle;

            child = xml.CreateNode(XmlNodeType.Element, "language", null);
            child.Attributes.Append(atName);
            child.Attributes.Append(atTitle);
            root.AppendChild(child);

            XmlNode wordNode;

            foreach (string key in translator.Words.GetKeys())
            {
                wordNode = xml.CreateNode(XmlNodeType.Element, "word", null);
                var wName = xml.CreateAttribute("name");
                wName.Value = key;
                wordNode.Attributes.Append(wName);
                wordNode.InnerText = translator.Words.Get(key);
                child.AppendChild(wordNode);
            }

            xml.Save(file);
        }

#endif
    }
}

