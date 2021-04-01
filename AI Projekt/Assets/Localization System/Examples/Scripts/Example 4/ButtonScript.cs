using UnityEngine;
using System.Collections;
using Translation;

// Button Script - When you click the button, it will translate your GUI
//  (this is only an example, you need to add the Click detection)
public class ButtonScript : MonoBehaviour
{

    string language;
    string text;
    Translator t;

    void Awake()
    {
        this.t = TranslationEngine.Instance.Translator;
    }

    public void SetLanguage(string key)
    {
        this.language = key;
        this.text = t.Languages.Get(key);
    }

    public void OnClick()
    {
        //  Selects the language
        TranslationEngine.Instance.SelectLanguage(this.language);
    }

    public void OnGUI()
    {
        if (t.SelectedLanguage.Equals(language))
        {
            GUI.Box(new Rect(transform.position.x, transform.position.y, 100f, 40f), "* " + this.text);
        }
        else if (GUI.Button(new Rect(transform.position.x, transform.position.y, 100f, 40f), this.text))
        {
            OnClick();
        }
    }
}