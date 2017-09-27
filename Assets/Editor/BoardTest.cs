using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class BoardTest  {

    Board mBoard;
    int[] mat;
    int[] matShifted;

    BoxTile atile;
    BoxTile othertile;

    [SetUp]
    public void BoardSetup()
    {
        mat = new int[]
        {
            0,0,1,3,4,
            1,1,2,0,0
        };

        matShifted = new int[]
        {
            0,0,2,3,4,
            3,3,3,0,2
        };

        mBoard = new Board();
        mBoard.boxSize = 1;
        mBoard.width = 5;
        mBoard.height = 2;
        mBoard.prefab_jewel = Resources.Load<BoxTile>("Jewel/prefabs/Jewel");
        mBoard.FillFromMatrix(mat);
    }

    [Test]
    public void TouchingBoardReturnsTrue()
    {
        mBoard.GetTile(2, 0, out atile);
        Vector3 pos_1 = atile.transform.position;
        Assert.AreEqual(true, mBoard.TouchedBoard(pos_1));
    }

    [Test]
    public void MakeASuccessfulSwap()
    {
        mBoard.GetTile(2, 0, out atile);
        Vector3 pos_1 = atile.transform.position;
        mBoard.TouchedBoard(pos_1);
        mBoard.GetTile(2, 1, out othertile);
        Vector3 pos_2 = othertile.transform.position;
        Assert.AreEqual(true, mBoard.CheckSwap(pos_2));
    }

    [Test]
    public void SwapTilesAreSame()
    {
        mBoard.GetTile(2, 0, out atile);
        Vector3 pos_1 = atile.transform.position;
        mBoard.TouchedBoard(pos_1);
        mBoard.GetTile(2, 1, out othertile);
        Vector3 pos_2 = othertile.transform.position;
        mBoard.CheckSwap(pos_2);
        mBoard.GetTile(2, 0, out atile);
        Assert.AreEqual(true, atile.HasSameValue(othertile));
    }

    [Test]
    public void ConnectionsDoExist()
    {

        mBoard.GetTile(2, 0, out atile);
        Vector3 pos_1 = atile.transform.position;
        mBoard.TouchedBoard(pos_1);
        mBoard.GetTile(2, 1, out othertile);
        Vector3 pos_2 = othertile.transform.position;
        mBoard.CheckSwap(pos_2);
        mBoard.GetTile(2, 0, out atile);
        atile.HasSameValue(othertile);
        Assert.AreEqual(true, mBoard.ConnectionsExist());
    }

    [Test]
    public void ThereAreThreeConnections()
    {
        int con = mBoard.GetConnectionsCount();
        Assert.AreEqual(3, con);
    }

    [Test]
    public void TheBoardGetsNewPieces()
    {
        mBoard.ClearPieces();

        mBoard.GetTile(0, 0, out atile);
        Assert.AreEqual(matShifted[0], atile.colorIndex);
        mBoard.GetTile(1, 0, out atile);
        Assert.AreEqual(matShifted[1], atile.colorIndex);
        mBoard.GetTile(2, 1, out atile);
        Assert.AreEqual(matShifted[2], atile.colorIndex);
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
        gmBoard.boxSize = 1;
        gmBoard.width = 5;
        gmBoard.height = 2;
        gmBoard.prefab_jewel = Resources.Load<BoxTile>("Jewel/prefabs/Jewel");
        gmBoard.FillFromMatrix(mat);

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

        while (gmBoard.FlushBoard() == false)
        {

        }
        Assert.AreEqual(true, gmBoard.FlushBoard());
    }
}
