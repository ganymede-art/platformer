using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [Header("Event Attributes")]
    public GameObject eventSource;
    public string eventGuid;
    public string gameState;
    public bool isOrdered;
    public bool isPriority;

    [Header("Variable Attributes")]
    public bool isOneShot;
    public string oneShotVariableName;

    private void Start()
    {
        if (eventGuid == null || eventGuid == string.Empty)
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
        var gameEvent = new GameEvent(eventGuid, gameState, eventSource);


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
