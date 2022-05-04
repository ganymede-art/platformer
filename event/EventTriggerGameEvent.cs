using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerGameEvent : MonoBehaviour, IEventController
{
    public GameObject nextEventSource;
    [Header("Event Attributes")]
    public GameObject gameEventTriggerObject;

    private void Start()
    {
        
    }

    public void FinishEvent(GameEvent gameEvent) { }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_TRIGGER_GAME_EVENT;
    }

    public string GetEventDescription()
    {
        return GameConstants.EVENT_TYPE_TRIGGER_GAME_EVENT;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return true;
    }

    public bool IsGameEventComplete(GameEvent gameEvent)
    {
        return true;
    }

    public bool GetIsUpdateComplete(GameEvent gameEvent)
    {
        return true;
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public void UpdateEvent(GameEvent gameEvent) { }

    public void StartEvent(GameEvent gameEvent)
    {
        if (gameEventTriggerObject == null)
        {
            Debug.LogError("Missing event trigger object.");
            return;
        }

        var triggerComponent = gameEventTriggerObject
            .GetComponent<GameEventTrigger>();

        if (triggerComponent == null)
        {
            Debug.LogError("Missing event trigger component.");
            return;
        }

        triggerComponent.StartGameEvent();
    }

    public void ResetEvent(GameEvent gameEvent) { }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource,optionalObject1:gameEventTriggerObject,optionalColour1:Color.cyan);
    }
}
