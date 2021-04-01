using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueCreation/Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver, IDialogueGetter
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup;
        public DialogueCharacters dialogueCharacters;

#if UNITY_EDITOR
        private void Awake()
        {

            OnValidate();
        }
#endif

        public void OnValidate()
        {
            nodeLookup = new Dictionary<string, DialogueNode>();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }


        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public List<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            List<DialogueNode> result = new List<DialogueNode>();
            foreach (string childID in parentNode.GetChildren())
                if (nodeLookup.ContainsKey(childID))
                    result.Add(nodeLookup[childID]);
            return result;
        }

        private static DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();

            if (parent != null)
            {
                parent.AddChild(newNode.name);
                int parrentDistance = 30;
                newNode.SetPosition(new Vector2(parent.GetRect().position.x + parent.GetRect().width + parrentDistance, parent.GetRect().position.y));
            }

            return newNode;
        }

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);

            #if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");
            #endif

            AddNode(newNode);
        }

        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);

            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            #if UNITY_EDITOR
            Undo.RecordObject(this, "Deleted Dialogue Node");
            #endif

            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);

            #if UNITY_EDITOR
            Undo.DestroyObjectImmediate(nodeToDelete);
            #endif
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
                node.RemoveChild(nodeToDelete.name);
        }

        public void OnBeforeSerialize()
        {
            #if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
            #endif
        }

        public void OnAfterDeserialize()
        {
        }

        public IEnumerable<INodeGetter> GetAllNodesGetter()
        {
            return GetAllNodes();
        }

        public INodeGetter GetRootNodeGetter()
        {
            return GetRootNode();
        }

        public List<INodeGetter> GetAllChildrenGetter(INodeGetter parentNode)
        {
            List<INodeGetter> result = new List<INodeGetter>();
            foreach (string childID in parentNode.GetChildren())
                if (nodeLookup.ContainsKey(childID))
                    result.Add(nodeLookup[childID]);
            return result;
        }
    }
}

