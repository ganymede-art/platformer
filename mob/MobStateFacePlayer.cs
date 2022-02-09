using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.script.GameConstants;

public class MobStateFacePlayer : MonoBehaviour, IMobState
{
    const float TURNING_AROUND_INTERVAL = 1.0F;

    private GameObject playerObject;
    private float distanceToPlayer;

    private Vector3 facingDirection;

    [Header("State Attributes")]
    public string stateId;
    public string[] nextStates;

    [Header("Facing Attributes")]
    public float maxFacingDistance;

    [Header("Animation Attributes")]
    public string animationTrigger;

    private void Start()
    {
        playerObject = GameMasterController.GlobalPlayerObject;
    }

    public void BeginState(MobController mc, params object[] parameters)
    {
        // play animation.
        mc.mobAnimator.ResetAllAnimatorTriggers();
        if (animationTrigger == string.Empty)
            mc.mobAnimator.SetTrigger("idle");
        else
            mc.mobAnimator.SetTrigger(animationTrigger);
    }

    public void FinishState(MobController mc) {}

    public void FixedUpdateState(MobController mc)
    {
        distanceToPlayer = Vector3.Distance
            (gameObject.transform.position, playerObject.transform.position);
    }

    public string GetStateId()
    {
        return stateId;
    }

    public void UpdateState(MobController mc)
    {
        facingDirection = playerObject.transform.position - this.gameObject.transform.position;
        facingDirection.y = 0.0F;

        if (distanceToPlayer > maxFacingDistance)
        {
            int nextStateIndex = Random.Range(0, nextStates.Length);
            mc.ChangeState(nextStates[nextStateIndex]);
        }

        MobStaticMethods.UpdateInternalDirection(mc, facingDirection, 5.0F);
        MobStaticMethods.UpdateRendererDirection(mc, facingDirection, 5.0F);
    }
}
