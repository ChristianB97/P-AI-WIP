using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSessionParticipant
{
    public string ParticipantName { private set; get; }
    public TextMeshPro TextMeshPro { private set; get; }

    public DialogueCharacterProfile Profile { private set; get; }

    public DialogueSessionParticipant(string _participantName, TextMeshPro _textMeshPro, DialogueCharacterProfile _profile)
    {
        Profile = _profile;
        ParticipantName = _participantName;
        TextMeshPro = _textMeshPro;
    }
}
