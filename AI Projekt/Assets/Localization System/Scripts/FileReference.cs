using System;
using Translation;
using UnityEngine;

/// <summary>
/// Optional. Specifies a different URL for the language file.
/// </summary>
[Serializable]
public class FileReference : MonoBehaviour
{
    public EPlatform[] platforms;
    public string pathOrUrl = "";

    public FileReference()
    {
    }

    public FileReference(string pathOrUrl, params EPlatform[] platforms)
    {
        this.pathOrUrl = pathOrUrl;
        this.platforms = platforms;
    }
}
