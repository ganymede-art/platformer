using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.script.GameExtensionMethods;

public class MobStateAlert : MonoBehaviour, IMobState
{
    private float stateTimer = 0.0F;
    private float stateInterval = 0.0F;

    private GameObject playerObject;
    private Vector3 alertDirection;


    [Header("State Attributes")]
    public string stateId;
    public string[] nextStates;

    [Header("Alert Attributes")]
    public float alertInterval;

    [Header("Animation Attributes")]
    public string animationTrigger;

    private void Start()
    {
        playerObject = GameMasterController.GlobalPlayerObject;
    }

    public void BeginState(MobController mc, params object[] parameters)
    {
        stateTimer = 0.0f;
        stateInterval = alertInterval;

        // play animation.
        mc.mobAnimator.ResetAllAnimatorTriggers();
        if (animationTrigger == string.Empty)
            mc.mobAnimator.SetTrigger("alert");
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
        alertDirection = playerObject.transform.position - this.gameObject.transform.position;
        alertDirection.y = 0.0F;

        MobStaticMethods.UpdateInternalDirection(mc, alertDirection, 5.0F);
        MobStaticMethods.UpdateRendererDirection(mc, alertDirection, 5.0F);

        stateTimer += Time.deltaTime;

        if (stateTimer >= stateInterval)
        {
            int nextStateIndex = UnityEngine.Random.Range(0, nextStates.Length);
            mc.ChangeState(nextStates[nextStateIndex]);
        }
    }
}