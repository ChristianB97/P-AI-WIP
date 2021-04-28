using System;

public class DialogueReaderIteration
{
    private StringIterator stringIterator;
    private StringArrayIterator stringArrayIterator;
    private DialogueObjectIterator dialogueIterator;
    public Action<string> onNewActor;
    public Action<string> onNewText;

    public DialogueReaderIteration()
    {
        stringIterator = new StringIterator();
        stringArrayIterator = new StringArrayIterator();
        dialogueIterator = new DialogueObjectIterator();
    }

    public void PrepareDialogueObject(IDialogueGetter _dialogue)
    {
        dialogueIterator.SetNewDialogue(_dialogue);
        onNewActor?.Invoke(dialogueIterator.GetCurrentName());
        stringArrayIterator.SetNewStringArray(dialogueIterator.GetCurrentSpeech());
        SetNextString();
    }

    private void SetNextString()
    {
        string nextString = stringArrayIterator.GetStringAndIterate();
        stringIterator.SetNewString(nextString);
        onNewText?.Invoke("");
    }

    private void SetNextStringArray()
    {
        string[] nextStringArray = dialogueIterator.GetNextNodeBySequenceNumber(0);
        onNewActor?.Invoke(dialogueIterator.GetCurrentName());
        stringArrayIterator.SetNewStringArray(nextStringArray);
    }

    public char GetCharAndIterateString()
    {
        return stringIterator.GetCharAndIterate();
    }

    public bool Iterate()
    {
        if (!stringArrayIterator.IsEndOfStringArray())
        {
            SetNextString();
            return true;
        }
            
        else if (!dialogueIterator.IsEndOfDialogue())
        {
            SetNextStringArray();
            SetNextString();
            return true;
        }
        return false;
    }

    public bool IsEndOfStringIteration()
    {
        return stringIterator.IsEndOfString();
    }
}
