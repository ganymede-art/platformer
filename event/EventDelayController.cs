using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class EventDelayController : MonoBehaviour, IEventController
{
    private GameEvent parentEvent;
    public GameEvent ParentEvent
    {
        get => parentEvent;
        set => parentEvent = value;
    }

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

    public void StartEvent(GameEvent gameEvent)
    {
        startTime = Time.time;
    }

    public void ProcessEvent(GameEvent gameEvent)
    {
        return;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return (Time.time - startTime) >= delaySeconds;
    }

    public bool GetIsGameEventComplete(GameEvent gameEvent)
    {
        return GetIsEventComplete(gameEvent);
    }

    public bool GetIsProcessComplete(GameEvent gameEvent)
    {
        return GetIsEventComplete(gameEvent);
    }

    public void FinishEvent(GameEvent gameEvent)
    {
        return;
    }

    public void ResetEvent(GameEvent gameEvent) { }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
    }
}
