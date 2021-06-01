using System;

public class UnifiedDialogueIterator
{
    private StringIterator stringIterator;
    private StringArrayIterator stringArrayIterator;
    private DialogueObjectIterator dialogueIterator;
    public Action<string> onNewActor;
    public Action onNewText;

    public UnifiedDialogueIterator(IDialogueGetter dialogue)
    {
        stringIterator = new StringIterator();
        stringArrayIterator = new StringArrayIterator();
        dialogueIterator = new DialogueObjectIterator();
        dialogueIterator.SetNewDialogue(dialogue);
        stringArrayIterator.SetNewStringArray(dialogueIterator.GetCurrentSpeech());
        stringIterator.SetNewString(stringArrayIterator.GetStringAndIterate());
    }

    public char IterateAndGetChar()
    {
        if (!stringIterator.IsEndOfString())
        {
            return stringIterator.GetCharAndIterate();
        }
        else if (!stringArrayIterator.IsEndOfStringArray())
        {
            SetNextStringInStringIterator();
            return stringIterator.GetCharAndIterate();
        }
        else if (!dialogueIterator.IsEndOfDialogue())
        {
            SetNextStringArrayInStringArrayIterator();
            SetNextStringInStringIterator();
            return stringIterator.GetCharAndIterate();
        }
        else
        {
            return ' ';
        }
    }

    private void SetNextStringArrayInStringArrayIterator()
    {
        string[] newStringArray = dialogueIterator.GetNextNodeBySequenceNumberAndSetItAsCurrent(0);
        stringArrayIterator.SetNewStringArray(newStringArray);
        onNewActor?.Invoke(dialogueIterator.GetCurrentName());
    }

    private void SetNextStringInStringIterator()
    {
        string nextString = stringArrayIterator.GetStringAndIterate();
        stringIterator.SetNewString(nextString);
        onNewText?.Invoke();
    }

    public bool IsIterateable()
    {
        return stringIterator.IsEndOfString() || stringArrayIterator.IsEndOfStringArray() || dialogueIterator.IsEndOfDialogue();
    }
}
