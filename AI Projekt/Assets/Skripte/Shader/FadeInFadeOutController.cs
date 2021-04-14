using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeInFadeOutController : MonoBehaviour
{
    private Material material;
    private void Start()
    {
        material = GetComponent<TilemapRenderer>().material;
        if (material.shader.name != "FadeInFadeOut")
            Debug.Log(name + " besitzt falschen Shader");
    }
}
