using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapAutoCutscene : MonoBehaviour
{
    [Header("Event Attributes")]
    [FormerlySerializedAs("event_source")]
    public GameObject eventSource;
    public GameState gameState = GameState.Cutscene;
    public bool isOrdered;
    public bool isPriority;

    [Header("Variable Attributes")]
    public bool isOneShot;
    public string oneShotVariableName;

    void Start()
    {
        // destroy if one shot and
        // variables is set.

        if(isOneShot)
        {
            bool isSet = GameMasterController.GlobalMasterController
                .dataController.GetGameVarBool(oneShotVariableName);

            if (isSet)
            {
                GameObject.Destroy(gameObject);
                return;
            }


        }

        var gameEvent = new GameEvent(gameState, eventSource);

        if (isOrdered)
        {
            if (isPriority)
                GameMasterController.GlobalMasterController
                    .cutsceneController.InsertOrderedGameEvent(gameEvent);
            else
                GameMasterController.GlobalMasterController
                    .cutsceneController.AddOrderedGameEvent(gameEvent);
        }
        else
        {
            GameMasterController.GlobalMasterController
                .cutsceneController.AddGeneralGameEvent(gameEvent);
        }

        if (isOneShot)
            GameMasterController.GlobalMasterController
                .dataController.UpdateGameVar(oneShotVariableName, true);
    }
}
