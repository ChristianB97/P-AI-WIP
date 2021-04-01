using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFacing : MonoBehaviour
{
    private Vector3Int facingDirection;
    private Vector3Int FacingDirection
    {
        set
        {
            facingDirection = FacingDirection;
            onChangedDirection?.Invoke(GetWorldCoordinateFacing());
        }

        get
        {
            return facingDirection;
        }
    }
    private byte currentDirection;
    public Action<Vector3> onChangedDirection;
    public Grid grid;

    private void Start()
    {
        facingDirection = Vector3Int.zero;
    }

    private void Update()
    {
        SetFacingDirection();
    }

    private void SetFacingDirection()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
            facingDirection = Vector3Int.right;
        else if (Input.GetAxisRaw("Horizontal") < 0)
            facingDirection = Vector3Int.left;
        else if (Input.GetAxisRaw("Vertical") > 0)
            facingDirection = Vector3Int.up;
        else if (Input.GetAxisRaw("Vertical") < 0)
            facingDirection = Vector3Int.down;
    }

    public byte GetDirection()
    {
        return currentDirection;
    }

    public Vector3 GetCharacterMiddleOfGridPosition()
    {
        return grid.CellToWorld(GetCharacterGridCoordinate()) + new Vector3(0.5f, 0.5f); //Offset der TileMap
    }

    public Vector3Int GetCharacterGridCoordinate()
    {
        return grid.WorldToCell(transform.position);
    }

    public Vector3Int GetGridCoordinateFacing()
    {
        return GetCharacterGridCoordinate() + facingDirection;
    }

    public Vector3 GetWorldCoordinateFacing()
    {
        return grid.CellToWorld(GetGridCoordinateFacing()) + new Vector3(0.5f , 0.5f); //Offset der TileMap
    }
}
