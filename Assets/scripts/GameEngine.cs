using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour {

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
    public GameDataProvider gameDataProvider;
    public bool freeze;
    public TimedEventPlanner timedEventPlanner;
    public TimedEvent timeLeft;

	// Use this for initialization
	void Start () {
        gameState = GameState.WaitMove;
        BeginBoard();
        gameDataProvider = GameObject.FindObjectOfType<GameDataProvider>();
        timedEventPlanner = new TimedEventPlanner();
        freeze = false;
	}

    private void BeginBoard()
    {
        gameBoard.prefab_jewel = Resources.Load<BoxTile>("Jewel/prefabs/Jewel");
        gameBoard.FillBoard();
    }
	
	// Update is called once per frame
	void Update () {
        timedEventPlanner.StepEvents(Time.deltaTime);
        if (!freeze)
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
        timedEventPlanner.ClearEvents();
	}

    public void WaitMove(Vector3 worldPos)
    {
        if(Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
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
            gameBoard.ClearPieces();
            int value;
            if(gameDataProvider.gameData.TryGetInt(GameData.SCORE,out value))
            {
                value += 100;
            }
            else
            {
                value = 100;
            }
            gameDataProvider.gameData.SetInt(GameData.SCORE, value);

            SwitchState(GameState.FlushBoard);
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
        freeze = true;
        gameState = newState;
        timedEventPlanner.AddEvent(0.2f, Unfreeze);
    }

    public void Unfreeze()
    {
        freeze = false;
    }
}
