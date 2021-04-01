using UnityEngine;
using UnityEngine.UI;

public class MSE_LanguageDropdown : MonoBehaviour
{
    public Dropdown dropdown;

    public void OnChange()
    {
        FindObjectOfType<MSE_LanguageSettings>().ChangeLanguage(dropdown.value == 0 ? "en-US" : "es-AR");
    }

    public void Refresh()
    {
        if (TranslationEngine.Instance.Language.Contains("es"))
        {
            dropdown.value = 1;
        }
        else
        {
            dropdown.value = 0;
        }
    }
}
