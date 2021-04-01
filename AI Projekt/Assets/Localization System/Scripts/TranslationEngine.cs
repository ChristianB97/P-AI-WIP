using System;
using System.Collections.Generic;
using Translation;
using UnityEngine;

public class TranslationEngine : MonoBehaviour
{
    private static TranslationEngine instance = null;
    public static TranslationEngine Instance
    {
        get
        {
            if ((instance ?? (instance = FindObjectOfType<TranslationEngine>())) == null)
            {
                Debug.LogError("TranslationEngine component doesn't exists. Please, add the script to a GameObject.");
            }

            return instance;
        }
    }

    private Translator translator;
    /// <summary>
    /// Translator instance.
    /// </summary>
    /// <value>
    /// The translation engine.
    /// </value>
    [Obsolete("Please, use Translator instead of Trans.")]
    public Translator Trans
    {
        get
        {
            return this.translator;
        }
        set
        {
            translator = value;
        }
    }

    /// <summary>
    /// Translator instance.
    /// </summary>
    /// <value>
    /// The translation engine.
    /// </value>
    /// <remarks>Alias</remarks>
    public Translator Translator
    {
        get
        {
            return this.translator;
        }
        set
        {
            translator = value;
        }
    }

    /// <summary>
    /// Selected language.
    /// </summary>
    public string Language = "en-US";

    public ELangFormat Format = ELangFormat.Xml;

    protected MonoBehaviour FormatComponent = null;

    public void SelectLanguage(string langCode)
    {
        Translator translator = TranslationEngine.Instance.Translator;

        if (translator == null)
        {
            Debug.LogWarning("Translator not found. Please, select a language after an awake method.");
            return;
        }

        translator.LoadDictionary(langCode);
        Language = langCode;

        Translate();
    }

    public void SelectNextLanguage()
    {
        Translator translator = TranslationEngine.Instance.Translator;
        string[] langs = translator.Languages.GetKeys();

        if (langs.Length == 0)
            return;

        int index = 0;
        while (index < langs.Length && !langs[index].Equals(translator.LangCulture))
        {
            index++;
        }
        index = (index + 1) % langs.Length;

        translator.LoadDictionary(langs[index]);
        Language = langs[index];

        Translate();
    }

    /// <summary>
    /// Translates all "ObjectTranslation" in the scene. This is needed 
    /// </summary>
    public void Translate()
    {
        var objects = FindObjectsOfType<ObjectTranslation>();

        foreach (var obj in objects)
        {
            var translatable = (ITranslatable)obj;
            translatable.Translate();
        }

        //	Reload UGUI
        var ugui = FindObjectOfType<UguiAutoTranslation>();
        if (ugui != null)
        {
            ugui.Reload();
        }
    }

    private List<Component> GetComponents(Type t, GameObject parent, bool recursive, bool includeInactive)
    {
        List<Component> components = new List<Component>();
        components.AddRange(parent.GetComponents(t));

        if (recursive)
        {
            components.AddRange(parent.GetComponentsInChildren(t, includeInactive));
        }

        return components;
    }

    public void AutoTranslateNgui(GameObject parent, bool recursive, bool includeInactive)
    {
#if UNITY_WSA && !UNITY_EDITOR

        Type t = Type.GetType("UILabel");
        //System.Reflection.PropertyInfo p = t.GetTypeInfo().GetDeclaredProperty("text");
        var p = WindowsReflectionHelper.GetProperty(t, "text");

        foreach (Component comp in GetComponents(t, parent, recursive, includeInactive))
        {

            string text = (p.GetValue(comp, null) ?? "").ToString().Trim();
            string key = "";

            if (text.StartsWith("{") && text.EndsWith("}"))
            {
                key = text.Remove(text.LastIndexOf("}")).Substring(1);

                TranslationInfo info = comp.gameObject.AddComponent<TranslationInfo>();
                info.key = key;
            }
            else
            {
                TranslationInfo info = comp.gameObject.GetComponent<TranslationInfo>();
                if (info != null)
                {
                    key = info.key;
                }
            }

            if (!string.IsNullOrEmpty(key))
            {
                string newText = Translator[key];
                p.SetValue(comp, newText, null);
                if (string.IsNullOrEmpty(newText))
                {
                    Debug.LogWarning("NGUI Translation: Empty string from key: '" + key + "'");
                }
            }
        }

#else

        Type t = Type.GetType("UILabel");
        System.Reflection.PropertyInfo p = t.GetProperty("text");

        foreach (Component comp in GetComponents(t, parent, recursive, includeInactive))
        {

            string text = (p.GetValue(comp, null) ?? "").ToString().Trim();
            string key = "";

            if (text.StartsWith("{") && text.EndsWith("}"))
            {
                key = text.Remove(text.LastIndexOf("}")).Substring(1);

                TranslationInfo info = comp.gameObject.AddComponent<TranslationInfo>();
                info.key = key;
            }
            else
            {
                TranslationInfo info = comp.gameObject.GetComponent<TranslationInfo>();
                if (info != null)
                {
                    key = info.key;
                }
            }

            if (!string.IsNullOrEmpty(key))
            {
                string newText = Translator[key];
                p.SetValue(comp, newText, null);
                if (string.IsNullOrEmpty(newText))
                {
                    Debug.LogWarning("NGUI Translation: Empty string from key: '" + key + "'");
                }
            }
        }
#endif
    }

