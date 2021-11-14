using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class EventSetCameraController : MonoBehaviour, IEventController
{
    const float TRANSITION_DURATION_MIN = 0.01f;
    const float TRANSITION_DURATION_DEFAULT = 2.0f;

    private GameEvent parentEvent;
    public GameEvent ParentEvent
    {
        get => parentEvent;
        set => parentEvent = value;
    }

    private GameMasterController master;

    [FormerlySerializedAs("next_event_source")]
    public GameObject nextEventSource = null;

    public bool isTracking;
    public bool isInstant;
    [FormerlySerializedAs("transitionSpeed")]
    public float transitionDuration;
    public Transform fixedTransform;

    void Start()
    {
        master = GameMasterController.Global;

        if (transitionDuration < TRANSITION_DURATION_MIN)
            transitionDuration = TRANSITION_DURATION_DEFAULT;
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_SET_CAMERA;
    }

    public string GetEventDescription()
    {
        return GameConstants.EVENT_TYPE_SET_CAMERA + ((fixedTransform == null) ? "_null" : "_" + fixedTransform.name);
    }

    public void StartEvent(GameEvent gameEvent)
    {
        var player_camera_object = GameMasterController.GlobalCameraObject;

        player_camera_object.GetComponent<CameraController>()
            .SetFixedCamera(fixedTransform, isTracking, isInstant, transitionDuration);
    }

    public void ProcessEvent(GameEvent gameEvent)
    {
        return;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        var player_camera_object = GameMasterController.GlobalCameraObject;
        float fixed_transition = player_camera_object.GetComponent<CameraController>().transitionProgress;

        return fixed_transition >= 1.0f;
    }

    public bool GetIsProcessComplete(GameEvent gameEvent)
    {
        return GetIsEventComplete(gameEvent);
    }

    public bool GetIsGameEventComplete(GameEvent gameEvent)
    {
        return GetIsEventComplete(gameEvent);
    }

    public void FinishEvent(GameEvent gameEvent) { }
    public void ResetEvent(GameEvent gameEvent) { }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource, 
            optionalObject1: fixedTransform.gameObject, optionalColour1: Color.green, optionalIcon1: "ev_cam.png");
    }
}
