using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;

    TileBase tBase;
    private void Start()
    {
  
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Camera>().transform;
        }
        if (playerTransform == null)
        {
            playerTransform = FindObjectOfType<Player_Walk>().transform;
        }
    }

    private void LateUpdate()
    {
        cameraTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, cameraTransform.position.z);
    }
}
