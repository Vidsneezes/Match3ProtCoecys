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
    public TimedEventPlanner timedEventPlanner;
    private bool freeze;


	// Use this for initialization
	void Start () {
        timedEventPlanner = new TimedEventPlanner();
        gameBoard.FillBoard();
        gameState = GameState.WaitMove;
        freeze = false;
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
            }
        }

        CleanFrame();
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
            gameBoard.ClearPieces();
            SwitchState(GameState.FlushBoard);
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
        timedEventPlanner.AddEvent(0.3f, Unfreeze);
    }

    private void Unfreeze()
    {
        freeze = false;
    }

}

public delegate void OnComplete();

public class TimedEventPlanner
{
    public List<TimedEvent> timedEvents;
    public List<TimedEvent> eventsFinished;


    public TimedEventPlanner()
    {
        timedEvents = new List<TimedEvent>();
        eventsFinished = new List<TimedEvent>();
    }

    public void StepEvents(float deltaTime)
    {

        for (int i = 0; i < timedEvents.Count; i++)
        {
            timedEvents[i].Step(deltaTime);
            if (timedEvents[i].IsFinished)
            {
                eventsFinished.Add(timedEvents[i]);
            }
        }
    }

    public void ClearEvents()
    {
        for (int i = 0; i < eventsFinished.Count; i++)
        {
            timedEvents.Remove(eventsFinished[i]);
        }
    }

    public void AddEvent(float time, OnComplete onComp)
    {
        TimedEvent tm = new TimedEvent();
        tm.SetupEvent(time);
        tm.onComplete += onComp;
        timedEvents.Add(tm);
    }
}

public class TimedEvent
{

    public float waitTime;
    public OnComplete onComplete;

    private bool finished;
    public bool IsFinished
    {
        get
        {
            return finished;
        }
    }

    private float timer;

    public TimedEvent()
    {
    }

    public void SetupEvent(float _waitTime)
    {
        timer = 0;
        waitTime = _waitTime;
        finished = false;
    }

    public void Step(float deltaStep)
    {
        timer += deltaStep;
        if(timer > waitTime)
        {
            Completed();
        }
    }

    public void Completed()
    {
        finished = true;
        if(onComplete != null)
        {
            onComplete();
        }
    }

   
}
