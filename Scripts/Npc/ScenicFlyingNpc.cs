using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class ScenicFlyingNpc : MonoBehaviour
{
    // Consts.
    private float MAX_FLIGHT_INTERVAL = 7.0F;

    // Private fields.
    private ScenicFlyingNpcStatus activeStatus;
    private ScenicFlyingNpcStatus previousStatus;
    private float statusTimer;
    private float flightInterval;
    private float flightSpeed;
    private float flightTurningSpeed;
    private NavigationNode targetNode;

    // Public fields.
    public Renderer npcRenderer;
    public Animator npcAnimator;
    public float minStatusInterval;
    public float maxStatusInterval;
    public float minFlightSpeed;
    public float maxFlightSpeed;
    public float minTurningSpeed;
    public float maxTurningSpeed;
    public float minDistToTargetNode;
    public NavigationNode startingNode;

    void Start()
    {
        StateHighLogic.G.HighLogicStateChanged += OnHighLogicStateChanged;

        targetNode = startingNode;
        transform.position = targetNode.transform.position;

        activeStatus = ScenicFlyingNpcStatus.Flying;
        previousStatus = ScenicFlyingNpcStatus.Flying;
        ChangeStatus(ScenicFlyingNpcStatus.Flying);

        npcAnimator.ResetAllAnimatorTriggers();
        npcAnimator.SetTrigger(ANIMATION_TRIGGER_FLY);
    }

    private void OnDestroy()
    {
        if (StateHighLogic.G == null)
            return;
        StateHighLogic.G.HighLogicStateChanged -= OnHighLogicStateChanged;
    }

    void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        if(activeStatus == ScenicFlyingNpcStatus.Flying)
        {
            Vector3 relativePos = targetNode.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * flightTurningSpeed);

            float perc = statusTimer / flightInterval;
            perc = Math.Clamp(perc, 0.0F, 1.0F);

            transform.position = Vector3.MoveTowards(transform.position, transform.TransformPoint(Vector3.forward), (flightSpeed * (1-perc)) * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetNode.transform.position, (flightSpeed * perc) * Time.deltaTime);

            if (Vector3.Distance(targetNode.transform.position, transform.position) < minDistToTargetNode)
            {
                OnReachTargetNode();
                return;
            }
        }
        else if(activeStatus == ScenicFlyingNpcStatus.Perching)
        {
            if (statusTimer >= flightInterval)
            {
                ChangeStatus(ScenicFlyingNpcStatus.Flying);
                return;
            }
        }
        
        statusTimer += Time.deltaTime;
    }

    private void SetRandomFlightProperties()
    {
        flightInterval = UnityEngine.Random.Range(minStatusInterval, maxStatusInterval);
        flightSpeed = UnityEngine.Random.Range(minFlightSpeed, maxFlightSpeed);
        flightTurningSpeed = UnityEngine.Random.Range(minTurningSpeed, maxTurningSpeed);
    }

    private void OnReachTargetNode()
    {
        if(targetNode.navigationNodeType.NavigationNodeType == NavigationNodeType.Perch)
        {
            ChangeStatus(ScenicFlyingNpcStatus.Perching);
        }
        else if(targetNode.navigationNodeType.NavigationNodeType == NavigationNodeType.Fly)
        {
            ChangeStatus(ScenicFlyingNpcStatus.Flying);
        }
    }

    private void ChangeStatus(ScenicFlyingNpcStatus newStatus)
    {
        EndStatus();
        previousStatus = activeStatus;
        activeStatus = newStatus;
        statusTimer = 0.0F;
        BeginStatus();
    }

    private void BeginStatus()
    {
        if(activeStatus == ScenicFlyingNpcStatus.Flying)
        {
            npcAnimator.ResetAllAnimatorTriggers();
            npcAnimator.SetTrigger(ANIMATION_TRIGGER_FLY);
            SetRandomFlightProperties();
            SetRandomTargetNode();
        }
        else if(activeStatus == ScenicFlyingNpcStatus.Perching)
        {
            npcAnimator.ResetAllAnimatorTriggers();
            npcAnimator.SetTrigger(ANIMATION_TRIGGER_IDLE);
            SetRandomFlightProperties();
        }
    }

    private void EndStatus()
    {
        if (activeStatus == ScenicFlyingNpcStatus.Flying)
        {

        }
        else if (activeStatus == ScenicFlyingNpcStatus.Perching)
        {

        }
    }

    private void SetRandomTargetNode()
    {
        int nextNodeIndex = UnityEngine.Random.Range(0, targetNode.nextNavigationNodes.Length);
        targetNode = targetNode.nextNavigationNodes[nextNodeIndex];
    }

    private void OnHighLogicStateChanged(object sender, EventArgs e)
    {
        npcAnimator.enabled 
            = (StateHighLogic.G.ActiveState == HighLogicStateId.Play
            || StateHighLogic.G.ActiveState == HighLogicStateId.Film);
    }
}
