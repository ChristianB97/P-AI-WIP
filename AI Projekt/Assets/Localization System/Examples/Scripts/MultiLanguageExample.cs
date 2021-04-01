using UnityEngine;
using UnityEngine.UI;

public class MultiLanguageExample : MonoBehaviour
{
    public Text welcomeEs;
    public Text welcomeEn;

    void Start()
    {
        var newTranslator = TranslationEngine.Instance.Translator.Clone();

        newTranslator.LoadDictionary("es-AR");
        welcomeEs.text = newTranslator["welcome"];

        newTranslator.LoadDictionary("en-US");
        welcomeEn.text = newTranslator["welcome"];
    }
}
