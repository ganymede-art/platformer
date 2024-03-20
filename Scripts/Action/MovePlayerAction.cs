using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerAction : MonoBehaviour, IAction
{
    // Consts.
    private static readonly Vector3 PLAYER_FEET_OFFSET = new Vector3(0.0F, 0.1875F, 0.0F);

    // Private fields.
    private float actionTimer;
    private float actionInterval;
    private float actionProgress;
    private float moveProgress;
    private Vector3 startPosition;
    private Vector3 finishPosition;

    // Public fields
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Move Attributes")]
    public Transform startTransform;
    public Transform finishTransform;
    public float moveInterval;
    public bool isMoveSmooth;

    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.MovePlayer;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => actionProgress >= 1.0F;
    public bool IsActionUpdateComplete => actionProgress >= 1.0F;

    public void BeginAction(ActionSource actionSource)
    {
        actionTimer = 0.0F;
        actionInterval = moveInterval;
        actionProgress = 0.0F;

        moveProgress = 0.0F;

        startPosition = startTransform == null 
            ? ActiveSceneHighLogic.G.CachedPlayerObject.transform.position 
            : startTransform.position + PLAYER_FEET_OFFSET;
        finishPosition = finishTransform.position + PLAYER_FEET_OFFSET;
    }
    public void UpdateAction(ActionSource actionSource)
    {
        actionTimer += Time.deltaTime;

        moveProgress = Mathf.InverseLerp(0, moveInterval, actionTimer);
        actionProgress = Mathf.InverseLerp(0, actionInterval, actionTimer);

        float moveLerp = moveProgress;

        if(isMoveSmooth)
            moveLerp = Mathf.SmoothStep(0.0F, 1.0F, moveProgress);

        moveProgress = Mathf.Clamp(moveProgress, 0.0F, 1.0F);
        actionProgress = Mathf.Clamp(actionProgress, 0.0F, 1.0F);

        ActiveSceneHighLogic.G.CachedPlayerObject.transform.position 
            = Vector3.Lerp(startPosition, finishPosition, moveLerp);
    }

    public void EndAction(ActionSource actionSource) { }
}
