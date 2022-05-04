using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;

public class EventSetGameVarInt : MonoBehaviour, IEventController
{
    [Header("Event Attributes")]
    public GameObject nextEventSource = null;

    [Header("Var Attributes")]
    public string variableName;
    public int variableValue;
    public bool isRelative;

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_SET_GAME_VAR_INT;
    }

    public string GetEventDescription()
    {
        return GameConstants.EVENT_TYPE_SET_GAME_VAR_INT + "_" + variableName + "_" + variableValue;
    }

    public void StartEvent(GameEvent gameEvent)
    {
        if(isRelative)
        {
            int currentValue = GameDataController.Global.GetGameVarInt(variableName);
            GameDataController.Global.UpdateGameVar(variableName, currentValue + variableValue);
        }
        else
        {
            GameDataController.Global.UpdateGameVar(variableName, variableValue);
        }
    }

    public void UpdateEvent(GameEvent gameEvent) { }
    public bool GetIsEventComplete(GameEvent gameEvent) { return true; }
    public bool IsGameEventComplete(GameEvent gameEvent) { return true; }
    public bool GetIsUpdateComplete(GameEvent gameEvent) { return true; }
    public void FinishEvent(GameEvent gameEvent) { }
    public void ResetEvent(GameEvent gameEvent) { }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
    }
}
