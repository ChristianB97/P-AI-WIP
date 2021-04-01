using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dialogue
{
    public class DialogueNode : ScriptableObject, INodeGetter
    {
        [SerializeField]private Speech speech = new Speech();
        [SerializeField]private List<string> children = new List<string>();
        [SerializeField]private Rect usedRect = new Rect(100, 100, 200, 100);
        private Rect defaultRect = new Rect(100, 100, 200, 100);
        [SerializeField] private int popUpId = -1;



        public int GetPopUpId()
        {
            return popUpId;
        }

        public IEnumerable GetChildren()
        {
            return children;
        }

        public bool HasChildren()
        {
            return children != null && children.Count != 0;
        }

        public Rect GetRect()
        {
            return usedRect;
        }

        public bool ContainsChild(string child)
        {
            return children.Contains(child);
        }

        public void AddChild(string child)
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "New Child");
            #endif

            children.Add(child);

            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
        public void RemoveChild(string child)
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "Removed Child");
            #endif
            children.Remove(child);
            
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }


        public void SetCharacterName(string _character)
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "New Character");
            #endif
            speech.SetName(_character);
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void SetFirstSpeech(string newText)
        {
            if (newText != speech.GetFirstSpeech())
            {
                #if UNITY_EDITOR
                Undo.RecordObject(this, "Text Change");
                #endif
                speech.SetFirstSpeech(newText);
                #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                #endif
            }
        }

        public void ExpandRectBy(Vector2 summand)
        {
            usedRect.size = defaultRect.size + summand;
        }

        public void SetPopUpId(int _id)
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "New Character");
            #endif
            popUpId = _id;
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public void SetPosition(Vector2 newPosition)
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "New Position");
            #endif
            usedRect.position = newPosition;
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        public ISpeechGetter GetSpeechGetter()
        {
            return speech;
        }

        public string GetFirstSpeech()
        {
            return speech.GetFirstSpeech();
        }
    }
}