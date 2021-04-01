using LS.Editor;
using System.Collections;
using System.Collections.Generic;
using Translation;
using Translation.XML;
using UnityEditor;
using UnityEngine;

public class XMLEditorWindow : LSEditorWindow
{
    private bool compiling = false;
    private Vector2 sectionEditorScroll = Vector2.zero;

    protected void Awake()
    {
        this.titleContent = new GUIContent("XML Editor - Localization System");
    }

    protected void OnGUI()
    {

        EditorGUILayout.BeginVertical();

        SectionTitle();
        EditorGUILayout.Separator();

        if (EditorApplication.isCompiling)
        {
            GUILayout.Label("Waiting for compilation...");
            compiling = true;
        }
        else
        {
            if (compiling)
            {
                compiling = false;
                Clear();
            }

            SectionEditor();

            GUILayout.FlexibleSpace();

            SectionButtons();

        }

        EditorGUILayout.EndVertical();
    }

    protected void OnInspectorUpdate()
    {
        this.Repaint();
    }

    private void SectionEditor()
    {
        sectionEditorScroll = EditorGUILayout.BeginScrollView(sectionEditorScroll);

        foreach (var lang in Languages)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(lang.Code);

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Edit"))
            {
                EditLanguageButton_Click(lang.Code);
            }
            if (GUILayout.Button("XML"))
            {
                XMLLanguageButton_Click(lang.Code);
            }
            if (GUILayout.Button("Delete"))
            {
                DeleteLanguageButton_Click(lang.Code);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void SectionTitle()
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("General Configuration");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add language"))
        {
            AddLanguageButton_Click();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void SectionButtons()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Sync keys"))
        {
            SyncKeysButton_Click();
        }

        if (GUILayout.Button("Refresh"))
        {
            this.Clear();
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Support"))
        {
            FeedbackButton_Click();
        }

        EditorGUILayout.EndHorizontal();
    }
}
