using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;

public class EventSetGameVarBool : MonoBehaviour, IEventController
{
    private GameEvent parentEvent;
    public GameEvent ParentEvent
    {
        get => parentEvent;
        set => parentEvent = value;
    }

    [Header("Event Attributes")]
    [FormerlySerializedAs("next_event_source")]
    public GameObject nextEventSource = null;

    [Header("Var Attributes")]
    public string variableName;
    public bool variableValue;


    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_SET_GAME_VAR_BOOL;
    }

    public string GetEventDescription()
    {
        return GameConstants.EVENT_TYPE_SET_GAME_VAR_BOOL + "_" + variableName + "_" + variableValue;
    }

    public void StartEvent(GameEvent gameEvent)
    {
        GameMasterController.Global.dataController.UpdateGameVar(variableName, variableValue);
    }

    public void UpdateEvent(GameEvent gameEvent)
    {
        return;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return true;
    }

    public bool IsGameEventComplete(GameEvent gameEvent)
    {
        return GetIsEventComplete(gameEvent);
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
