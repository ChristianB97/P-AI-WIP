using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogue;
using UnityEngine;

public class DialogueObjectIterator
{
    private INodeGetter currentNode;
    private IDialogueGetter currentDialogue;

    public string[] GetNextNodeBySequenceNumberAndSetItAsCurrent(int sequenceNumber)
    {
        if (!IsEndOfDialogue()&&currentNode!=null)
        {
            List<INodeGetter> childrenNodes = currentDialogue.GetAllChildrenGetter(currentNode);
            currentNode = childrenNodes[sequenceNumber];
            return GetCurrentSpeech();
        }
        return null;
    }

    public string GetCurrentName()
    {
        if (currentNode != null)
            return currentNode.GetSpeechGetter().GetName();
        return "";
    }

    public string[] GetCurrentSpeech()
    {
        if (currentNode != null)
            return currentNode.GetSpeechGetter().GetSpeech();
        return null;
    }

    public IEnumerable GetFollowUpSpeeches()
    {
        return currentNode.GetChildren();
    }

    public void SetNewDialogue(IDialogueGetter value)
    {
        if (value != null)
        {
            currentDialogue = value;
            currentNode = value.GetRootNodeGetter();
        }
    }

    public bool IsEndOfDialogue()
    {
        return currentNode != null && currentDialogue != null && !currentNode.HasChildren();
    }
}
