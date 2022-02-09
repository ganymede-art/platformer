using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class EventRandomNextEvent : MonoBehaviour, IEventController
{
    private GameEvent parentEvent;
    public GameEvent ParentEvent
    {
        get => parentEvent;
        set => parentEvent = value;
    }

    public GameObject[] nextEventSources;

    void Start() { }

    public GameObject GetNextEventSource()
    {
        int nextEventIndex = UnityEngine.Random.Range(0, nextEventSources.Length);
        return nextEventSources[nextEventIndex];
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_RANDOM_NEXT_EVENT;
    }

    public string GetEventDescription()
    {
        return GameConstants.EVENT_TYPE_RANDOM_NEXT_EVENT + "_" + nextEventSources.Length;
    }

    public void StartEvent(GameEvent gameEvent) {}

    public void UpdateEvent(GameEvent gameEvent) {}

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return true;
    }

    public bool GetIsUpdateComplete(GameEvent gameEvent)
    {
        return GetIsEventComplete(gameEvent);
    }

    public void FinishEvent(GameEvent gameEvent) { }

    public void ResetEvent(GameEvent gameEvent) { }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, this.gameObject);
    }
}
