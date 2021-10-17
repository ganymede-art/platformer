using UnityEngine;
using Assets.script;
using Assets.script.enemy;
using static Assets.script.enemy.EnemyStaticMethods;

public class EnemyStateWanderController : MonoBehaviour, IEnemyStateController
{
    const float ANIM_TURNING_SPEED = 5.0f;

    private float stateLifetimeInterval = 0.0f;
    private float stateLifetimeTimer = 0.0f;

    private Vector3 movementDirection = Vector3.zero;

    private Transform ecRenderTransform = null;

    public float movementForceMulitplier = 1.0f;
    public float movementMaxSpeed = 0.25f;

    public float stateTimeMin = 1.0f;
    public float stateTimeMax = 3.0f;

    public EnemyStateType[] nextStates;

    public void BeginState(IEnemyCoreController ec)
    {
        stateLifetimeTimer = 0.0f;
        stateLifetimeInterval = UnityEngine.Random.Range(stateTimeMin, stateTimeMax);

        float random_x = UnityEngine.Random.Range(-1.0f, 1.0f);
        float random_z = UnityEngine.Random.Range(-1.0f, 1.0f);

        movementDirection = new Vector3(random_x, 0, random_z).normalized;

        ec.GetData().enemyAnimator.ResetAllAnimatorTriggers();
        ec.GetData().enemyAnimator.SetTrigger("actor_move");
    }

    public void CheckState(IEnemyCoreController ec)
    {
        
    }

    public void FinishState(IEnemyCoreController ec)
    {
        
    }

    public EnemyStateType GetStateType()
    {
        return EnemyStateType.wander;
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
