using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MobConstants;

public class DestroyMobState : MonoBehaviour, IState<Mob, MobStateId>
{
    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // Public fields.
    [Header("State Attributes")]
    public MobStateIdConstant stateId;

    public void BeginState(Mob c, Dictionary<string, object> args = null)
    {
        GameObject.Destroy(c.gameObject);
    }

    public void FixedUpdateState(Mob c) { }
    public void UpdateState(Mob c) { }
    public void EndState(Mob c) { }
    
}
