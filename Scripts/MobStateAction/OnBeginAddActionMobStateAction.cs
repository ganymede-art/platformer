using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBeginAddActionMobStateAction : MonoBehaviour, IStateAction<Mob, MobStateId>
{
    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // Public fields.
    public MobStateIdConstant stateId;
    [Space]
    public AddActionHighLogicTrigger onBeginAddActionHighLogicTrigger;

    public void BeginStateAction(Mob controller, Dictionary<string, object> args = null)
    {
        if (onBeginAddActionHighLogicTrigger != null)
            onBeginAddActionHighLogicTrigger.AddAction();
    }

    public void EndStateAction(Mob controller) { }
    public void FixedUpdateStateAction(Mob controller) { }
    public void UpdateStateAction(Mob controller) { }
}
