using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTileMarker : MonoBehaviour
{
    public Grid grid;

    public Sprite marker;
    public float playerXOffset;
    public float playerYOffset;
    public BoolValue playerWalkBool;
    private bool isTileMarkMode;
    public SpriteRenderer markerSpriteRenderer;
    public CharacterFacing characterFacing;

    private void Start()
    {
        if (markerSpriteRenderer == null)
        {
            CreateVisuals();
            Debug.Log("SpriteRenderer missing at " + name);
        }
        markerSpriteRenderer.enabled = false;
        characterFacing.onChangedDirection += UpdateDirection;
    }

    private void CreateVisuals()
    {
        GameObject tileMarker = new GameObject();
        tileMarker.transform.parent = transform;
        markerSpriteRenderer = tileMarker.AddComponent<SpriteRenderer>();
        markerSpriteRenderer.sprite = marker;
        markerSpriteRenderer.sortingLayerName = "ColliderLayer";
    }

    private void Update()
    {
        if (Input.GetMouseButton(2))
        {
            SetFrontTileMode(true);
            UpdateDirection(characterFacing.GetWorldCoordinateFacing());
        }
        else //schöner machen evtl.
        {
            SetFrontTileMode(false);
        }
    }

    public void SetFrontTileMode(bool value)
    {
        playerWalkBool.RuntimeValue = !value;
        isTileMarkMode = value;
        markerSpriteRenderer.enabled = value;
    }

    public void UpdateDirection(Vector3 facedTilePosition)
    {
        if (isTileMarkMode)
        {
            markerSpriteRenderer.transform.position = facedTilePosition;
        }
    }
}