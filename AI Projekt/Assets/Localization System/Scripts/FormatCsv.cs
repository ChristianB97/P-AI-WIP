using System;
using UnityEngine;

/// <summary>
/// CSV Initializer.
/// </summary>
[Serializable]
public class FormatCsv : MonoBehaviour
{
    public string delimiter = ";";
    public string filename = "translation.csv";
    public string[] additionalFiles = null;
}
