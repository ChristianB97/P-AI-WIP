using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSession
{
    private List<DialogueSessionParticipant> participants;
    private UnifiedDialogueIterator iterator;
    private IDialogueGetter dialogue;

    public DialogueSession(IDialogueGetter _dialogue)
    {
        dialogue = _dialogue;
        iterator = new UnifiedDialogueIterator(dialogue);
        participants = new List<DialogueSessionParticipant>();
        iterator.onNewText += ClearUI;
    }

    public bool AddParticipant(string name, TextMeshPro textMeshPro, DialogueCharacterProfile profile)
    {
        participants.Add(new DialogueSessionParticipant(name, textMeshPro, profile));
        ClearUI();
        return true;
    }

    public bool IsRegistrationFinished()
    {
        //List<DialogueCharacterProfile> dialogueProfiles = dialogue.GetCharacterProfiles();
        foreach (DialogueSessionParticipant participant in participants)
        {
            
        }
        return true;
    }

    public void ProceedDialogue()
    {
        if (iterator.IsIterateable() && IsRegistrationFinished())
        {
            char nextChar = iterator.IterateAndGetChar();
            foreach (DialogueSessionParticipant participant in participants)
            {
                participant.TextMeshPro.text += nextChar;
                if (nextChar == ' ')
                {
                    ProceedDialogue();
                }
            }
            
        }
    }

    public void ClearUI()
    {
        foreach (DialogueSessionParticipant participant in participants)
        {
            participant.TextMeshPro.text = "";
        }
    }
}
