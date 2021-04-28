using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dialogue;

public class NPCDialogue : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    private DialogueReader reader;
    public Dialogue.Dialogue dialogue;

    private void Start()
    {
        reader = new DialogueReader(textMeshPro);
        StartDialogue(dialogue);

    }

    private void Update()
    {
        if (reader.IsUpdateable())
        {
            reader.UpdateDialogue(Time.deltaTime);
        }
    }

    public void StartDialogue(Dialogue.Dialogue _dialogue)
    {
        reader.SetDialog(_dialogue);
    }
}
