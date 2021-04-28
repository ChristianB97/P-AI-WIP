using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightTargetSwitch : MonoBehaviour
{
    public Light2D lightSource;
    public string tag1;
    public string layer1;
    public string tag2;
    public string layer2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tag1))
        {

        }
    }
}
