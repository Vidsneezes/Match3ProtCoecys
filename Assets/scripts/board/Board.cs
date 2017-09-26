using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Board  {

    public int width;
    public int height;
    public int boxSize;

    public float tilePaddingRight;
    public float tilePaddingUp;

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
                Vector3 newPOs = startPos;
                newPOs.x = i * boxSize * tilePaddingRight;
                newPOs.y = j * boxSize * tilePaddingUp;
                BoxTile sampleJewel = GameObject.Instantiate(prefab_jewel);
                sampleJewel.SetupRandom(newPOs , new Vector2(i, j));
                sampleJewel.name = "Box " + i + " " + j;
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
                Vector3 newPOs = startPos;
                newPOs.x = i * boxSize * tilePaddingRight;
                newPOs.y = j * boxSize * tilePaddingUp;
                BoxTile sampleJewel = GameObject.Instantiate(prefab_jewel);
                sampleJewel.Setup(newPOs, new Vector2(i, j),matrix[i+j*width]);
                sampleJewel.name = "Box " + i + " " + j;
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
            if (tiles[i].IsTouched(worldPos, out currentTile))
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
            if (tiles[i].IsTouched(worldPos, out neighBor))
            {
                bool differentTile = neighBor != currentTile;
                bool horNeighBorByOne = Mathf.Abs(neighBor.x - currentTile.x) <= 1 && Mathf.Abs(neighBor.y - currentTile.y) == 0;
                bool verNeighBorByOne = Mathf.Abs(neighBor.y - currentTile.y) <= 1 && Mathf.Abs(neighBor.x - currentTile.x) == 0;

                if (neighBor != currentTile && (verNeighBorByOne || horNeighBorByOne))
                {
                    SwapTiles(currentTile, neighBor);
                    swappedWith = neighBor;
                    return true;
                }
            }
        }
        return false;
    }

    public void ReverseMove()
    {
        SwapTiles(currentTile, swappedWith);
        currentTile = null;
        swappedWith = null;
    }

    private void SwapTiles(BoxTile sideOne, BoxTile sideTwo)
    {
        Vector3 freePos = sideOne.transform.position;
        sideOne.transform.position = sideTwo.transform.position;
        sideTwo.transform.position = freePos;

        int freex = sideOne.x;
        sideOne.x = sideTwo.x;
        sideTwo.x = freex;

        int freey = sideOne.y;
        sideOne.y = sideTwo.y;
        sideTwo.y = freey;

        SetTile(sideOne.x, sideOne.y, sideOne);
        SetTile(sideTwo.x, sideTwo.y, sideTwo);
    }

    private void MetaSwapTiles(BoxTile sideOne, BoxTile sideTwo)
    {
        int freex = sideOne.x;
        sideOne.x = sideTwo.x;
        sideTwo.x = freex;

        int freey = sideOne.y;
        sideOne.y = sideTwo.y;
        sideTwo.y = freey;

        SetTile(sideOne.x, sideOne.y, sideOne);
        SetTile(sideTwo.x, sideTwo.y, sideTwo);
        sideOne.inPlace = false;
        sideTwo.inPlace = false;
    }

    public bool FlushBoard()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            Vector3 destination = new Vector3(tiles[i].x * boxSize * tilePaddingRight, tiles[i].y * boxSize * tilePaddingUp);
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
        return;
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

    private List<BoxTile> CrawlAllConnections()
    {
        List<BoxTile> allConnections = new List<BoxTile>();
        List<BoxTile> availiableNeighBors = new List<BoxTile>();
        availiableNeighBors.Add(currentTile);
        while(availiableNeighBors.Count > 0)
        {
            BoxTile headTile = availiableNeighBors[0];
            headTile.inPlace = false;
            BoxTile neighBor;

            allConnections.Add(headTile);

            if(GetTile(headTile.x,headTile.y-1,out neighBor))
            {
                if (headTile.HasSameValue(neighBor))
                {
                    if (!allConnections.Contains(neighBor))
                    {
                        availiableNeighBors.Add(neighBor);
                    }
                }
            }

            if(GetTile(headTile.x,headTile.y +1,out neighBor))
            {
                if (headTile.HasSameValue(neighBor))
                {
                    if (!allConnections.Contains(neighBor))
                    {
                        availiableNeighBors.Add(neighBor);
                    }
                }
            }

            if (GetTile(headTile.x+1, headTile.y , out neighBor))
            {
                if (headTile.HasSameValue(neighBor))
                {
                    if (!allConnections.Contains(neighBor))
                    {
                        availiableNeighBors.Add(neighBor);
                    }
                }
            }

            if (GetTile(headTile.x - 1, headTile.y, out neighBor))
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

    private void ShiftConnections(List<BoxTile> connections)
    {
        for (int i = 0; i < connections.Count; i++)
        {
            ShiftTileUp(connections[i]);
        }

        for (int i = 0; i < connections.Count; i++)
        {
            connections[i].transform.position = new Vector3(connections[i].x, connections[i].y + height * 2);
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
            if(GetTile(tile.x,tileY,out topTile))
            {
                MetaSwapTiles(tile,topTile);
            }
        }
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

    private bool PositionInBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public int[] GetColorIndexMapping()
    {
        int[] colorIndexes = new int[width * height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                colorIndexes[i + j * width] = tiles[i + j * width].colorIndex;
            }
        }
        return colorIndexes;
    }

    public void DebugColorIndex()
    {
        string con = "";
        int[] colorIndexes = GetColorIndexMapping();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                con += colorIndexes[j + i * width];
            }
            con += "\n";
        }
        Debug.Log(con);
    }
}
