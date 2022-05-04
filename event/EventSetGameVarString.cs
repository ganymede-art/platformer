using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Script.GameConstants;

public class EventSetGameVarString : MonoBehaviour, IEventController
{
    [Header("Event Attributes")]
    public GameObject nextEventSource;

    [Header("Var Attributes")]
    public string variableName;
    public string variableValue;
    public bool isAppend;

    public void StartEvent(GameEvent gameEvent)
    {
        if(isAppend)
        {
            GameDataController.Global.AppendGameVar(variableName, variableValue);
        }
        else
        {
            GameDataController.Global.UpdateGameVar(variableName, variableValue);
        }
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventDescription()
        { return EVENT_TYPE_SET_GAME_VAR_STRING + "_" + variableName + "_" + variableValue; }

    public string GetEventType()
        { return EVENT_TYPE_SET_GAME_VAR_STRING; }

    public bool GetIsEventComplete(GameEvent gameEvent)
        { return true; }

    public bool GetIsUpdateComplete(GameEvent gameEvent)
        { return true; }

    public void ResetEvent(GameEvent gameEvent) { }
    public void UpdateEvent(GameEvent gameEvent) { }
    public void FinishEvent(GameEvent gameEvent) { }
}
