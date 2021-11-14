using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EventSaveGameController : MonoBehaviour, IEventController
{
    private GameEvent parentEvent;
    public GameEvent ParentEvent
    {
        get => parentEvent;
        set => parentEvent = value;
    }

    private GameMasterController master;

    [FormerlySerializedAs("next_event_source")]
    public GameObject nextEventSource = null;
    [FormerlySerializedAs("save_player_origin")]
    public string savePlayerOrigin;
    [FormerlySerializedAs("save_camera_origin")]
    public string saveCameraOrigin;

    private void Start()
    {
        master = GameMasterController.Global;
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_SAVE_GAME;
    }

    public string GetEventDescription()
    {
        return GetEventType();
    }

    public void StartEvent(GameEvent gameEvent)
    {
        // save current data.

        master.dataController.SaveData(savePlayerOrigin, saveCameraOrigin);
    }

    public void ProcessEvent(GameEvent gameEvent)
    {
        return;
    }

    public bool GetIsProcessComplete(GameEvent gameEvent)
    {
        return true;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return true;
    }

    public bool GetIsGameEventComplete(GameEvent gameEvent)
    {
        return true;
    }

    public void FinishEvent(GameEvent gameEvent) { }
    public void ResetEvent(GameEvent gameEvent) { }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
    }

}
