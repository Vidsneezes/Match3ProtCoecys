using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour {

    public enum GameMode
    {
        LimitMode,
        HighMode
    }

    public enum GameState
    {
        WaitMove,
        CheckMove,
        ReverseMove,
        SolveMove,
        FlushBoard,
        WonTimeLimit,
        GameOver
    }

    public int timeLimit;
    public int winLimit;
    public float feelTimer;

    public Board gameBoard;
    public GameState gameState;
    public TimedEventPlanner timedEventPlanner;
    public GameMode gameMode;

    private bool freeze;
    private TimedEvent timeLeftEvent;
    private int score;
    private bool gameOver;
    private GameDataProvider gameDataProvider;

    private void Start()
    {
        timedEventPlanner = new TimedEventPlanner();
        gameDataProvider = GameObject.FindGameObjectWithTag("GameDataProvider").GetComponent<GameDataProvider>();
        StartGame();
    }

    public void StartGame()
    {
        gameBoard.FillBoard();
        freeze = false;
        gameState = GameState.WaitMove;
        timeLeftEvent = timedEventPlanner.AddEvent(timeLimit, GameTimeDone);
        gameOver = false;
        FillVariables();
    }

    public void FillVariables()
    {
        int gameModeValue;
        if(gameDataProvider.gameData.TryGetInt(GameData.GAMEMODE,out gameModeValue))
        {
            if(gameModeValue == 0)
            {
                gameMode = GameMode.LimitMode;
            }
            else
            {
                gameMode = GameMode.HighMode;
            }
        }

        gameDataProvider.gameData.SetInt(GameData.SCORE, 0);
        gameDataProvider.gameData.SetInt(GameData.TIMELEFT, 0);
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
                case GameState.ReverseMove: ReverseMove();break;
                case GameState.FlushBoard: FlushBoard();break;
                case GameState.GameOver: GameOver();break;
                case GameState.WonTimeLimit: GameLimitWon();break;
            }
        }

        CleanFrame();
        float timeLeftClamp = Mathf.Clamp((timeLimit - timeLeftEvent.GetSeconds()), 0, timeLimit);
        gameDataProvider.gameData.SetInt(GameData.TIMELEFT, (int)timeLeftClamp);
    }

    public void GameLimitWon()
    {
        gameDataProvider.gameData.SetInt(GameData.GAMECONDITION, 1);
    }

    public void GameOver()
    {
        gameDataProvider.gameData.SetInt(GameData.SCORE, score);
        gameDataProvider.gameData.SetInt(GameData.GAMECONDITION, 0);
    }

    public void IncreaseScore(int amountToDelete)
    {
        score += amountToDelete * 100;
        gameDataProvider.gameData.SetInt(GameData.SCORE, score);
        gameDataProvider.gameData.SetInt(GameData.SCOREPERCENT, (int)Mathf.Clamp((((float)score / (float)winLimit) * 100), 0, 100));
    }

    public void GameTimeDone()
    {
        gameOver = true;
    }

    public void CleanFrame()
    {
        timedEventPlanner.ClearEvents();
    }

    public void WaitMove(Vector3 worldPos)
    {
        if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
        {
            gameBoard.NotTouched();
        }

        if (Input.GetMouseButtonDown(0))
        {

            if(gameBoard.TouchedBoard(worldPos))
            {
                SwitchState(GameState.CheckMove);
            }
        }
    }

    public void CheckMove(Vector3 worldPos)
    {
        if(gameBoard.CheckSwap(worldPos))
        {
            SwitchState(GameState.SolveMove);
        }
    }

    public void SolveMove()
    {
        if (gameBoard.ConnectionsExist())
        {
            int piecesCount = gameBoard.GetConnectionsCount() + 1;
            IncreaseScore(piecesCount);
            gameBoard.ClearPieces();

            if(score >= winLimit)
            {
                SwitchState(GameState.WonTimeLimit);
            }
            else
            {
                SwitchState(GameState.FlushBoard);

            }
        }
        else
        {
            SwitchState(GameState.ReverseMove);
        }
    }

    public void ReverseMove()
    {
        gameBoard.ReverseMove();
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
        timedEventPlanner.AddEvent(feelTimer, Unfreeze);
        if (gameOver)
        {
            gameState = GameState.GameOver;
        }
    }

    private void Unfreeze()
    {
        freeze = false;
    }

}
