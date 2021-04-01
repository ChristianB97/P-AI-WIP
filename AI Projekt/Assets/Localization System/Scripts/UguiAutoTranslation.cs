using UnityEngine;
using System.Collections;

/// <summary>
/// Auto translate the Game Object's instance and its children (if recursive is enabled).
/// </summary>
public class UguiAutoTranslation : MonoBehaviour {

	public bool recursive = true;
	public bool includeInactive = true;

	void Start () 
    {
		TranslationEngine.Instance.AutoTranslateUgui(gameObject, recursive, includeInactive);
	}

	public void Reload() 
    {
		TranslationEngine.Instance.AutoTranslateUgui(gameObject, recursive, includeInactive);
	}
}
