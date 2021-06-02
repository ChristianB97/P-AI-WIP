using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnterExitEvent : MonoBehaviour
{
    public Action onEnter;
    public Action onExit;
    public UnityEvent onEnter2;
    public UnityEvent onExit2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onEnter?.Invoke();
            onEnter2?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onExit?.Invoke();
            onExit2?.Invoke();
        }
    }
}
