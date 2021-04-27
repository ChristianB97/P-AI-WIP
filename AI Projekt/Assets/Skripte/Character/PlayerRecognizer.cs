using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecognizer : MonoBehaviour
{
    public Transform playerTarget;
    public LineRenderer lineRenderer;
    public LayerMask maske;

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
            playerTarget = collision.transform;
            lineRenderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerTarget = null;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (playerTarget)
        {
            if (Physics2D.Linecast(gameObject.transform.position, playerTarget.position, maske))
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
                lineRenderer.SetPosition(1, playerTarget.position);
            }

        }

    }
}
