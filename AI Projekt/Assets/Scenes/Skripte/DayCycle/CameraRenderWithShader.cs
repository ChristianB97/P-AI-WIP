using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRenderWithShader : MonoBehaviour
{
    private Camera currentCamera;
    public Shader shader;
    public string targetTag;

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            currentCamera = gameObject.GetComponent<Camera>();
            if (currentCamera != null)
            {
                currentCamera.enabled = true;
                if (shader != null)
                {
                    currentCamera.SetReplacementShader(shader, "");
                    currentCamera.Render();
                }
                else
                    Debug.Log("No shader attached!");
                currentCamera.enabled = false;
            }
            else
                Debug.Log("No camera attached!");

        }
    }
}
