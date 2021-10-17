using Assets.script;
using Assets.script.enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.script.enemy.EnemyStaticMethods;

public class EnemyStateHopController : MonoBehaviour, IEnemyStateController
{
    const float ANIM_TURNING_SPEED = 5.0f;

    private float stateLifetimeInterval = 0.0f;
    private float stateLifeTimeTimer = 0.0f;

    private Vector3 movementDirection = Vector3.zero;

    private Transform ecRenderTransform = null;

    public float stateTimeMin = 1.0f;
    public float stateTimeMax = 3.0f;
    
    public EnemyStateType[] nextStates;

    public void BeginState(IEnemyCoreController ec)
    {
        stateLifeTimeTimer = 0.0f;
        stateLifetimeInterval = UnityEngine.Random.Range(stateTimeMin, stateTimeMax);

        movementDirection = ec.GetData().enemyRendererObject.transform.forward;

        ec.GetData().rigidBody.AddForce(Vector3.up * 3, ForceMode.VelocityChange);
        ec.GetData().rigidBody.AddForce(movementDirection, ForceMode.VelocityChange);

        ec.GetData().enemyAnimator.ResetAllAnimatorTriggers();
        ec.GetData().enemyAnimator.SetTrigger("actor_jump_up");
    }

    public void CheckState(IEnemyCoreController ec)
    {

    }

    public void FinishState(IEnemyCoreController ec)
    {

    }

    public void FixedUpdateState(IEnemyCoreController ec)
    {

    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.hop;
    }

    public void UpdateState(IEnemyCoreController ec)
    {
        stateLifeTimeTimer += Time.deltaTime;

        if (stateLifeTimeTimer >= stateLifetimeInterval)
        {
            ec.ChangeState(GetRandomNextStateType(nextStates));
        }
    }

    public void UpdateStateAnimator(IEnemyCoreController ec)
    {
        ec.GetData().directionTransform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        ecRenderTransform = ec.GetData().enemyRendererObject.transform;
        ecRenderTransform.rotation = Quaternion.LookRotation
            (Vector3.RotateTowards(ecRenderTransform.forward, movementDirection, ANIM_TURNING_SPEED * Time.deltaTime, 0.0f));
    }
}
