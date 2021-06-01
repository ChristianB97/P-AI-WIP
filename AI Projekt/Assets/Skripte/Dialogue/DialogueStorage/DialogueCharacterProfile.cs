using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "DialogueCreation/CharacterProfile")]
public class DialogueCharacterProfile : ScriptableObject
{
    public int id;
    public string characterName;
    public Sprite picture;
    public Color color;
    //talking Sound
}


