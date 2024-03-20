using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffectMobStateAction : MonoBehaviour, IStateAction<Mob, MobStateId>
{
    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // Public fields.
    public MobStateIdConstant stateId;
    [Space]
    public ParticleSystem fx;

    public void BeginStateAction(Mob controller, Dictionary<string, object> args = null)
    {
        if (fx != null)
            fx.Play();
    }

    public void EndStateAction(Mob controller)
    {
        if (fx != null)
            fx.Stop();
    }

    public void FixedUpdateStateAction(Mob controller) { }
    public void UpdateStateAction(Mob controller) { }
}
