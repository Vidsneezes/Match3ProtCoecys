using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public TimedEvent AddEvent(float time, OnComplete onComp)
    {
        TimedEvent tm = new TimedEvent();
        tm.SetupEvent(time);
        tm.onComplete += onComp;
        timedEvents.Add(tm);
        return tm;
    }
}