using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class EventDelayController : MonoBehaviour, IEventController
{
    private GameMasterController master;
    private float startTime = 0.0f;

    [FormerlySerializedAs("next_event_source")]
    public GameObject nextEventSource = null;
    [FormerlySerializedAs("delay_seconds")]
    public float delaySeconds = 0.0f;

    void Start()
    {
        master = GameMasterController.GlobalMasterController;
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_DELAY;
    }

    public void StartEvent()
    {
        startTime = Time.time;
    }

    public void ProcessEvent()
    {
        return;
    }

    public bool GetIsEventComplete()
    {
        return (Time.time - startTime) >= delaySeconds;
    }

    public bool GetIsGameEventComplete()
    {
        return GetIsEventComplete();
    }

    public bool GetIsProcessComplete()
    {
        return GetIsEventComplete();
    }

    public void FinishEvent()
    {
        return;
    }
}
