using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Translation;
using Translation.XML;
using UnityEditor;
using UnityEngine;

namespace LS.Editor
{
    public sealed class XMLDictionaryEditorWindow : LSToolWindow
    {
        private bool dirty = false;
        private Vector2 sectionEditorScroll = Vector2.zero;
        private XmlLangFileInfo info = null;
        public XmlLangFileInfo Info 
        {
            get
            {
                return info;
            }

            private set
            {
                info = value;
                Lines = new List<XmlWordEditorLine>();
                Lines.AddRange(Info.Words.GetKeys().Select(m => new XmlWordEditorLine(m, Info.Words.Get(m))));
                if (Lines.Count == 0)
                {
                    Lines.Add(new XmlWordEditorLine());
                }
            }
        }

        public List<XmlWordEditorLine> Lines { get; private set; }

        private List<XmlWordEditorLine> linesToDelete;

        protected override void Awake()
        {
            sectionEditorScroll = Vector2.zero;
            dirty = false;
            titleContent = new GUIContent("XML Editor");
            linesToDelete = new List<XmlWordEditorLine>();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            SectionTitle();

            SectionEditor();

            GUILayout.FlexibleSpace();

            SectionButtons();

            EditorGUILayout.EndVertical();
        }

        private void SectionTitle()
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("XML Editor - Language: " + Info.Code);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Add word"))
            {
                AddWord_Click();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void SectionEditor()
        {
            if (Lines == null)
                return;

            string oldTitle = Info.Title;
            Info.Title = EditorGUILayout.TextField("Title", Info.Title);
            EditorGUILayout.Separator();

            sectionEditorScroll = EditorGUILayout.BeginScrollView(sectionEditorScroll);

            int i = 1; 
            foreach (var line in Lines)
            {
                SubSectionLine(i++, line);
            }

            if (linesToDelete.Count > 0)
            {
                linesToDelete.ForEach(d => Lines.Remove(d));
                linesToDelete.Clear();
            }

            EditorGUILayout.EndScrollView();

            if (oldTitle != Info.Title)
            {
                dirty = true;
            }
        }

        private void SubSectionLine(int lineNumber, XmlWordEditorLine line)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(lineNumber.ToString() + ((line.IsNew || line.Dirty) ? "*" : ""), GUILayout.Width(28));

            line.Key = EditorGUILayout.TextField(line.Key, GUILayout.Width(100));
            EditorGUILayout.Space();
            line.Value = EditorGUILayout.TextField(line.Value, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("Del"))
            {
                if (EditorUtility.DisplayDialog("Delete", "Do you want to delete this line?", "Yes", "No"))
                {
                    linesToDelete.Add(line);
                    dirty = true;
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();

            if (line.Dirty)
            {
                dirty = true;
            }

        }

        private void SectionButtons()
        {
            EditorGUILayout.BeginHorizontal();

            if (dirty && GUILayout.Button("Save", GUILayout.MinWidth(40)))
            {
                Info.Words = new Dict();
                foreach (var line in Lines)
                {
                    Info.Words.Set(line.Key, line.Value);
                    line.Save();
                }

                XMLEditorManager.SaveLanguage(Info);
                AssetDatabase.Refresh();

                dirty = false;
            }

            if (dirty && GUILayout.Button("Reload", GUILayout.MinWidth(40)))
            {
                dirty = false;
                Info = XmlIO.ImportFromXML(Info.File);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Close", GUILayout.MinWidth(40)))
            {
                dirty = false;
                CancelDialog();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void AddWord_Click()
        {
            sectionEditorScroll = Vector2.zero;
            Lines.Insert(0, new XmlWordEditorLine());
        }

        private void OnInspectorUpdate()
        {
            Repaint();

            if (Lines == null)
                this.Close();
        }

        public static void ShowDialog(XmlLangFileInfo info)
        {
            XMLDictionaryEditorWindow window = EditorWindow.GetWindow<XMLDictionaryEditorWindow>(true);
            window.Info = info;
        }

        public static void ShowDialog(XmlLanguageDefinition def)
        {
            ShowDialog(XmlIO.ImportFromXML(def.File));
        }
    }
}
