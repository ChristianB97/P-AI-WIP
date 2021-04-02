using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public void SwitchDialogueUI(bool value)
    {
        panel.SetActive(value);
    }

    public void AddCharToText(char character)
    {
        dialogueText.SetText(dialogueText.text + character);
    }

    public void SetDialogueText(string dialogue)
    {
        dialogueText.SetText(dialogue);
    }

    public void SetCharacterNameText(string characterName)
    {
        characterNameText.SetText(characterName);
    }

    public void ResetUI()
    {
        characterNameText.SetText("");
        dialogueText.SetText("");
    }
}
