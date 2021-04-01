using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
#if UNITY_WINRT
using UnityEngine.Windows;
#endif
using System.IO;

namespace Translation
{
    internal static class FileHelper
    {
        public static Dictionary<string, string> Cache;

        static FileHelper()
        {
            Cache = new Dictionary<string, string>();
        }

        public static EPlatform GetPlatform()
        {

#if UNITY_ANDROID
			return EPlatform.ANDROID;
#elif UNITY_WEBPLAYER
			return EPlatform.WEBPLAYER;
#else
            return EPlatform.DESKTOP;
#endif
        }

        /// <summary>
        /// Gets the file trying to get from references.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="refs">References.</param>
        /// <param name="defaultFile">Default file.</param>
        public static string GetFile(IEnumerable<FileReference> refs, string defaultFile)
        {

            if (refs == null)
                return defaultFile;

            FileReference reference = null;
            EPlatform platf = GetPlatform();

            //	Buscar referencia a plataforma
            foreach (var re in refs)
            {
                foreach (var pl in re.platforms)
                {
                    if (pl == platf)
                    {
                        reference = re;
                        break;
                    }
                }

                if (reference != null)
                    break;
            }

            //	Si existe, devolver
            if (reference != null)
                return reference.pathOrUrl;
            else
            {
                //	Sino, generar path
                //string filePath = System.IO.Path.Combine(Application.dataPath, defaultFile);

                //if (filePath.Contains("://")) {
                //	filePath = Application.dataPath +"/"+ System.Uri.EscapeUriString(defaultFile);
                //}

                //return filePath;

                return defaultFile;
            }
        }

        [Obsolete]
        public static string GetPath(string filename)
        {
            return Path.Combine(Application.dataPath, filename);
        }

        [Obsolete]
        public static bool Exists(string filename)
        {
#if UNITY_WINRT
			return UnityEngine.Windows.File.Exists (GetPath (filename));	
#else
            return File.Exists(GetPath(filename));
#endif
        }

        public static string Read(string pathOrUrl)
        {
#if UNITY_EDITOR
            if (pathOrUrl.StartsWith("$mem$"))
            {
                return pathOrUrl.Substring(5);
            }
#endif

            if (Cache.ContainsKey(pathOrUrl))
            {
                return Cache[pathOrUrl];
            }

            string result = "";

            if (pathOrUrl.Contains("://"))
            {

                //WWW www = new WWW(pathOrUrl);
                UnityWebRequest www = UnityWebRequest.Get(pathOrUrl);

                int timeout = 20 * 1000;

                while (!www.isDone && !www.isNetworkError && !www.isHttpError)
                {
#if !UNITY_WINRT
                    System.Threading.Thread.Sleep(100);
#endif
                    timeout -= 100;

                    if (timeout <= 0)
                    {
                        throw new TimeoutException("The operation was timed-out (" + pathOrUrl + ")");
                    }
                }

                if (www.isHttpError || www.isNetworkError)
                {
                    throw new Exception(www.error);
                }

                result = www.downloadHandler.text;
            }
            else if (!Application.isEditor)
            {

                var asset = Resources.Load<TextAsset>(pathOrUrl);

                if (asset == null)
                    throw new Exception("Can't find the file: " + pathOrUrl);

                result = asset.text;

                try { Resources.UnloadAsset(asset); }
                catch { }
            }
#if UNITY_EDITOR
            else
            {
                var allResources = System.IO.Directory.GetDirectories(Application.dataPath, "Resources", SearchOption.AllDirectories);
                foreach (var resource in allResources)
                {
                    string resourcePath = Path.Combine(resource, pathOrUrl).Replace('\\', '/');
                    string folder = Path.GetDirectoryName(resourcePath);
                    string name = Path.GetFileNameWithoutExtension(resourcePath);

                    var di = new System.IO.DirectoryInfo(folder);
                    if (di.Exists)
                    {
                        foreach (FileInfo fi in di.GetFiles(name + ".*", SearchOption.TopDirectoryOnly))
                        {
                            if (fi.Name.ToLower().EndsWith(".meta"))
                                continue;

                            return System.IO.File.ReadAllText(fi.FullName);
                        }
                    }
                }
            }
#endif

            Cache[pathOrUrl] = result;

            return result;
        }

        public static string[] ReadLines(string filename)
        {
            return Read(filename).Split(new string[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}

