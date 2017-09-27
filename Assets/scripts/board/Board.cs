using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Board  {

    public int width;
    public int height;
    public int boxSize;

    public BoxTile prefab_jewel;

    public BoxTile currentTile;

    private BoxTile[] tiles;
    private BoxTile swappedWith;
    private List<BoxTile> connections;

    public void FillBoard()
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
                sampleJewel.Setup(tilePosition, new Vector2(i, j), 0);
                sampleJewel.RandomizeValue();
                sampleJewel.name = "Jewel Box " + (i * j);
                SetTile(i, j, sampleJewel);
            }
        }
    }

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

    public void NotTouched()
    {
        currentTile = null;
    }

    public bool TouchedBoard(Vector3 worldPos)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i].IsTouched(worldPos, out currentTile))
            {
                return true;
            }
        }
        currentTile = null;
        return false;
    }

    public bool CheckSwap(Vector3 worldPos)
    {
        BoxTile neighBor;
        for (int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i].IsTouched(worldPos, out neighBor))
            {
                bool differentTile = neighBor != currentTile;
                Debug.Log(neighBor);
                Debug.Log(currentTile);

                bool horCheck = Mathf.Abs(neighBor.x - currentTile.x) <= 1 && Mathf.Abs(neighBor.y - currentTile.y) == 0;
                bool verCheck = Mathf.Abs(neighBor.y - currentTile.y) <= 1 && Mathf.Abs(neighBor.x - currentTile.x) == 0;

                if(differentTile && (verCheck || horCheck))
                {
                    SwapTiles(currentTile, neighBor);
                    swappedWith = neighBor;
                    return true;
                }
            }
        }
        return false;
    }

    public void ReverseSwap()
    {
        SwapTiles(currentTile, swappedWith);
        currentTile = null;
        swappedWith = null;
    }

    public void SwapTiles(BoxTile siOne, BoxTile siTwo)
    {
        Vector3 freePos = siOne.transform.position;
        siOne.transform.position = siTwo.transform.position;
        siTwo.transform.position = freePos;

        int freex = siOne.x;
        siOne.x = siTwo.x;
        siTwo.x = freex;

        int freey = siOne.y;
        siOne.y = siTwo.y;
        siTwo.y = freey;

        SetTile(siOne.x, siOne.y, siOne);
        SetTile(siTwo.x, siTwo.y, siTwo);
    }

    public void MetaSwapTiles(BoxTile siOne, BoxTile siTwo)
    {
        int freex = siOne.x;
        siOne.x = siTwo.x;
        siTwo.x = freex;

        int freey = siOne.y;
        siOne.y = siTwo.y;
        siTwo.y = freey;

        SetTile(siOne.x, siOne.y, siOne);
        SetTile(siTwo.x, siTwo.y, siTwo);
    }

    public bool FlushBoard()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            Vector3 destination = new Vector3(tiles[i].x * boxSize, tiles[i].y * boxSize);
            tiles[i].Step(destination);
        }

        bool allInPlace = true;
        for (int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i].inPlace == false)
            {
                allInPlace = false;
                return allInPlace;
            }
        }
        return allInPlace;
    }

    public void ClearPieces()
    {
        ShiftConnections(connections);
    }

    private void ShiftConnections(List<BoxTile> connections)
    {
        for (int i = 0; i < connections.Count; i++)
        {
            ShiftTileUp(connections[i]);
        }

        for (int i = 0; i < connections.Count; i++)
        {
            connections[i].transform.position = new Vector3(connections[i].x, connections[i].y + height * 2);
            connections[i].inPlace = false;
            connections[i].RandomizeValue();
        }
    }

    private void ShiftTileUp(BoxTile tile)
    {
        int tileY = tile.y;
        while(tileY < height)
        {
            tileY += 1;
            BoxTile topTile;
            if(GetTile(tile.x, tileY, out topTile))
            {
                MetaSwapTiles(tile, topTile);
            }
        }
    }

    private List<BoxTile> CrawlAllConnections()
    {
        List<BoxTile> allConnections = new List<BoxTile>();
        List<BoxTile> availiableNeighBors = new List<BoxTile>();
        availiableNeighBors.Add(currentTile);
        
        while(availiableNeighBors.Count > 0)
        {
            BoxTile headTile = availiableNeighBors[0];
            BoxTile neighBor;

            allConnections.Add(headTile);

            if(GetTile(headTile.x, headTile.y -1 , out neighBor))
            {
                if (headTile.HasSameValue(neighBor))
                {
                    if (!allConnections.Contains(neighBor))
                    {
                        availiableNeighBors.Add(neighBor);
                    }
                }
            }

            if (GetTile(headTile.x, headTile.y + 1, out neighBor))
            {
                if (headTile.HasSameValue(neighBor))
                {
                    if (!allConnections.Contains(neighBor))
                    {
                        availiableNeighBors.Add(neighBor);
                    }
                }
            }

            if (GetTile(headTile.x +1 , headTile.y, out neighBor))
            {
                if (headTile.HasSameValue(neighBor))
                {
                    if (!allConnections.Contains(neighBor))
                    {
                        availiableNeighBors.Add(neighBor);
                    }
                }
            }

            if (GetTile(headTile.x - 1, headTile.y , out neighBor))
            {
                if (headTile.HasSameValue(neighBor))
                {
                    if (!allConnections.Contains(neighBor))
                    {
                        availiableNeighBors.Add(neighBor);
                    }
                }
            }
            availiableNeighBors.Remove(headTile);
        }
        return allConnections;
    }

    public bool ConnectionsExist()
    {
        connections = CrawlAllConnections();
        if(connections.Count > 2)
        {
            return true;
        }

        return false;
    }

    public int GetConnectionsCount()
    {
        return connections.Count;
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
