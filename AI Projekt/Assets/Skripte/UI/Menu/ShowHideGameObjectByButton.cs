using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideGameObjectByButton : MonoBehaviour
{
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private string button;
    void Update()
    {
        if (Input.GetButtonDown(button))
        {
            print("pressed");
            selectedObject.SetActive(!selectedObject.active);
        }
    }
}
