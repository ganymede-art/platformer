using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Script.GameExtensionMethods;

public class MobStateIdle : MonoBehaviour, IMobState
{
    private float stateTimer = 0.0F;
    private float stateInterval = 0.0F;

    [Header("State Attributes")]
    public string stateId;
    public string[] nextStates;

    [Header("Wander Attributes")]
    public float minInterval;
    public float maxInterval;

    [Header("Animation Attributes")]
    public string animationTrigger;

    public void BeginState(MobController mc, params object[] parameters)
    {
        stateTimer = 0.0f;
        stateInterval = UnityEngine.Random.Range(minInterval, maxInterval);

        // play animation.
        mc.mobAnimator.ResetAllAnimatorTriggers();
        if (animationTrigger == string.Empty)
            mc.mobAnimator.SetTrigger("idle");
        else
            mc.mobAnimator.SetTrigger(animationTrigger);
    }

    public void FinishState(MobController mc) { }
    public void FixedUpdateState(MobController mc) { }

    public string GetStateId()
    {
        return stateId;
    }

    public void UpdateState(MobController mc)
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= stateInterval)
        {
            int nextStateIndex = UnityEngine.Random.Range(0, nextStates.Length);
            mc.ChangeState(nextStates[nextStateIndex]);
        }
    }
}