using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerRecognizer : MonoBehaviour
{
    public string target;
    private Transform currentTargetTransform;
    public LineRenderer lineRenderer;
    public LayerMask maske;
    public Action<Transform> onTargetFound;
    public Action onTargetLost;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.sortingLayerName = "Roof";
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.5f;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentTargetTransform = collision.transform;
            lineRenderer.enabled = true;
            onTargetFound?.Invoke(currentTargetTransform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentTargetTransform = null;
            lineRenderer.enabled = false;
            onTargetLost?.Invoke();
        }
    }

    private void Update()
    {
        if (currentTargetTransform)
        {
            if (Physics2D.Linecast(gameObject.transform.position, currentTargetTransform.position, maske))
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }
            else
            {
                if (!lineRenderer.enabled)
                {
                    lineRenderer.enabled = true;
                }
                lineRenderer.SetPosition(0, gameObject.transform.position);
                lineRenderer.SetPosition(1, currentTargetTransform.position);
            }

        }

    }
}
