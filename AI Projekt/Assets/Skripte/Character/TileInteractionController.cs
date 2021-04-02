using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CharacterFacing))]
public class TileInteractionController : MonoBehaviour
{
    private CharacterFacing facing;
    public Tilemap tileMap;

    private void Start()
    {
        facing = GetComponent<CharacterFacing>();
    }

    private void Update()
    {
        if (Input.GetButtonUp("Jump"))
        {
            Use();
        }
    }

    public void Use()
    {
        Vector3Int pos = facing.GetGridCoordinateFacing();
        print(pos);
        Tile tile = tileMap.GetTile<Tile>(pos);
        tileMap.SetTile(pos, null);
    }


}
