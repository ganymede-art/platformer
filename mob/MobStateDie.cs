using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.script.GameExtensionMethods;

public class MobStateDie : MonoBehaviour, IMobState
{
    const float STATE_INTERVAL = 1.5F;

    private float stateInterval = 0.0F;

    [Header("State Attributes")]
    public string stateId;

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
            mc.mobAnimator.SetTrigger("die");
        else
            mc.mobAnimator.SetTrigger(animationTrigger);
    }

    public void UpdateState(MobController mc)
    {
        if (mc.stateTimer >= stateInterval)
        {
            mc.ChangeState(mc.defaultState);
        }
    }

    public void FinishState(MobController mc)
    {
        GameObject.Destroy(gameObject.transform.root.gameObject);
    }

    public void FixedUpdateState(MobController mc) { }

    public string GetStateId()
    {
        return stateId;
    }


}