using Assets.script;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    private string eventGuid;

    [Header("Event Attributes")]
    public GameObject eventSource;
    public string gameState;
    public bool isOrdered;
    public bool isPriority;

    [Header("Variable Attributes")]
    public bool isOneShot;
    public string oneShotVariableName;

    private void Start()
    {
        eventGuid = Guid.NewGuid().ToString();

        if (gameState == string.Empty)
            gameState = GameConstants.GAME_STATE_CUTSCENE;
    }

    public void StartGameEvent()
    {
        // destroy if one shot and
        // variables is set.
        if (isOneShot)
        {

            bool isSet = GameDataController.Global.GetGameVarBool(oneShotVariableName);

            if (isSet)
                return;
        }

        // create the event.
        var gameEvent = new GameEvent(gameState, eventSource);

        // override its default guid with the guid of this trigger.
        gameEvent.eventGuid = eventGuid;

        // add the event.
        if (isOrdered)
        {
            if (isPriority)
                GameEventController.Global.InsertOrderedGameEvent(gameEvent);
            else
                GameEventController.Global.AddOrderedGameEvent(gameEvent);
        }
        else
        {
            GameEventController.Global.AddGeneralGameEvent(gameEvent);
        }

        if (isOneShot)
            GameMasterController.Global
                .dataController.UpdateGameVar(oneShotVariableName, true);
    }
}