    public void AutoTranslateUgui(GameObject parent, bool recursive, bool includeInactive)
    {
        AutoTraslateUguiText(parent, recursive, includeInactive);
        AutoTraslateUguiDropdown(parent, recursive, includeInactive);
    }

    private void AutoTraslateUguiText(GameObject parent, bool recursive, bool includeInactive)
    {
        List<UnityEngine.UI.Text> components = new List<UnityEngine.UI.Text>();
        components.AddRange(parent.GetComponents<UnityEngine.UI.Text>());

        if (recursive)
        {
            components.AddRange(parent.GetComponentsInChildren<UnityEngine.UI.Text>(includeInactive));
        }

        foreach (UnityEngine.UI.Text uguiText in components)
        {
            if (uguiText.gameObject.GetComponentInParent<UnityEngine.UI.Dropdown>() != null)
            {
                continue;
            }

            string text = uguiText.text;
            string result = AutoTranslateText(text, uguiText.gameObject, true);
            if (result != null)
            {
                uguiText.text = result;
            }
        }
    }

    private void AutoTraslateUguiDropdown(GameObject parent, bool recursive, bool includeInactive)
    {
        List<UnityEngine.UI.Dropdown> components = new List<UnityEngine.UI.Dropdown>();
        components.AddRange(parent.GetComponents<UnityEngine.UI.Dropdown>());

        if (recursive)
        {
            components.AddRange(parent.GetComponentsInChildren<UnityEngine.UI.Dropdown>(includeInactive));
        }

        foreach (UnityEngine.UI.Dropdown dropdown in components)
        {
            int i = 0;
            bool force = dropdown.GetComponent<TranslationInfo>() == null;
            foreach (var opt in dropdown.options)
            {
                string result = AutoTranslateText(opt.text, dropdown.gameObject, false, i++, force);
                opt.text = result;
            }

            dropdown.captionText.text = dropdown.options[dropdown.value].text;
        }
    }

    private string AutoTranslateText(string _text, GameObject go, bool singleInfo)
    {
        return AutoTranslateText(_text, go, singleInfo, 0, false);
    }

    private string AutoTranslateText(string _text, GameObject go, bool singleInfo, int index, bool forceAddInfos)
    {
        string text = (_text ?? "").Trim();
        string key = "";
        bool ignore = false;

        if (text.StartsWith("{") && text.EndsWith("}"))
        {
            key = text.Remove(text.LastIndexOf("}")).Substring(1);
            ignore = false;

            TranslationInfo info = go.AddComponent<TranslationInfo>();
            info.key = key;
        }
        else if (forceAddInfos)
        {
            TranslationInfo info = go.AddComponent<TranslationInfo>();
            key = text;
            info.key = text;
            info.ignore = true;
            ignore = true;
        }
        else if (singleInfo)
        {
            TranslationInfo info = go.GetComponent<TranslationInfo>();
            if (info != null)
            {
                key = info.key;
                ignore = info.ignore;
            }
        }
        else
        {
            TranslationInfo[] info = go.GetComponents<TranslationInfo>();
            if (info != null)
            {
                key = info[index].key;
                ignore = info[index].ignore;
            }
        }

        string result = null;

        if (!string.IsNullOrEmpty(key) || ignore)
        {
            result = ignore ? key : Translator[key];
            if (string.IsNullOrEmpty(result))
            {
                Debug.LogWarning("UGUI Translation: Empty string from key: '" + text + "'");
                result = string.Empty;
            }
        }

        return result;
    }

    void Awake()
    {
        try
        {
            if (instance == null)
            {
                instance = this;
            }

            //	CSV Initialization
            if (Format == ELangFormat.Csv)
            {
                InitializeFromCSV();
            }
            else
            {
                InitializeFromXML();
            }

            // Gets all available languages
            //selections = Translator.GetAvailableLanguages();
            // Selects the first one
            //selections[selection]
            Translator.LoadDictionary(Language);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void InitializeFromCSV()
    {
        var component = this.GetComponent<FormatCsv>();
        this.FormatComponent = component;

        // Creates a new translator
        Translator = new Translator(false);

        //	Add special references
        foreach (var re in this.GetComponents<FileReference>())
        {
            Translator.AddFileReference(re);
        }

        Translator.Init(Format, component.filename, component.delimiter, component.additionalFiles);
    }

    private void InitializeFromXML()
    {
        //	Initialization (XML)
        this.FormatComponent = null;
        // Creates a new translator
        Translator = new Translator(Format);
    }
}