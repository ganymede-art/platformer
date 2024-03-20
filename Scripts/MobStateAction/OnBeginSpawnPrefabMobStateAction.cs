using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBeginSpawnPrefabMobStateAction : MonoBehaviour, IStateAction<Mob, MobStateId>
{
    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // Public fields.
    public MobStateIdConstant stateId;
    [Space]
    public GameObject spawnPrefab;
    public Vector3 spawnOffset;

    public void BeginStateAction(Mob c, Dictionary<string, object> args = null)
    {
        if (spawnPrefab != null)
            Instantiate
                ( spawnPrefab
                , c.mobDirectionObject.transform.TransformPoint(spawnOffset)
                , c.mobDirectionObject.transform.rotation);
    }

    public void UpdateStateAction(Mob c) { }
    public void FixedUpdateStateAction(Mob c) { }
    public void EndStateAction(Mob c) { }
}
