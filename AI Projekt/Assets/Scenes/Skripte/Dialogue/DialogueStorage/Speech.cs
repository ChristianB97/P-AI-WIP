using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Speech : ISpeechGetter
{
    [SerializeField]private string name;
    [TextArea(14, 20)][SerializeField]private string[] speech = new string[1];

    public void SetName(string _name)
    {
        name = _name;
    }
    public void SetFirstSpeech(string _speech)
    {
        if (speech == null || speech.Length == 0)
            speech = new string[1];
        speech[0] = _speech;
    }

    public string GetName()
    {
        return name;
    }

    public string GetFirstSpeech()
    {
        if (speech.Length > 0)
            return speech[0];
        return "";
    }

    public string[] GetSpeech()
    {
        return speech;
    }
}
