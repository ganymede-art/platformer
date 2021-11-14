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
            
            bool isSet = GameDataController.Global.GetGameVarBool(oneShotVariableName);

            if (isSet)
            {
                Debug.Log(oneShotVariableName + " was set, deleting.");
                GameObject.Destroy(gameObject);
                return;
            }


        }

        var gameEvent = new GameEvent(gameState, eventSource);

        if (isOrdered)
        {
            if (isPriority)
                GameMasterController.Global
                    .cutsceneController.InsertOrderedGameEvent(gameEvent);
            else
                GameMasterController.Global
                    .cutsceneController.AddOrderedGameEvent(gameEvent);
        }
        else
        {
            GameMasterController.Global
                .cutsceneController.AddGeneralGameEvent(gameEvent);
        }

        if (isOneShot)
            GameMasterController.Global
                .dataController.UpdateGameVar(oneShotVariableName, true);
    }
}
