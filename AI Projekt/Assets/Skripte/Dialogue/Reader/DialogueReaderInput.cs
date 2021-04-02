using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueReader))]
public class DialogueReaderInput : MonoBehaviour
{
    private DialogueReader dialogueReader;
    private void Start()
    {
        dialogueReader = GetComponent<DialogueReader>();
    }

    private void Update()
    {
        if (dialogueReader.IsDialogue)
        {
            if (Input.GetButtonDown("Jump"))
            {
                dialogueReader.MoveOnWithDialogue();
            }
        }
    }
}
