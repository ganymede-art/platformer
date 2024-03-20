using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    // Private fields.
    private float statusTimer;
    private NpcStatus activeStatus;
    private NpcStatus previousStatus;

    private bool isTargetInRange;
    private Vector3 trackingTargetPosition;
    private Vector3 trackingDirection;
    private Vector3 resetDirection;

    // Public properties.
    public float DistanceToPlayer => Vector3.Distance
        ( transform.position, ActiveSceneHighLogic.G.CachedPlayerObject.transform.position);
    public float DistanceToTarget => Vector3.Distance
        (transform.position, trackingTargetObject.transform.position);

    // Public fields.
    public bool doTrackPlayer;
    public bool doTrackTarget;
    public bool doReset;
    public float resetInterval;
    public GameObject trackingTargetObject;
    [Space]
    public float trackingRange;
    public float trackingSpeed;
    [Space]
    public Animator npcAnimator;
    public AnimatorTriggerIdConstant animatorTriggerId;
    [Space]
    public bool doUpdateInFilm;

    private void Awake()
    {
        StateHighLogic.G.HighLogicStateChanged += OnHighLogicStateChanged;
    }

    private void OnDestroy()
    {
        if (StateHighLogic.G != null)
            StateHighLogic.G.HighLogicStateChanged -= OnHighLogicStateChanged;
    }

    private void Start()
    {
        if(animatorTriggerId != null && npcAnimator != null)
        {
            npcAnimator.ResetAllAnimatorTriggers();
            npcAnimator.SetTrigger(animatorTriggerId.AnimatorTriggerId);
        }

        resetDirection = transform.forward;
        activeStatus = NpcStatus.Idle;
        previousStatus = NpcStatus.Idle;
        ChangeStatus(NpcStatus.Idle);
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play 
            && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        if (StateHighLogic.G.ActiveState == HighLogicStateId.Film 
            && !doUpdateInFilm)
            return;

        if(activeStatus == NpcStatus.Idle)
        {
            if (doTrackPlayer && DistanceToPlayer < trackingRange)
            {
                ChangeStatus(NpcStatus.TrackingPlayer);
                return;
            }
            if(doTrackTarget && DistanceToTarget < trackingRange)
            {
                ChangeStatus(NpcStatus.TrackingTarget);
                return;
            }
        }
        else if(activeStatus == NpcStatus.Reset)
        {
            if(statusTimer >= resetInterval)
            {
                ChangeStatus(NpcStatus.Idle);
                return;
            }

            NpcStatics.UpdateRendererDirection(this, resetDirection, trackingSpeed);
        }
        else if(activeStatus == NpcStatus.TrackingPlayer)
        {
            if(DistanceToPlayer > trackingRange)
            {
                if(doReset)
                {
                    ChangeStatus(NpcStatus.Reset);
                    return;
                }
                else
                {
                    ChangeStatus(NpcStatus.Idle);
                    return;
                } 
            }

            trackingTargetPosition = ActiveSceneHighLogic.G.CachedPlayerObject.transform.position;

            isTargetInRange = Vector3.Distance(transform.position, trackingTargetPosition) < trackingRange;

            if (!isTargetInRange)
            {
                return;
            }

            trackingDirection = trackingTargetPosition - transform.position;

            NpcStatics.UpdateRendererDirection(this, trackingDirection, trackingSpeed);
        }
        else if(activeStatus == NpcStatus.TrackingTarget)
        {

        }

        statusTimer += Time.deltaTime;
    }

    private void ChangeStatus(NpcStatus newStatus)
    {
        EndStatus();
        previousStatus = activeStatus;
        activeStatus = newStatus;
        statusTimer = 0.0F;
        BeginStatus();
    }

    private void BeginStatus()
    {

    }

    private void EndStatus()
    {

    }

    public void OnHighLogicStateChanged(object sender, EventArgs e)
    {
        // Animator.
        if (npcAnimator.isActiveAndEnabled)
        {
            if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
                && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
                npcAnimator.enabled = false;
        }
        else
        {
            if (StateHighLogic.G.ActiveState == HighLogicStateId.Play
                || StateHighLogic.G.ActiveState == HighLogicStateId.Film)
                npcAnimator.enabled = true;
        }
    }
}
