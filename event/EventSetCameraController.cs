using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class EventSetCameraController : MonoBehaviour, IEventController
{
    private GameMasterController master;

    [FormerlySerializedAs("next_event_source")]
    public GameObject nextEventSource = null;

    public bool isTracking;
    public bool isInstant;
    public Transform fixedTransform;

    void Start()
    {
        master = GameMasterController.GetMasterController();
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_SET_CAMERA;
    }

    public void StartEvent()
    {
        var player_camera_object = GameMasterController.GetPlayerCameraObject();

        player_camera_object.GetComponent<CameraController>()
            .SetFixedCamera(fixedTransform, isTracking, isInstant);
    }

    public void ProcessEvent()
    {
        return;
    }

    public bool GetIsEventComplete()
    {
        var player_camera_object = GameMasterController.GetPlayerCameraObject();
        float fixed_transition = player_camera_object.GetComponent<CameraController>().Fixed_Transition;

        return fixed_transition >= 1.0f;
    }

    public bool GetIsProcessComplete()
    {
        return GetIsEventComplete();
    }

    public bool GetIsGameEventComplete()
    {
        return GetIsEventComplete();
    }

    public void FinishEvent()
    {
        return;
    }
}
