using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Script.GameConstants;

public class EventMovePlayerPosition : MonoBehaviour, IEventController
{
    const float MOVE_DURATION_MIN = 0.1F;
    const float MOVE_DURATION_DEFAULT = 1.0F;

    private static readonly Vector3 PLAYER_HEIGHT_OFFSET = new Vector3(0.0F, 0.1875F, 0.0F);

    private Vector3 startPosition;
    private Vector3 endPosition;

    private bool isActive = false;

    private float moveTimer = 0.0F;
    private float moveProgess = 0.0F;

    [Header("Event Sources")]
    public GameObject nextEventSource;

    [Header("Move Attributes")]
    public Transform startTransform;
    public Transform endTransform;

    public float moveDuration;

    public bool isOffsetForPlayerHeight;

    public void FinishEvent(GameEvent gameEvent)
    {
        isActive = false;
    }

    public string GetEventDescription()
    {
        return EVENT_TYPE_MOVE_PLAYER_POSITION;
    }

    public string GetEventType()
    {
        return EVENT_TYPE_MOVE_PLAYER_POSITION;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return (moveProgess >= 1.0f);
    }

    public bool GetIsUpdateComplete(GameEvent gameEvent)
    {
        return (moveProgess >= 1.0f);
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public void ResetEvent(GameEvent gameEvent) { }

    public void StartEvent(GameEvent gameEvent)
    {
        // set the start position,
        // no need to offset for height if taken
        // from the player dynamically.

        if (startTransform == null)
        {
            startPosition = GameMasterController.GlobalPlayerObject.transform.position;
        }
        else
        {
            startPosition = startTransform.position;
        
            if (isOffsetForPlayerHeight)
                startPosition += PLAYER_HEIGHT_OFFSET;
        }

        // set the end position.

        endPosition = endTransform.position;

        if(isOffsetForPlayerHeight)
            endPosition += PLAYER_HEIGHT_OFFSET;

        // set the progress.

        moveTimer = 0.0F;
        moveProgess = 0.0F;

        isActive = true;
    }

    public void UpdateEvent(GameEvent gameEvent)
    {
        moveTimer += Time.deltaTime;

        moveProgess = Mathf.InverseLerp(0, moveDuration, moveTimer);
        moveProgess = Mathf.Clamp(moveProgess, 0.0f, 1.0f);

        GameMasterController.GlobalPlayerObject.transform.position 
            = Vector3.Lerp(startPosition, endPosition, moveProgess);
    }

    void Start()
    {
        if (moveDuration < MOVE_DURATION_MIN)
            moveDuration = MOVE_DURATION_DEFAULT;
    }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource,
            optionalObject1: endTransform.gameObject, optionalColour1: Color.red, optionalIcon1: "ev_moveobject.png");
    }
}
