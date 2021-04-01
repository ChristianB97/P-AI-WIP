using UnityEngine;

public class ExampleClass2 : MonoBehaviour
{
    void Start()
    {
        Play();
    }

    void OnGUI()
    {

        if (TranslationEngine.Instance.Translator == null)
            return;

        if (GUI.Button(new Rect(10, 10, 80, 30), TranslationEngine.Instance.Translator["change"]))
        {
            // Change the language
            TranslationEngine.Instance.SelectNextLanguage();

            // Play audio clip
            Play();
        }
    }

    void Play()
    {
        // Get the AudioTranslation component from "AudioContainer".
        AudioTranslation audio = GameObject.Find("AudioContainer").GetComponent<AudioTranslation>();
        // And play
        audio.Play();
    }
}
