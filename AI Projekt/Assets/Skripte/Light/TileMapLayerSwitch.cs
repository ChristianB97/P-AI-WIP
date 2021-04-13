using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapLayerSwitch : MonoBehaviour
{
    public OnEnterExitEvent events;
    public string enterLayer;
    public string exitLayer;
    public TilemapRenderer tilemap;

    private void Start()
    {
        if (!tilemap) { tilemap = GetComponent<TilemapRenderer>(); }
        events.onEnter += delegate { SetTileMapLayer(enterLayer); };
        events.onExit += delegate { SetTileMapLayer(exitLayer); };
        tilemap.sortingLayerName = exitLayer;
    }

    private void SetTileMapLayer(string layer)
    {
        tilemap.sortingLayerName = layer;
    }
}
