using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnterExitEvent : MonoBehaviour
{
    public Action onEnter;
    public Action onExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onEnter?.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        onExit?.Invoke();
    }
}
