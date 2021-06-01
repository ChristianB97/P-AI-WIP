using UnityEngine;
using Dialogue;
using TMPro;

public class DialogueReader : MonoBehaviour
{
    public DialogueSessionContainer dialogueSessionContainer;
    private TextMeshPro tmpro;
    public Dialogue.Dialogue dialogue;

    private void Start()
    {
        tmpro = gameObject.AddComponent<TextMeshPro>();
        dialogueSessionContainer.CreateSession("idReady", dialogue);
    }


}
