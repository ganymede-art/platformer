using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class EventUnsetCameraController : MonoBehaviour, IEventController
{
    private GameMasterController master;

    [FormerlySerializedAs("next_event_source")]
    public GameObject nextEventSource = null;

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
        return GameConstants.EVENT_TYPE_UNSET_CAMERA;
    }

    public void StartEvent()
    {
        var player_camera_object = GameMasterController.GetPlayerCameraObject();
        player_camera_object.GetComponent<CameraController>().UnsetCamera();
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
