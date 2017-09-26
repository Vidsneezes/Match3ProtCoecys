using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


public class BoardTest  {

    [Test]
    public void CorrectlySetsANewMatrix()
    {
        int[] mat =
        {
            0,0,
            1,0
        };
        Board gmBoard = new Board();
        gmBoard.width = 2;
        gmBoard.height = 2;
        gmBoard.prefab_jewel = Resources.Load<BoxTile>("Jewel/prefab/Jewel");
        gmBoard.FillFromMatrix(mat);
        Assert.AreEqual(mat, gmBoard.GetColorIndexMapping());
    }

    [Test]
    public void CanGetATouchFromAll()
    {
        int[] mat =
        {
            0,0,1,3,4,
            1,1,2,0,2
        };
        Board gmBoard = new Board();
        gmBoard.tilePaddingRight = 1;
        gmBoard.tilePaddingUp = 1;
        gmBoard.boxSize = 1;
        gmBoard.width = 5;
        gmBoard.height = 2;
        gmBoard.prefab_jewel = Resources.Load<BoxTile>("Jewel/prefab/Jewel");
        gmBoard.FillFromMatrix(mat);
        gmBoard.DebugColorIndex();
        bool allWork = true;
        for (int i = 0; i < gmBoard.width; i++)
        {
            for (int j = 0; j < gmBoard.height; j++)
            {
                BoxTile atile;
                gmBoard.GetTile(i, j, out atile);
                Vector3 pos = atile.transform.position;
                pos.x += 0.25f;
                pos.y += 0.25f;
                bool gotTouch = gmBoard.TouchedBoard(pos);
                if(gotTouch == false)
                {
                    allWork = false;
                    break;
                }
            }
        }
        Assert.AreEqual(true, allWork);
    }

    [Test]
    public void TestASuccessfulSwap()
    {
        int[] mat =
        {
            0,0,1,3,4,
            1,1,2,0,2
        };

        int[] matShifted =
        {
            0,0,2,3,4,
            0,0,2,0,2
        };
        Board gmBoard = new Board();
        gmBoard.tilePaddingRight = 1;
        gmBoard.tilePaddingUp = 1;
        gmBoard.boxSize = 1;
        gmBoard.width = 5;
        gmBoard.height = 2;
        gmBoard.prefab_jewel = Resources.Load<BoxTile>("Jewel/prefab/Jewel");
        gmBoard.FillFromMatrix(mat);
        gmBoard.DebugColorIndex();

        BoxTile atile;
        gmBoard.GetTile(2, 0, out atile);
        Vector3 pos_1 = atile.transform.position;
        gmBoard.TouchedBoard(pos_1);
        BoxTile othertile;
        gmBoard.GetTile(2, 1, out othertile);

        Vector3 pos_2 = othertile.transform.position;

        Assert.AreEqual(true, gmBoard.CheckSwap(pos_2));
        gmBoard.GetTile(2, 0, out atile);

        Assert.AreEqual(true, atile.HasSameValue(othertile));

        Assert.AreEqual(true, gmBoard.ConnectionsExist());

        int con = gmBoard.GetConnectionsCount();
        Assert.AreEqual(3, con);

        gmBoard.ClearPieces();

        gmBoard.GetTile(0, 0, out atile);
        Assert.AreEqual(matShifted[0], atile.colorIndex);
        gmBoard.GetTile(1, 0, out atile);
        Assert.AreEqual(matShifted[1], atile.colorIndex);
        gmBoard.GetTile(2, 0, out atile);
        Assert.AreEqual(matShifted[2], atile.colorIndex);

        while(gmBoard.FlushBoard() == false)
        {

        }
        Assert.AreEqual(true, gmBoard.FlushBoard());
    }
}
