using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using UnityEngine;

namespace Save
{
    internal class SavingEnvironment : MonoBehaviour
    {
        private static Dictionary<string, string> saveMapping;
        private static string filePath;
        public static string MY_SAVE = "SAVE1";
        public static string FILE_TYPE = "xml";
        private static char SPECIAL_CHARACTER = '$';

        public static void InitEnvironment()
        {
            if (!IsInitiated())
            {            
                saveMapping = new Dictionary<string, string>();
                filePath = Application.persistentDataPath + "/" + MY_SAVE + "." + FILE_TYPE;
                ReadToRuntime();
            }  
        }

        public static bool IsInitiated()
        {
            return !string.IsNullOrEmpty(filePath);
        }

        public static void WriteRuntime<T>(T toSave, String key)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter sWriter = new StringWriter();
            serializer.Serialize(sWriter, toSave);
            if (saveMapping.ContainsKey(key))
            {
                saveMapping[key] = sWriter.ToString();
            }
            else
            {
                saveMapping.Add(key, sWriter.ToString());
            }
        }

        public static void WriteRuntimeToFile()
        {
            try
            {
                TextWriter writer = new StreamWriter(filePath);
                writer.Write("");
                writer.Close();
                writer = new StreamWriter(filePath);
                StringWriter sWriter = new StringWriter();
                using (writer)
                {
                    foreach (KeyValuePair<string, string> entry in saveMapping)
                    {
                        writer.Write(SPECIAL_CHARACTER + entry.Key);
                        writer.WriteLine();
                        writer.Write(entry.Value);
                        writer.WriteLine();
                    }
                }
                writer.Close();
            }
            catch (Exception exc)
            {
                Debug.LogError("Write Error in SavingEnvironment" + exc);
            }
        }

        public static void ReadToRuntime()
        {
            try
            {
                using (TextReader reader = new StreamReader(filePath))
                {
                    if (File.Exists(filePath))
                    {
                        string line;
                        string key = "";
                        string value = "";
                        saveMapping = new Dictionary<string, string>();

                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line[0] == SPECIAL_CHARACTER)
                            {
                                TryAddToRuntime(key, value);
                                key = WashKey(line);
                                value = "";
                            }
                            else
                            {
                                value += line;
                            }
                        }
                        saveMapping.Add(key, value);
                    }
                    else
                    {
                        throw new Exception("Dateipfad existiert nicht! Lesen nicht möglich!");
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.LogError("Read Error in Saving Environment" + exc);
            }
        }

        private static void TryAddToRuntime(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !saveMapping.ContainsKey(key))
            {
                saveMapping.Add(key, value);
            }
        }

        private static string WashKey(string key)
        {
            return key.Substring(1);
        }


        public static T ReadValueFromRuntime<T>(string key)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (saveMapping.ContainsKey(key))
            {
                StringReader reader = new StringReader(saveMapping[key]);
                return (T)serializer.Deserialize(reader);
            }
            return default(T);

        }
    }
}
