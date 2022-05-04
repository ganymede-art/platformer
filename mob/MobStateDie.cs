using Assets.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Script.GameExtensionMethods;

public class MobStateDie : MonoBehaviour, IMobState
{
    const float STATE_INTERVAL = 1.5F;

    private float stateInterval = 0.0F;

    [Header("State Attributes")]
    public string stateId;

    [Header("Animation Attributes")]
    public string animationTrigger;

    [Header("Random Drop Attributes")]
    public RandomDropData[] randomDropDatas;
    public Vector3 randomDropSpawnOffset;

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
        // if there are any random drops, spawn them.

        if (randomDropDatas != null && randomDropDatas.Length > 0)
        {
            foreach (var randomDropData in randomDropDatas)
            {
                float dropChance = UnityEngine.Random.Range(0.0F, 1.0F);
                if (dropChance <= randomDropData.randomDropChance)
                    Instantiate(randomDropData.randomDropPrefab
                        , this.transform.position + randomDropSpawnOffset
                        , this.transform.rotation);
            }
        }

        GameObject.Destroy(mc.gameObject);
    }

    public void FixedUpdateState(MobController mc) { }

    public string GetStateId()
    {
        return stateId;
    }


}