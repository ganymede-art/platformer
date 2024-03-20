using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectAction : MonoBehaviour, IAction
{
    private float actionTimer;
    private float actionInterval;
    private float actionProgress;

    private float moveProgress;
    private float rotateProgress;
    private float scaleProgress;

    private Vector3 startPosition;
    private Vector3 finishPosition;
    private Quaternion startRotation;
    private Quaternion finishRotation;
    private Vector3 startScale;
    private Vector3 finishScale;

    private Rigidbody moveRigidBody;

    // Public fields
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Object Attributes")]
    public GameObject moveObject;
    public bool isMoveObjectKinematic;

    [Header("Move Attributes")]
    public Transform startTransform;
    public Transform finishTransform;
    public bool doMove;
    public bool doRotate;
    public bool doScale;
    public float moveInterval;
    public float rotateInterval;
    public float scaleInterval;
    public bool isMoveSmooth;
    public bool isRotateSmooth;
    public bool isScaleSmooth;

    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.MoveObject;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => actionProgress >= 1.0F;
    public bool IsActionUpdateComplete => actionProgress >= 1.0F;

    public void BeginAction(ActionSource actionSource)
    {
        actionTimer = 0.0F;
        actionInterval = Mathf.Max(moveInterval, rotateInterval, scaleInterval);
        actionProgress = 0.0F;

        moveProgress = 0.0F;
        rotateProgress = 0.0F;
        scaleProgress = 0.0F;

        startPosition = startTransform == null 
            ? moveObject.transform.position 
            : startTransform.position;
        finishPosition = finishTransform.position;
        startRotation = startTransform == null
            ? moveObject.transform.rotation
            : startTransform.rotation;
        finishRotation = finishTransform.rotation;
        startScale = startTransform == null
            ? moveObject.transform.localScale
            : startTransform.localScale;
        finishScale = finishTransform.localScale;

        if (isMoveObjectKinematic)
            moveRigidBody = moveObject.GetComponent<Rigidbody>();
    }
    public void UpdateAction(ActionSource actionSource)
    {
        actionTimer += Time.deltaTime;

        moveProgress = Mathf.InverseLerp(0, moveInterval, actionTimer);
        rotateProgress = Mathf.InverseLerp(0, rotateInterval, actionTimer);
        scaleProgress = Mathf.InverseLerp(0, scaleInterval, actionTimer);
        actionProgress = Mathf.InverseLerp(0, actionInterval, actionTimer);

        float moveLerp = moveProgress;
        float rotateLerp = rotateProgress;
        float scaleLerp = scaleProgress;

        if(isMoveSmooth)
            moveLerp = Mathf.SmoothStep(0.0F, 1.0F, moveProgress);

        if (isRotateSmooth)
            rotateLerp = Mathf.SmoothStep(0.0F, 1.0F, rotateProgress);

        if (isScaleSmooth)
            scaleLerp = Mathf.SmoothStep(0.0F, 1.0F, scaleProgress);

        moveProgress = Mathf.Clamp(moveProgress, 0.0F, 1.0F);
        rotateProgress = Mathf.Clamp(rotateProgress, 0.0F, 1.0F);
        scaleProgress = Mathf.Clamp(scaleProgress, 0.0F, 1.0F);
        actionProgress = Mathf.Clamp(actionProgress, 0.0F, 1.0F);

        if (isMoveObjectKinematic)
        {
            if(doMove)
                moveRigidBody.MovePosition(Vector3.Lerp(startPosition, finishPosition, moveLerp));
            if (doRotate)
                moveRigidBody.MoveRotation(Quaternion.Lerp(startRotation, finishRotation, rotateLerp));
        }
        else
        {
            if (doMove)
                moveObject.transform.position 
                    = Vector3.Lerp(startPosition, finishPosition, moveLerp);
            if (doRotate)
                moveObject.transform.rotation 
                    = Quaternion.Lerp(startRotation, finishRotation, rotateLerp);
            if (doScale)
                moveObject.transform.localScale 
                    = Vector3.Lerp(startScale, finishScale, scaleLerp);
        }
    }

    public void EndAction(ActionSource actionSource) { }
}
