using Assets.script.enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using static Assets.script.enemy.EnemyStaticMethods;

public class EnemyStateAlertController : MonoBehaviour, IEnemyStateController
{
    const float ANIM_TURNING_SPEED = 9.0f;

    private float stateLifetimeInterval = 0.0f;
    private float stateLifetimeTimer = 0.0f;

    private Vector3 alertDirection = Vector3.zero;
    private Transform ecRenderTransform = null;

    public AudioClip sfxAlert;

    public EnemyStateType[] nextStates;

    public void BeginState(IEnemyCoreController ec)
    {
        stateLifetimeTimer = 0.0f;
        stateLifetimeInterval = 1.0f;

        ec.GetData().enemyAnimator.ResetAllAnimatorTriggers();
        ec.GetData().enemyAnimator.SetTrigger("actor_alert");

        // play alert sound.

        ec.GetData().audioSource.clip = sfxAlert;
        ec.GetData().audioSource.Play();

        // get the direction towards the player
        // to face when alerted.

        alertDirection = ec.GetData().playerObject.transform.position - ec.GetData().rigidBody.position;
        alertDirection.y = 0;
    }

    public void CheckState(IEnemyCoreController ec)
    {
        
    }

    public void FinishState(IEnemyCoreController ec)
    {
        
    }

    public void FixedUpdateState(IEnemyCoreController ec)
    {
        // get the direction towards the player
        // to face when alerted.

        alertDirection = ec.GetData().playerObject.transform.position - ec.GetData().rigidBody.position;
        alertDirection.y = 0;
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.alert;
    }

    public void UpdateState(IEnemyCoreController ec)
    {
        stateLifetimeTimer += Time.deltaTime;

        if (stateLifetimeTimer >= stateLifetimeInterval)
        {
            ec.ChangeState(GetRandomNextStateType(nextStates));
        }
    }

    public void UpdateStateAnimator(IEnemyCoreController ec)
    {
        ec.GetData().directionTransform.rotation = Quaternion.Euler(alertDirection);

        ecRenderTransform = ec.GetData().enemyRendererObject.transform;

        ecRenderTransform.rotation = Quaternion.LookRotation
            (Vector3.RotateTowards(ecRenderTransform.forward, alertDirection, ANIM_TURNING_SPEED * Time.deltaTime, 0.0f));
    }
}
