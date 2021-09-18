using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSaveGameController : MonoBehaviour, IEventController
{
    private GameMasterController master;

    public GameObject next_event_source = null;
    public string save_player_origin;
    public string save_camera_origin;

    private void Start()
    {
        master = GameMasterController.GetMasterController();
    }

    public GameObject GetNextEventSource()
    {
        return next_event_source;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_SAVE_GAME;
    }

    public void StartEvent()
    {
        // save current data.

        master.data_controller.SaveData(save_player_origin, save_camera_origin);
    }

    public void ProcessEvent()
    {
        return;
    }

    public bool GetIsProcessComplete()
    {
        return true;
    }

    public bool GetIsEventComplete()
    {
        return true;
    }

    public bool GetIsGameEventComplete()
    {
        return true;
    }

    public void FinishEvent()
    {
        return;
    }
}
