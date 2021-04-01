using System;
using System.Collections.Generic;
using Translation;
using Translation.XML;
using UnityEditor;
using D = System.Diagnostics;

namespace LS.Editor
{
    public class LSEditorWindow: EditorWindow
    {
        private List<XmlLanguageDefinition> languages = null;
        protected List<XmlLanguageDefinition> Languages
        {
            get
            {
                return languages ?? (languages = XmlIO.DetectLanguages());
            }
        }

        protected void SyncKeysButton_Click()
        {
            if (EditorUtility.DisplayDialog("Sync Keys", "Do you want to synchronize the keys from all languages?", "Yes", "No"))
            {
                XMLEditorManager.Sync();
            }
        }

        protected void AddLanguageButton_Click()
        {
            XMLAddLangWindow.ShowDialog(w =>
            {
                if (w.IsOK)
                {
                    try
                    {
                        XMLEditorManager.AddLanguage(w.LangFile, w.LangCode);
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        w.CancelClosing();
                        EditorUtility.DisplayDialog("Error", ex.Message, "Ok");
                    }
                }
            });
        }

        protected void EditLanguageButton_Click(string code)
        {
            XMLAddLangWindow.ShowDialog(XMLEditorManager.GetLanguage(code), w =>
            {
                if (w.IsOK)
                {
                    try
                    {
                        XMLEditorManager.EditLanguage(code, w.LangFile, w.LangCode);
                        Clear();
                    }
                    catch (Exception ex)
                    {
                        w.CancelClosing();
                        EditorUtility.DisplayDialog("Error", ex.Message, "Ok");
                    }
                }
            });
        }

        protected void XMLLanguageButton_Click(string code)
        {
            XMLDictionaryEditorWindow.ShowDialog(XMLEditorManager.GetLanguage(code));
        }

        protected void DeleteLanguageButton_Click(string code)
        {
            if (EditorUtility.DisplayDialog("Delete " + code, "Do you want to delete the language " + code + " permanently?", "Yes", "No"))
            {
                XMLEditorManager.DeleteLanguage(code);
                Clear();
            }
        }

        protected void FeedbackButton_Click()
        {
            D.Process.Start(Constants.SupportUrl);
        }

        protected virtual void Clear()
        {
            languages = null;
        }
    }
}
