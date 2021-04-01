using UnityEngine;

public class MSE_LanguageSettings : MonoBehaviour
{
    public string currentLanguage = "";

    void Start()
    {
        ChangeLanguage(PlayerPrefs.HasKey("language") ? PlayerPrefs.GetString("language") : TranslationEngine.Instance.Language);
    }

    public void ChangeLanguage(string lang)
    {
        if (string.IsNullOrEmpty(lang))
            return;

        currentLanguage = lang;
        TranslationEngine.Instance.SelectLanguage(currentLanguage);
        SaveLanguage();
    }

    public void SaveLanguage()
    {
        PlayerPrefs.SetString("language", currentLanguage);
    }
}
