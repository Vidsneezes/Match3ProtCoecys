using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (timer > waitTime)
        {
            Completed();
        }
    }

    public void Completed()
    {
        finished = true;
        if (onComplete != null)
        {
            onComplete();
        }
    }

    public int GetSeconds()
    {
        return (int)timer;
    }
}
