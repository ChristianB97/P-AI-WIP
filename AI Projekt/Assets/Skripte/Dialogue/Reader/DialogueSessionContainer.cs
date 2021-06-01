using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "DialogueSessionContainer", menuName = "Data/DialogueSessionContainer")]
public class DialogueSessionContainer : ScriptableObject
{
    private Dictionary<string, DialogueSession> sessions;
    private Metronome metronome;
    [SerializeField]private float tickTime;

    public void CreateSession(string sessionID, IDialogueGetter dialogue)
    {
        if (metronome == null)
            CreateMetronome();
        if (sessions == null)
            sessions = new Dictionary<string, DialogueSession>();
        sessions.Add(sessionID, new DialogueSession(dialogue));
    }

    public bool JoinSession(string sessionID, string characterName, TextMeshPro textMeshPro, DialogueCharacterProfile profile)
    {
        if (sessions.ContainsKey(sessionID))
        {
            bool isParticipantValid = sessions[sessionID].AddParticipant(characterName, textMeshPro, profile);
            return isParticipantValid;
        }
        return false;
    }

    public void EndSession(string sessionID)
    {
        sessions.Remove(sessionID);
    }

    public void CreateMetronome()
    {
        GameObject newObject = new GameObject();
        metronome = newObject.AddComponent<Metronome>();
        metronome.SetDurationWithoutTick(tickTime);
        metronome.onTick += ProceedDialogues;
    }

    private void ProceedDialogues()
    {
        foreach (KeyValuePair<string, DialogueSession> entry in sessions)
        {
            entry.Value.ProceedDialogue();
        }
    }

    public void FlushSessions()
    {
        sessions = new Dictionary<string, DialogueSession>();
    }
}
