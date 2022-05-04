using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using static Assets.Script.GameConstants;

public class MobStateSwimWander : MonoBehaviour, IMobState
{
    const float TURNING_AROUND_INTERVAL = 1.0F;

    private float wanderX;
    private float wanderY;
    private float wanderZ;
    private Vector3 wanderDirection;

    private float stateTimer = 0.0F;
    private float stateInterval = 0.0F;

    private bool isWallCheckAvailable;
    private MobBehaviourWallCheck behaviourWallCheck;
    private bool isWaterBehaviourAvailable;
    private MobBehaviourWater behaviourWater;

    private bool isTurningAround = false;
    private float turningAroundTimer = 0.0F;

    [Header("State Attributes")]
    public string stateId;
    public string[] nextStates;

    [Header("Wander Attributes")]
    public float minInterval;
    public float maxInterval;
    public float velocityChange;
    public float maxSpeed;

    [Header("Animation Attributes")]
    public string animationTrigger;

    public void BeginState(MobController mc, params object[] parameters)
    {
        // disable gravity.
        mc.mobRigidBody.useGravity = false;

        // disable friction.
        mc.mobCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        mc.mobCollider.material.dynamicFriction = 0.0F;
        mc.mobCollider.material.staticFriction = 0.0F;
        mc.mobRigidBody.drag = 1.0F;

        stateTimer = 0.0f;
        stateInterval = Random.Range(minInterval, maxInterval);

        wanderX = Random.Range(-1.0F, 1.0F);
        wanderY = Random.Range(-0.5F, 0.5F);
        wanderZ = Random.Range(-1.0F, 1.0F);
        wanderDirection = new Vector3(wanderX, wanderY, wanderZ).normalized;

        // set turning around timer.

        isTurningAround = false;
        turningAroundTimer = 0.0F;

        // get wall checker, if available.
        if (mc.behaviours.ContainsKey(MOB_BEHAVIOUR_WALL_CHECK))
        {
            isWallCheckAvailable = true;
            behaviourWallCheck = mc.behaviours[MOB_BEHAVIOUR_WALL_CHECK] as MobBehaviourWallCheck;
        }

        // get water checker, if available.
        if(mc.behaviours.ContainsKey(MOB_BEHAVIOUR_WATER))
        {
            isWaterBehaviourAvailable = true;
            behaviourWater = mc.behaviours[MOB_BEHAVIOUR_WATER] as MobBehaviourWater;
        }

        // play animation.
        mc.mobAnimator.ResetAllAnimatorTriggers();
        if (animationTrigger == string.Empty)
            mc.mobAnimator.SetTrigger("swim");
        else
            mc.mobAnimator.SetTrigger(animationTrigger);
    }

    public void FinishState(MobController mc)
    {
        // enable gravity.
        mc.mobRigidBody.useGravity = true;

        // restore collider material.
        mc.RestoreOriginalColliderMaterial();
        mc.RestoreOriginalRigidBodyProperties();
    }

    public void FixedUpdateState(MobController mc)
    {
        if (isWaterBehaviourAvailable)
        {
            if (behaviourWater.isFullSubmerged && mc.mobRigidBody.useGravity)
                mc.mobRigidBody.useGravity = false;
            else if (!behaviourWater.isFullSubmerged && !mc.mobRigidBody.useGravity)
                mc.mobRigidBody.useGravity = true;
        }

        if(isWaterBehaviourAvailable && (behaviourWater.waterYLevel - mc.mobRigidBody.transform.position.y) < 0.5F)
        {
            wanderDirection.y -= 0.01F;
        }

        if
        (
            mc.mobRigidBody.velocity.magnitude < maxSpeed
            && (!isWaterBehaviourAvailable || behaviourWater.isFullSubmerged)
        )
        {
            mc.mobRigidBody.AddForce(wanderDirection * velocityChange, ForceMode.VelocityChange);
        }

        
    }

    public string GetStateId()
    {
        return stateId;
    }

    public void UpdateState(MobController mc)
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= stateInterval)
        {
            int nextStateIndex = Random.Range(0, nextStates.Length);
            mc.ChangeState(nextStates[nextStateIndex]);
        }

        if (isWallCheckAvailable && behaviourWallCheck.isRaycastHit && !isTurningAround)
        {
            isTurningAround = true;
            wanderDirection = -wanderDirection;
        }

        if (isTurningAround)
            turningAroundTimer += Time.deltaTime;

        if (turningAroundTimer >= TURNING_AROUND_INTERVAL)
        {
            isTurningAround = false;
            turningAroundTimer = 0.0f;
        }

        MobStaticMethods.UpdateInternalDirection(mc, wanderDirection, 5.0F);
        MobStaticMethods.UpdateRendererDirection(mc, wanderDirection, 5.0F);
    }
}
