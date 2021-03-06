using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;

public class EventDelay : MonoBehaviour, IEventController
{
    private GameEvent parentEvent;
    public GameEvent ParentEvent
    {
        get => parentEvent;
        set => parentEvent = value;
    }

    private float startTime = 0.0F;

    [FormerlySerializedAs("next_event_source")]
    public GameObject nextEventSource = null;
    [FormerlySerializedAs("delay_seconds")]
    public float delaySeconds = 0.0F;

    void Start() { }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_DELAY;
    }

    public string GetEventDescription()
    {
        return GameConstants.EVENT_TYPE_DELAY + "_" + delaySeconds + "s";
    }

    public void StartEvent(GameEvent gameEvent)
    {
        startTime = Time.time;
    }

    public void UpdateEvent(GameEvent gameEvent)
    {
        return;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return (Time.time - startTime) >= delaySeconds;
    }

    public bool GetIsUpdateComplete(GameEvent gameEvent)
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
