using UnityEngine;
using Dialogue;

public class DialogueReader : MonoBehaviour
{
    public DialogueUI dialogueUI;
    private DialogueReaderIteration iterator;
    public bool IsDialogue { private set; get; }
    public Dialogue.Dialogue dialogue;
    public float maxCharTime;
    private float currCharTime;

    void Start()
    {
        iterator = new DialogueReaderIteration();
        iterator.onNewActor += UpdateCharacterUI;
        iterator.onNewText += UpdateTextUI;
        if (dialogueUI == null)
            dialogueUI = FindObjectOfType<DialogueUI>();
        else
            print("No DialogeUI is attached in this scene");
        StartDialog(dialogue);
    }

    private void Update()
    {
        if (IsDialogue && !iterator.IsEndOfStringIteration())
        {
            if (currCharTime < maxCharTime)
                currCharTime += Time.deltaTime;
            else
            {
                AddCharacterToUI();
                currCharTime = 0;
            }
        }
    }

    private void AddCharacterToUI()
    {
        char character = iterator.GetCharAndIterateString();
        dialogueUI.AddCharToText(character);
        if (character == ' ')
        {
            character = iterator.GetCharAndIterateString();
            dialogueUI.AddCharToText(character);
        }
    }

    private void UpdateCharacterUI(string character)
    {
        dialogueUI.SetCharacterNameText(character);
    }

    private void UpdateTextUI(string text)
    {
        dialogueUI.SetDialogueText(text);
    }

    public void StartDialog(IDialogueGetter _dialogue)
    {
        dialogueUI.SwitchDialogueUI(true);
        dialogueUI.ResetUI();
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
        dialogueUI.SwitchDialogueUI(false);
        IsDialogue = false;
    }
}
