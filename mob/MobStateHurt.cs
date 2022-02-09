using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.script.GameExtensionMethods;

public class MobStateHurt : MonoBehaviour, IMobState
{
    const float STATE_INTERVAL = 1.5F;

    private float stateInterval = 0.0F;

    [Header("State Attributes")]
    public string stateId;
    public string[] nextStates;

    [Header("Animation Attributes")]
    public string animationTrigger;

    public void BeginState(MobController mc, params object[] parameters)
    {
        stateInterval = STATE_INTERVAL;

        // disable the hitboxes.
        foreach (var hitboxObject in mc.hitboxObjects)
            hitboxObject.SetActive(false);

        // play animation.
        mc.mobAnimator.ResetAllAnimatorTriggers();
        if (animationTrigger == string.Empty)
            mc.mobAnimator.SetTrigger("damage_up");
        else
            mc.mobAnimator.SetTrigger(animationTrigger);

        // get the damage source object, and add a force opposite.

        GameObject damageSourceObject = parameters[0] as GameObject;

        Vector3 damageVector = (transform.position - damageSourceObject.transform.position).normalized;
        damageVector.y = 0.0F;

        mc.mobCollider.material.dynamicFriction = 0.0F;
        mc.mobCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        mc.mobRigidBody.AddForce(damageVector.normalized * 4, ForceMode.VelocityChange);
        mc.mobRigidBody.AddForce(Vector3.up * 4, ForceMode.VelocityChange);
    }

    public void FinishState(MobController mc)
    {
        // enable the hitboxes.
        foreach (var hitboxObject in mc.hitboxObjects)
            hitboxObject.SetActive(true);
    }

    public void FixedUpdateState(MobController mc) { }

    public string GetStateId()
    {
        return stateId;
    }

    public void UpdateState(MobController mc)
    {
        if (mc.stateTimer >= stateInterval)
        {
            int nextStateIndex = UnityEngine.Random.Range(0, nextStates.Length);
            mc.ChangeState(nextStates[nextStateIndex]);
        }
    }
}