using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePlayerCheckMobBehaviour : MonoBehaviour, IBehaviour<Mob,MobBehaviourId>
{
    // Private fields.
    private float inDistanceTimer = 0.0F;

    // Public properties.
    public MobBehaviourId BehaviourId => MobBehaviourId.DistancePlayerCheck;

    // Public fields.
    public MobStateIdConstant[] activeStateIds;
    public float maxDistance;
    public float minInterval;
    public MobStateIdConstant[] nextStateIds;

    public void BeginBehaviour(Mob c, Dictionary<string, object> args = null) { }
    public void EndBehaviours(Mob c) { }

    public void FixedUpdateBehaviour(Mob c)
    {
        if (!MobStatics.IsMobInState(c, activeStateIds))
            return;

        float distance = Vector3.Distance
            ( c.transform.position
            , ActiveSceneHighLogic.G.CachedPlayerObject.transform.position);

        if (distance <= maxDistance)
        {
            inDistanceTimer += Time.deltaTime;
            if (inDistanceTimer >= minInterval)
            {
                inDistanceTimer = 0.0F;
                c.ChangeToRandomState(nextStateIds);
                return;
            }
        }
        else
        {
            inDistanceTimer = 0.0F;
        }
    }

    public void UpdateBehaviour(Mob c) { }
}
