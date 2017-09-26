using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Board  {

    public int width;
    public int height;
    public int boxSize;

    public BoxTile prefab_jewel;

    private BoxTile[] tiles;

    public void FillFromMatrix(int[] matrix)
    {
        tiles = new BoxTile[width * height];
        Vector3 startPos = Vector3.zero;
        for (int i = 0; i < width; i++)
        {
            startPos.x = i * boxSize;
            for (int j = 0; j < height; j++)
            {
                startPos.y = j * boxSize;
                Vector3 tilePosition = startPos;
                tilePosition.x = i * boxSize;
                tilePosition.y = j * boxSize;
                BoxTile sampleJewel = GameObject.Instantiate(prefab_jewel);
                sampleJewel.Setup(tilePosition, new Vector2(i, j), matrix[i + j * width]);
                sampleJewel.name = "Jewel Box " + (i * j);
                SetTile(i, j, sampleJewel);
            }
        }
    }

    public bool TouchedBoard(Vector3 worldPos)
    {
        return false;
    }

    public bool CheckSwap(Vector3 worldPos)
    {
        return false;
    }

    public void ClearPieces()
    {

    }

    public bool ConnectionsExist()
    {
        return false;
    }

    public int GetConnectionsCount()
    {
        return 0;
    }

    public BoxTile SetTile(int x, int y, BoxTile newTile)
    {
        if (PositionInBounds(x, y))
        {
            BoxTile oldTile = tiles[x + width * y];
            tiles[x + width * y] = newTile;
            return oldTile;
        }
        return newTile;
    }

    public bool GetTile(int x, int y, out BoxTile emptyTile)
    {
        if (PositionInBounds(x, y))
        {
            emptyTile = tiles[x + width * y];
            return true;
        }
        emptyTile = null;
        return false;
    }

    private bool PositionInBounds(int x, int y)
    {
        return x >= 0 && x < width && y < height && y >= 0;
    }

    public int[] GetColorIndexMapping()
    {
        int[] colorMapping = new int[width * height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                colorMapping[i + j * width] = tiles[i + j * width].colorIndex;
            }
        }
        return colorMapping;
    }

}
