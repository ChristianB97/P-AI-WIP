using UnityEngine;
using Dialogue;
using TMPro;

public class DialogueReader
{
    public TextMeshPro textMeshPro;
    public DialogueReaderIteration iterator;
    public bool IsDialogue { private set; get; }
    public Dialogue.Dialogue dialogue;
    public float maxCharTime = 0.1f;
    private float currCharTime;
    public float maxPauseTime = 2f;
    private float currPauseTime;

    public DialogueReader(TextMeshPro _textMeshPro)
    {
        textMeshPro = _textMeshPro;
        iterator = new DialogueReaderIteration();
    }

    public void UpdateDialogue(float deltaTime)
    {
        if (IsUpdateable())
        {
            if (currCharTime < maxCharTime)
                currCharTime += deltaTime;
            else if (iterator.IsEndOfStringIteration())
            {
                if (currPauseTime < maxPauseTime)
                    currPauseTime += deltaTime;
                else
                {
                    textMeshPro.text = "";
                    currPauseTime = 0;
                    iterator.Iterate();
                }
                    
            }
            else
            {
                AddCharacterToUI();
                currCharTime = 0;
            }
        }
    }

    public bool IsUpdateable()
    {
        return IsDialogue;
    }

    private void AddCharacterToUI()
    {
        char character = iterator.GetCharAndIterateString();
        textMeshPro.text += character;
        if (character == ' ')
        {
            character = iterator.GetCharAndIterateString();
            textMeshPro.text += character;
        }
    }

    public void SetDialog(IDialogueGetter _dialogue)
    {
        textMeshPro.enabled = true;
        textMeshPro.text = "";
        iterator.PrepareDialogueObject(_dialogue);
        currCharTime = maxCharTime;
        IsDialogue = true;
    }

    public void MoveOnWithDialogue()
    {
        bool isIterationSuccessful = iterator.Iterate();
        if (!isIterationSuccessful)
            EndDialog();
    }

    private void EndDialog()
    {
        textMeshPro.enabled = false;
        IsDialogue = false;
    }
}
