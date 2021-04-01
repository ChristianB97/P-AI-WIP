using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour
{
    public Tilemap[] tileMapLayers;
    public RectTransform uiBackgroundRect;
    private float imageSize;
    public Image map;

    private void Start()
    {
        map = GetComponent<Image>();
        DrawMap();

    }

    public void DrawMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
            print("KILL");
        }
        Vector3Int[] gridCorners = GetCornersOfGrid();
        foreach (Tilemap tileMap in tileMapLayers)
        {
            Vector3[] corners = new Vector3[4];
            uiBackgroundRect.GetLocalCorners(corners);
            CalculateImageSize(gridCorners);
            DrawMapLayer(tileMap, corners[0], corners[2], gridCorners[0]);
        }
    }

    private void CalculateImageSize(Vector3Int[] corners)
    {
        int width = corners[2].x - corners[1].x;
        imageSize = uiBackgroundRect.sizeDelta.x / width;
        print(width);
    }

    private void DrawMapLayer(Tilemap tileMap, Vector2 uiStartCoordinate, Vector2 uiEndCoordinate, Vector3Int tileStartCoordinates)
    {
        int xUICount = 0;
        int yUICount = 0;

        for (int tileMapY = tileStartCoordinates.y; tileMapY < (tileMap.origin.y + tileMap.size.y); tileMapY++)
        {
            xUICount = 0;
            yUICount++;
            for (int tileMapX = tileStartCoordinates.x; tileMapX < (tileMap.origin.x + tileMap.size.x); tileMapX++)
            {
                xUICount++;
                Sprite sprite = tileMap.GetSprite(new Vector3Int(tileMapX, tileMapY, 0));
                if (sprite != null)
                {
                    DrawPicture(uiStartCoordinate, xUICount, yUICount, sprite);
                }
            }
        }
    }

    private void DrawPicture(Vector2 uiStartCoordinate, int xUICount, int yUICount, Sprite sprite)
    {
        GameObject picture = new GameObject();
        picture.transform.SetParent(gameObject.transform);
        RectTransform currentRect = picture.AddComponent<RectTransform>();
        currentRect.localPosition = new Vector2(uiStartCoordinate.x + (xUICount * imageSize), uiStartCoordinate.y + (yUICount * imageSize));
        //print(uiStartCoordinate.x + " + " + "(" + xUICount + "*" + mapSize + ")," + uiStartCoordinate.y + "+" + "(" + yUICount + "*" + mapSize + ")");
        Image currentImage = picture.AddComponent<Image>();
        currentImage.sprite = sprite;
        currentRect.sizeDelta = new Vector2(imageSize, imageSize);
    }

    private void DrawGL(Vector2 uiStartCoordinate, int xUICount, int yUICount, Sprite sprite)
    {
        Color[] spriteData = sprite.texture.GetPixels();
        int spriteSize = spriteData.Length;
        GL.Begin(GL.LINES);
        for (int i = 0; i < spriteSize; i++)
        {
            float localXPos = i % sprite.rect.width;
            float localYPos = Mathf.FloorToInt(sprite.rect.width / sprite.rect.width);
            GL.Color(spriteData[i]);
            GL.Vertex(new Vector3(localXPos * xUICount, localYPos * yUICount));
            GL.Vertex(new Vector3(localXPos * xUICount, localYPos * yUICount));
        }
        GL.End();
    }

    private Vector3Int[] GetCornersOfGrid()
    {
        int smallestXPos = tileMapLayers[0].origin.x + tileMapLayers[0].size.x;
        int smallestYPos = tileMapLayers[0].origin.y + tileMapLayers[0].size.y;
        int biggestXPos = tileMapLayers[0].origin.x;
        int biggestYPos = tileMapLayers[0].origin.y;
        foreach (Tilemap tileMap in tileMapLayers)
        {
            tileMap.CompressBounds();
            smallestXPos = Mathf.Min(smallestXPos, tileMap.cellBounds.min.x);
            smallestYPos = Mathf.Min(smallestYPos, tileMap.cellBounds.min.y);
            biggestXPos = Mathf.Max(biggestXPos, tileMap.cellBounds.max.x);
            biggestYPos = Mathf.Max(biggestYPos, tileMap.cellBounds.max.y);
        }
        Vector3Int lowerLeftCorner = new Vector3Int(smallestXPos, smallestYPos, 0);
        Vector3Int upperLeftCorner = new Vector3Int(smallestXPos, biggestYPos, 0);
        Vector3Int upperRightCorner = new Vector3Int(biggestXPos, biggestYPos, 0);
        Vector3Int lowerRightCorner = new Vector3Int(biggestXPos, smallestYPos, 0);
        Vector3Int[] corners = new Vector3Int[] { lowerLeftCorner, upperLeftCorner, upperRightCorner, lowerRightCorner };

        print("Lower left corner: " + lowerLeftCorner);
        print("Upper right corner: " + upperRightCorner);

        return corners;
    }
}
