using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEventPlanner  {

    public List<TimedEvent> timedEvents;
    public List<TimedEvent> finishedEvents;

    public TimedEventPlanner()
    {
        timedEvents = new List<TimedEvent>();
        finishedEvents = new List<TimedEvent>();
    }

    public void StepEvents(float deltaTime)
    {
        for (int i = 0; i < timedEvents.Count; i++)
        {
            timedEvents[i].Step(deltaTime);
            if (timedEvents[i].IsFinished)
            {
                finishedEvents.Add(timedEvents[i]);
            }
        }
    }

    public void ClearEvents()
    {
        for (int i = 0; i < finishedEvents.Count; i++)
        {
            timedEvents.Remove(finishedEvents[i]);
        }
        finishedEvents.Clear();
    }

    public TimedEvent AddEvent(float time, OnComplete onComplete)
    {
        TimedEvent tm = new TimedEvent();
        tm.SetupEvent(time);
        tm.onComplete += onComplete;
        timedEvents.Add(tm);
        return tm;
    }

}
