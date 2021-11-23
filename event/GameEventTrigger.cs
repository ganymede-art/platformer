using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [Header("Event Attributes")]
    public GameObject eventSource;
    public GameState gameState = GameState.Cutscene;
    public bool isOrdered;
    public bool isPriority;

    [Header("Variable Attributes")]
    public bool isOneShot;
    public string oneShotVariableName;

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

        var gameEvent = new GameEvent(gameState, eventSource);

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
