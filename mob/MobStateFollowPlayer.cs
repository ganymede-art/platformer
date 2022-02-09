using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.script.GameConstants;

public class MobStateFollowPlayer : MonoBehaviour, IMobState
{
    const float TURNING_AROUND_INTERVAL = 1.0F;

    private GameObject playerObject;
    private Vector3 followDirection;
    private float distanceToPlayer;

    private bool isWallCheckAvailable;
    private MobBehaviourWallCheck behaviourWallCheck;

    [Header("State Attributes")]
    public string stateId;
    public string[] nextStates;
    public string[] hitWallStates;

    [Header("Wander Attributes")]
    public float minFollowDistance;
    public float maxFollowDistance;
    public float velocityChange;
    public float maxSpeed;

    [Header("Animation Attributes")]
    public string animationTrigger;

    private void Start()
    {
        playerObject = GameMasterController.GlobalPlayerObject;
    }

    public void BeginState(MobController mc, params object[] parameters)
    {
        followDirection = playerObject.transform.position - this.gameObject.transform.position;
        followDirection.y = 0.0F;

        // get wall checker, if available.
        if(mc.behaviours.ContainsKey(MOB_BEHAVIOUR_WALL_CHECK))
        {
            isWallCheckAvailable = true;
            behaviourWallCheck = mc.behaviours[MOB_BEHAVIOUR_WALL_CHECK] as MobBehaviourWallCheck;
        }

        // play animation.
        mc.mobAnimator.ResetAllAnimatorTriggers();
        if (animationTrigger == string.Empty)
            mc.mobAnimator.SetTrigger("move");
        else
            mc.mobAnimator.SetTrigger(animationTrigger);
    }

    public void FinishState(MobController mc) {}

    public void FixedUpdateState(MobController mc)
    {
        if(mc.mobRigidBody.velocity.magnitude < maxSpeed
            && distanceToPlayer > minFollowDistance)
        {
            mc.mobRigidBody.AddForce(followDirection * velocityChange, ForceMode.VelocityChange);
        }
    }

    public string GetStateId()
    {
        return stateId;
    }

    public void UpdateState(MobController mc)
    {
        distanceToPlayer = Vector3.Distance
            (gameObject.transform.position, playerObject.transform.position);

        followDirection = playerObject.transform.position - this.gameObject.transform.position;
        followDirection.y = 0.0F;

        if (distanceToPlayer > maxFollowDistance)
        {
            int nextStateIndex = Random.Range(0, nextStates.Length);
            mc.ChangeState(nextStates[nextStateIndex]);
        }

        if (isWallCheckAvailable && behaviourWallCheck.isRaycastHit)
        {
            int nextStateIndex = Random.Range(0, hitWallStates.Length);
            mc.ChangeState(hitWallStates[nextStateIndex]);
        }

        MobStaticMethods.UpdateInternalDirection(mc, followDirection, 5.0F);
        MobStaticMethods.UpdateRendererDirection(mc, followDirection, 5.0F);
    }
}
