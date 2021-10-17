using Assets.script.enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using static Assets.script.enemy.EnemyStaticMethods;

public class EnemyStateIdleController : MonoBehaviour, IEnemyStateController
{
    private float stateLifetimeInterval = 0.0f;
    private float stateLifetimeTimer = 0.0f;

    public float stateTimeMin = 1.0f;
    public float stateTimeMax = 3.0f;

    public EnemyStateType[] nextStates;

    public void BeginState(IEnemyCoreController ec)
    {
        stateLifetimeTimer = 0.0f;
        stateLifetimeInterval = UnityEngine.Random.Range(stateTimeMin, stateTimeMax);

        ec.GetData().enemyAnimator.ResetAllAnimatorTriggers();
        ec.GetData().enemyAnimator.SetTrigger("actor_idle");
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
        return EnemyStateType.idle;
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
        
    }
}
