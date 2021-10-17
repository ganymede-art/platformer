using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using Assets.script.enemy;
using System;
using static Assets.script.enemy.EnemyStaticMethods;

public class EnemyStateStampedeController : MonoBehaviour, IEnemyStateController
{
    const float ANIM_TURNING_SPEED = 5.0f;
    const float ATTACK_TIME_INTERVAL = 2.0f;

    private float stateLifetimeInterval = 0.0f;
    private float stateLifetimeTimer = 0.0f;

    private Vector3 movementDirection = Vector3.zero;

    private Transform ecRenderTransform = null;

    public float movementForceMultiplier = 1.0f;
    public float movementMaxSpeed = 1.0f;

    public AudioClip sfxStampede;

    public EnemyStateType[] nextStates;

    public void BeginState(IEnemyCoreController ec)
    {
        stateLifetimeTimer = 0.0f;
        stateLifetimeInterval = ATTACK_TIME_INTERVAL;

        // play alert sound.

        ec.GetData().audioSource.clip = sfxStampede;
        ec.GetData().audioSource.Play();

        // attack in player direction.

        movementDirection = ec.GetData().playerObject.transform.position - ec.GetData().rigidBody.position;
        movementDirection.y = 0.0f;

        // play attack animation.

        ec.GetData().enemyAnimator.ResetAllAnimatorTriggers();
        ec.GetData().enemyAnimator.SetTrigger("actor_move_attack");
    }

    public void CheckState(IEnemyCoreController ec)
    {}

    public void FinishState(IEnemyCoreController ec)
    {}

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.stampede;
    }

    public void UpdateState(IEnemyCoreController ec)
    {
        stateLifetimeTimer += Time.deltaTime;

        if(stateLifetimeTimer >= stateLifetimeInterval)
        {
            ec.ChangeState(GetRandomNextStateType(nextStates));
        }
    }

    public void FixedUpdateState(IEnemyCoreController ec)
    {
        // attack in player direction.

        movementDirection = ec.GetData().playerObject.transform.position - ec.GetData().rigidBody.position;
        movementDirection.y = 0.0f;

        Vector3 horizontal_velocity = ec.GetData().rigidBody.velocity;

        horizontal_velocity.y = 0;

        if(horizontal_velocity.magnitude < movementMaxSpeed)
            ec.GetData().rigidBody.AddForce(movementDirection, ForceMode.VelocityChange);
    }

    public void UpdateStateAnimator(IEnemyCoreController ec)
    {
        ec.GetData().directionTransform.rotation = Quaternion.Euler(movementDirection);

        ecRenderTransform = ec.GetData().enemyRendererObject.transform;
        ecRenderTransform.rotation = Quaternion.LookRotation
            (Vector3.RotateTowards(ecRenderTransform.forward, movementDirection, ANIM_TURNING_SPEED * Time.deltaTime, 0.0f));
    }
}
