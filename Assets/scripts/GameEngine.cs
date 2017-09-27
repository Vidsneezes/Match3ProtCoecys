using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{

    public enum GameState
    {
        WaitMove,
        CheckMove,
        ReverseMove,
        SolveMove,
        FlushBoard
    }

    public Board gameBoard;
    public GameState gameState;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        int[] mat =
       {
            0,0,1,3,4,
            1,1,2,0,2
        };

        gameBoard.boxSize = 1;
        gameBoard.width = 5;
        gameBoard.height = 2;
        gameBoard.prefab_jewel = Resources.Load<BoxTile>("Jewel/prefabs/Jewel");
        gameBoard.FillFromMatrix(mat);

        gameState = GameState.WaitMove;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        switch (gameState)
        {
            case GameState.WaitMove: WaitMove(worldPos); break;
            case GameState.CheckMove: CheckMove(worldPos); break;
            case GameState.SolveMove: SolveMove(); break;
            case GameState.ReverseMove: ReverseMove(); break;
            case GameState.FlushBoard: FlushBoard(); break;
        }
    }

    public void WaitMove(Vector3 worldPos)
    {
        if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
        {
            gameBoard.NotTouched();
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (gameBoard.TouchedBoard(worldPos))
            {
                SwitchState(GameState.CheckMove);
            }
        }
    }

    public void CheckMove(Vector3 worldPos)
    {
        if (gameBoard.CheckSwap(worldPos))
        {
            SwitchState(GameState.SolveMove);
        }
    }

    public void SolveMove()
    {
        if (gameBoard.ConnectionsExist())
        {
            int piecesCount = gameBoard.GetConnectionsCount() + 1;
            gameBoard.ClearPieces();
        }
        else
        {
            SwitchState(GameState.ReverseMove);
        }
    }

    public void ReverseMove()
    {
        gameBoard.ReverseSwap();
        SwitchState(GameState.WaitMove);
    }

    public void FlushBoard()
    {
        if (gameBoard.FlushBoard())
        {
            SwitchState(GameState.WaitMove);
        }
    }

    public void SwitchState(GameState newState)
    {
        gameState = newState;
    }
}