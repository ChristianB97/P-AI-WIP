using UnityEngine;

/// <summary>
/// Auto translate the Game Object's instance and its children (if recursive is enabled).
/// </summary>
public class NguiAutoTranslation : MonoBehaviour
{
    public bool recursive = true;
    public bool includeInactive = true;

    void Start()
    {
        Load();
    }

    public void Reload()
    {
        Load();
    }

    private void Load()
    {
        TranslationEngine.Instance.AutoTranslateNgui(gameObject, recursive, includeInactive);
    }
}
