using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodeGetter
{
    IEnumerable GetChildren();
    bool ContainsChild(string child);
    bool HasChildren();

    ISpeechGetter GetSpeechGetter();
}
