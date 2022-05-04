using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Script.GameConstants;

public class MobStateJumpWander : MonoBehaviour, IMobState
{
    private float wanderX;
    private float wanderZ;
    private Vector3 wanderDirection;

    private float stateTimer = 0.0F;
    private float stateInterval = 0.0F;

    private AudioSource jumpSoundSource;

    [Header("State Attributes")]
    public string stateId;
    public string[] nextStates;

    [Header("Wander Attributes")]
    public float minInterval;
    public float maxInterval;
    public float jumpForce;

    [Header("Animation Attributes")]
    public string animationTrigger;

    [Header("Sound Attributes")]
    public GameObject jumpSoundSourceObject;

    public void BeginState(MobController mc, params object[] parameters)
    {
        stateTimer = 0.0f;
        stateInterval = Random.Range(minInterval, maxInterval);

        wanderX = Random.Range(-1.0F, 1.0F);
        wanderZ = Random.Range(-1.0F, 1.0F);
        wanderDirection = new Vector3(wanderX, 0.0F, wanderZ).normalized;

        // play animation.
        mc.mobAnimator.ResetAllAnimatorTriggers();
        if (animationTrigger == string.Empty)
            mc.mobAnimator.SetTrigger("jump_up");
        else
            mc.mobAnimator.SetTrigger(animationTrigger);

        // add force.

        mc.mobRigidBody.AddForce(wanderDirection, ForceMode.VelocityChange);
        mc.mobRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        // set the sound, if available.

        if(jumpSoundSourceObject != null)
        {
            jumpSoundSource = jumpSoundSourceObject.GetComponent<AudioSource>();
            jumpSoundSource.Play();
        }
        
    }

    public void FinishState(MobController mc) {}

    public void FixedUpdateState(MobController mc) {}

    public string GetStateId()
    {
        return stateId;
    }

    public void UpdateState(MobController mc)
    {
        stateTimer += Time.deltaTime;

        if(stateTimer >= stateInterval)
        {
            int nextStateIndex = Random.Range(0, nextStates.Length);
            mc.ChangeState(nextStates[nextStateIndex]);
        }

        MobStaticMethods.UpdateInternalDirection(mc, wanderDirection, 5.0F);
        MobStaticMethods.UpdateRendererDirection(mc, wanderDirection, 5.0F);
    }
}
