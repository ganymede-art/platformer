using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class EventSetGameVarBool : MonoBehaviour, IEventController
{
    private GameEvent parentEvent;
    public GameEvent ParentEvent
    {
        get => parentEvent;
        set => parentEvent = value;
    }

    [FormerlySerializedAs("next_event_source")]
    public GameObject nextEventSource = null;

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

    public void ProcessEvent(GameEvent gameEvent)
    {
        return;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return true;
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
