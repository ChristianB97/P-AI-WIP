using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
public class StringMultipleMasksAttribute : PropertyAttribute
{
    private string[] _maskNames;

    public void OnGUI(Rect position,
                            SerializedProperty property,
                            GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        uint a = (uint)(EditorGUI.MaskField(position, label, property.intValue, property.enumNames));
        if (EditorGUI.EndChangeCheck())
        {
            property.intValue = (int)a;
        }
    }

    public string[] MaskNames { get { return _maskNames; } }
}
