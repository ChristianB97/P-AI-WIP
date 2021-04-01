using System;
using System.Collections.Generic;
using Translation;
using UnityEngine;

public class ObjectTranslation : MonoBehaviour, ITranslatable
{
    #region Required objects

    public enum TranslationType
    {
        ObjectActivation
    }

    [Serializable]
    public class Translatable
    {
        public string language = "en-US";
        public GameObject translatable = null;
    }

    #endregion

    public TranslationType mode = TranslationType.ObjectActivation;
    public Translatable[] items;

    private List<Translatable> dynamicTranslatables = null;

    void Start()
    {
        Translate();
    }

    public void Translate()
    {
        Translate(items);

        if (dynamicTranslatables != null)
        {
            Translate(dynamicTranslatables);
        }
    }

    /// <summary>
    /// Adds translatables objects dynamically.
    /// </summary>
    /// <param name="item"></param>
    public void AddTranslatable(Translatable item)
    {
        if (dynamicTranslatables == null)
        {
            dynamicTranslatables = new List<Translatable>();
        }

        dynamicTranslatables.Add(item);

        Translate();
    }

    private void Translate(IEnumerable<Translatable> items)
    {
        var language = TranslationEngine.Instance.Language;
        foreach (Translatable item in items)
        {
            if (mode == TranslationType.ObjectActivation)
            {
                item.translatable.SetActive(item.language.Equals(language));
            }
        }
    }

}
