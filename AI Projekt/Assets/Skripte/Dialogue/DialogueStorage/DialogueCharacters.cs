using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "DialogueCreation/Characters")]
public class DialogueCharacters : ScriptableObject
{
    public List<DialogueCharacterProfile> profiles = new List<DialogueCharacterProfile>();
    public Dictionary<string, DialogueCharacterProfile> profileLookUp;
    public string[] characterNames;

    public void Awake()
    {
        OnValidate();
    }

    public int GetIdByNameElseCreateName(string _name)
    {
        if (!profileLookUp.ContainsKey(_name))
        {
            DialogueCharacterProfile profile = new DialogueCharacterProfile();
            profile.name = _name;
            profiles.Add(profile);
            OnValidate();
            Debug.Log(_name + " was not added to your characters. " + _name + " added!");
        }
        if (profileLookUp.ContainsKey(_name))
        {
            return profileLookUp[_name].id;
        }
        return -1;
    }

    public void OnValidate()
    {
        if (profileLookUp == null)
            profileLookUp = new Dictionary<string, DialogueCharacterProfile>();
        else
            profileLookUp.Clear();

        characterNames = new string[profiles.Count];
        for (int i = 0; i < profiles.Count; i++)
            if (profiles[i] != null)
            {
                profiles[i].id = i;
                profileLookUp.Add(profiles[i].name, profiles[i]);
                characterNames[i] = profiles[i].name;
            }     
    }
}
