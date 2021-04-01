using System;
using Translation.XML;
using UnityEditor;
using UnityEngine;

namespace LS.Editor
{
    public sealed class XMLAddLangWindow : LSToolWindow
    {
        public bool EditMode { get; set; }
        public string LangCode { get; set; }
        public string LangFile { get; set; }

        private Action<XMLAddLangWindow> FinishCallback { get; set; }

        protected override void Awake()
        {
            base.Awake();

            titleContent = new GUIContent((EditMode ? "Edit" : "Add") + " language");
            LangCode = "";
            LangFile = "";
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label(EditMode ? "Edit language" : "Add a new language");

            SectionEditor();

            GUILayout.FlexibleSpace();

            SectionButtons();

            EditorGUILayout.EndVertical();
        }

        private void SectionEditor()
        {
            LangFile = EditorGUILayout.TextField("Name", LangFile);
            LangCode = EditorGUILayout.TextField("Culture", LangCode);
        }

        private void SectionButtons()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("OK", GUILayout.MinWidth(40)))
            {
                AcceptDialog();
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Cancel", GUILayout.MinWidth(40)))
            {
                CancelDialog();
            }

            EditorGUILayout.EndHorizontal();
        }

        protected override void ExecuteExitCallback()
        {
            if (FinishCallback != null)
            {
                FinishCallback(this);
                FinishCallback = null;
            }
        }

        private void OnInspectorUpdate()
        {
            Repaint();

            if (EditorApplication.isCompiling)
                this.Close();
        }

        public static void ShowDialog(Action<XMLAddLangWindow> callback)
        {
            XMLAddLangWindow window = EditorWindow.GetWindow<XMLAddLangWindow>(true);
            window.EditMode = false;
            window.FinishCallback = callback;
        }

        public static void ShowDialog(XmlLanguageDefinition def, Action<XMLAddLangWindow> callback)
        {
            XMLAddLangWindow window = EditorWindow.GetWindow<XMLAddLangWindow>(true);
            window.EditMode = true;

            if (def != null)
            {
                window.LangCode = def.Code;
                window.LangFile = def.File;
            }
            else
            {
                Debug.LogWarning("Cannot find the reference for " + def.File + " " + def.Code);
            }

            window.FinishCallback = callback;
        }

    }
}
