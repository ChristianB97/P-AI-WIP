using UnityEngine;
using Translation;
using System;

/// <summary>
/// CSV Using Code.
/// </summary>
public class ExampleClass3 : MonoBehaviour
{
    #region Language Translator
    /* Translator */
    private Translator translator;
    protected Translator Translator
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
    #endregion

    #region Some GUI attributes
    private int prevSel = 0;
    private int selection = 0;
    private string[] selections;

    public float X = 0;
    public float Y = 0;

    string error = "";

    bool flashFix = false;
    #endregion

    void Start()
    {
        try
        {
            // Create a new translator
            Translator = new Translator(ELangFormat.Csv, "Example CSV/languages", ";");

            // Get all available languages
            selections = Translator.GetAvailableLanguages();

            // Select the first one
            Translator.LoadDictionary(selections[selection]);
        }
        catch (Exception ex)
        {
            error = ex.Message;
            Translator = null;
        }
    }

    void OnGUI()
    {
        if (Translator == null || selections == null || selections.Length == 0)
        {
            GUI.Label(new Rect(X, Y, Screen.width - X, 50), "Error, no se ha podido cargar el traductor. " + error);
            if (!flashFix) // Fighting with Flash
            {
                try
                {
                    Translator = new Translator(true);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                finally
                {
                    flashFix = true;
                }
            }
        }
        else
        {
            // Using Translator["key"], translates a string
            GUI.Label(new Rect(X, Y, 300, 20), Translator["hello"]);
            GUI.Label(new Rect(X, Y + 25, 300, 20), string.Format(Translator["my-name"], Translator.LangTitle));

            selection = GUI.SelectionGrid(new Rect(10, 100, Screen.width - 20, 50), selection, selections, selections.Length);

            if (prevSel != selection)
            {
                prevSel = selection;
                // Select another language
                Translator.LoadDictionary(selections[selection]);
            }
        }
    }

}
