using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Translation.Editor
{
    public class Icons
    {
        static Dictionary<string, Texture2D> icons = new Dictionary<string, Texture2D>();

        public static Texture2D Get(string name)
        {
            Texture2D tex;

            if (!icons.TryGetValue(name, out tex))
            {
                tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/XML Localization System/Editor/Icons/" + name + ".png", typeof(Texture2D));
                icons[name] = tex;
            }

            return tex;
        }
    }
}

