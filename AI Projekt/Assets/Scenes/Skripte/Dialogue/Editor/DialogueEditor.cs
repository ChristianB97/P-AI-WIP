#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace Dialogue.Editor 
{ 
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] Vector2 draggingOffset;
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode deletingNode = null;
        DialogueNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized] bool draggingCanvas = false;
        Vector2 draggingCanvasOffset;
        int canvasSize = 10000;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceId) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnGUI()
        {
            if (selectedDialogue)
            {
                ProcessEvents();
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                Rect background = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                EditorGUILayout.EndScrollView();
                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }



        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                    
                else 
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        

        private DialogueNode GetNodeAtPoint(Vector2 mousePosition)
        {
            DialogueNode lastFoundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                if (node.GetRect().Contains(mousePosition))
                    lastFoundNode = node;
            return lastFoundNode;
        }

        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginArea(node.GetRect(), nodeStyle);

            DrawPopUp(node);

            node.SetFirstSpeech(EditorGUILayout.TextField(node.GetFirstSpeech()));

            DrawButtons(node);

            GUILayout.EndArea();
            GUILayout.EndVertical();
        }

        private void DrawPopUp(DialogueNode node)
        {
            if (selectedDialogue.dialogueCharacters)
            {
                CheckAndFixCorruptPopupID(node);
                string[] characterNames = selectedDialogue.dialogueCharacters.characterNames;
                node.SetPopUpId(EditorGUILayout.Popup(node.GetPopUpId(), characterNames));
                if (node.GetPopUpId() >= 0)
                {
                    node.SetCharacterName(characterNames[node.GetPopUpId()]);
                }
                
            }
        }

        private void CheckAndFixCorruptPopupID(DialogueNode node)
        {
            string[] characterNames = selectedDialogue.dialogueCharacters.characterNames;
            if (characterNames.Length <= node.GetPopUpId())
                node.SetPopUpId(characterNames.Length - 1);
            if (node.GetPopUpId() >= 0 && node.GetSpeechGetter().GetName() != characterNames[node.GetPopUpId()])
                node.SetPopUpId(selectedDialogue.dialogueCharacters.GetIdByNameElseCreateName(node.GetSpeechGetter().GetName()));
        }

        private DialogueNode DrawButtons(DialogueNode node)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("DELETE"))
            {
                deletingNode = node;
            }
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (node == linkingParentNode)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.ContainsChild(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null;
                }
            }
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            GUILayout.EndHorizontal();
            return node;
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 1;
                Handles.DrawBezier(
                    startPosition, endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white, null, 4f);

                int arrowHeadLineSize = 10;

                Vector3 arrowHeadTopA = new Vector2(endPosition.x, endPosition.y - 1);
                Vector3 arrowHeadTopB = new Vector2(endPosition.x, endPosition.y + 1);
                Vector3 arrowHeadPosA = new Vector2(endPosition.x - arrowHeadLineSize, endPosition.y + arrowHeadLineSize);
                Vector3 arrowHeadPosB = new Vector2(endPosition.x - arrowHeadLineSize, endPosition.y - arrowHeadLineSize);

                Handles.DrawBezier(arrowHeadTopA, arrowHeadPosA, arrowHeadTopA, arrowHeadPosA, Color.white, null, 4f);
                Handles.DrawBezier(arrowHeadTopB, arrowHeadPosB, arrowHeadTopB, arrowHeadPosB, Color.white, null, 4f);
            }

        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChange;
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(20, 20, 20, 20);

        }

        private void OnSelectionChange()
        {
            Dialogue dialogue = Selection.activeObject as Dialogue;
            if (dialogue != null)
            {
                selectedDialogue = dialogue;
                Repaint();
            }
        }
    }
}

#endif