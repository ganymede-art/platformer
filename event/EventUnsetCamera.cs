using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class EventUnsetCamera : MonoBehaviour, IEventController
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

    void Start()
    {
        master = GameMasterController.Global;
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_UNSET_CAMERA;
    }

    public string GetEventDescription()
    {
        return GetEventType();
    }

    public void StartEvent(GameEvent gameEvent)
    {
        var player_camera_object = GameMasterController.GlobalCameraObject;
        player_camera_object.GetComponent<CameraController>().UnsetCamera();
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        var player_camera_object = GameMasterController.GlobalCameraObject;
        float fixed_transition = player_camera_object.GetComponent<CameraController>().transitionProgress;

        return fixed_transition >= 1.0f;
    }

    public bool GetIsUpdateComplete(GameEvent gameEvent)
    {
        return GetIsEventComplete(gameEvent);
    }

    public void UpdateEvent(GameEvent gameEvent) { }
    public void FinishEvent(GameEvent gameEvent) { }
    public void ResetEvent(GameEvent gameEvent) { }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
    }
}
