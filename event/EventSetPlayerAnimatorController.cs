using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class EventSetPlayerAnimatorController : MonoBehaviour, IEventController
{
    private GameMasterController master;

    [FormerlySerializedAs("next_event_source")]
    public GameObject nextEventSource = null;
    public string trigger = string.Empty;

    void Start()
    {
        master = GameMasterController.GlobalMasterController;
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_SET_PLAYER_ANIMATOR;
    }

    public void StartEvent()
    {
        var player = GameMasterController.GlobalPlayerObject;
        var animator = player.GetComponentInChildren<Animator>();
        animator.SetTrigger(trigger);
    }

    public void ProcessEvent()
    {
        return;
    }

    public bool GetIsEventComplete()
    {
        return true;
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
