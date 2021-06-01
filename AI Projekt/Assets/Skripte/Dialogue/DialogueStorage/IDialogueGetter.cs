using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

public interface IDialogueGetter
{
    IEnumerable<INodeGetter> GetAllNodesGetter();
    INodeGetter GetRootNodeGetter();
    List<INodeGetter> GetAllChildrenGetter(INodeGetter parentNode);

    List<DialogueCharacterProfile> GetCharacterProfiles();
}
