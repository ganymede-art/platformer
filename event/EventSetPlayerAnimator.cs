using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;

public class EventSetPlayerAnimator : MonoBehaviour, IEventController
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
    public string trigger = string.Empty;
    public bool doSetSpeed;
    public float speed;

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
        return GameConstants.EVENT_TYPE_SET_PLAYER_ANIMATOR;
    }

    public string GetEventDescription()
    {
        return GetEventType();
    }

    public void StartEvent(GameEvent gameEvent)
    {
        var player = GameMasterController.GlobalPlayerObject;
        var animator = player.GetComponentInChildren<Animator>();
        animator.ResetAllAnimatorTriggers();
        animator.SetTrigger(trigger);

        if (doSetSpeed)
            animator.SetFloat("speed_multiplier", speed);
    }

    public void UpdateEvent(GameEvent gameEvent)
    {
        return;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return true;
    }

    public bool GetIsUpdateComplete(GameEvent gameEvent)
    {
        return GetIsEventComplete(gameEvent);
    }

    public void FinishEvent(GameEvent gameEvent)
    {
        return;
    }

    public void ResetEvent(GameEvent gameEvent) { }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
    }
}
