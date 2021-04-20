using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeInFadeOutController : MonoBehaviour
{
    private Material material;
    public bool isFadingIn;
    public bool isFadingOut;

    public float fadingSpeed = 1.5f;
    public float fadingMax = 1;
    public float fadingMin = 0.2f;
    public float defaultFadingValue;
    public string parameterName = "Vector1_705C2F5";
    private float FadingValue
    {
        set
        {
            material.SetFloat(parameterName, value);
        }
        get
        {
            return material.GetFloat(parameterName);
        }
    }

    private void Start()
    {
        material = GetComponent<TilemapRenderer>().material;
        if (material.shader.name != "Shader Graphs/FadeInFadeOut")
            Debug.Log(name + " besitzt falschen Shader");
        FadingValue = defaultFadingValue;
    }

    private void Update()
    {
        if (isFadingIn)
            DoFadeIn();
        else if (isFadingOut)
            DoFadeOut();
    }

    private void DoFadeIn()
    {
        FadingValue += fadingSpeed * Time.deltaTime;
        if (FadingValue >= fadingMax)
        {
            FadingValue = fadingMax;
            isFadingIn = false;
        }
    }

    private void DoFadeOut()
    {
        FadingValue -= fadingSpeed * Time.deltaTime;
        if (FadingValue <= fadingMin)
        {
            FadingValue = fadingMin;
            isFadingOut = false;
        }
    }

    public void StartFadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;
    }

    public void StartFadeOut()
    {
        isFadingOut = true;
        isFadingIn = false;
    }
}
