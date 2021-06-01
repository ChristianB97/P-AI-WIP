using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField]private DialogueSessionContainer sessionContainer;
    [SerializeField]private TextMeshPro tmpro;
    [SerializeField] private Dialogue.Dialogue dialogue;
    [SerializeField] private DialogueCharacterProfile profile;

    private void Start()
    {
        sessionContainer.CreateSession("faulheit", dialogue);
        sessionContainer.JoinSession("faulheit", "Dieter", tmpro, profile);
    }

}
