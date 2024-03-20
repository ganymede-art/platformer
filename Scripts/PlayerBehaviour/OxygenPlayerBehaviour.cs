using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenPlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    // Consts.
    private const float OXYGEN_DRAIN_INTERVAL = 1.0F;
    private const int OXYGEN_EMPTY_DAMAGE_AMOUNT = 1;

    // Private fields.
    private float oxygenDrainTimer;
    private bool isOxygenDrain;
    private bool wasOxygenDrain;

    // Public properties.
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.Oxygen;

    public void BeginBehaviour(Player c, Dictionary<string, object> args = null)
    {
    }

    public void FixedUpdateBehaviour(Player c)
    {
        wasOxygenDrain = isOxygenDrain;
        isOxygenDrain = (c.Water.IsFullSubmerged);

        if (!wasOxygenDrain && isOxygenDrain)
            BeginOxygenDrain(c);
        else if (wasOxygenDrain && !isOxygenDrain)
            EndOxygenDrain(c);
    }

    public void UpdateBehaviour(Player c)
    {
        if (!isOxygenDrain)
            return;

        if(oxygenDrainTimer >= OXYGEN_DRAIN_INTERVAL)
        {
            PlayerHighLogic.G.ModifyOxygen(-1);
            if(PlayerHighLogic.G.Oxygen <= 0)
            {
                c.Damage.OnSimpleDamage(OXYGEN_EMPTY_DAMAGE_AMOUNT);
            }
            oxygenDrainTimer = 0.0F;
        }

        oxygenDrainTimer += Time.deltaTime;
    }

    public void EndBehaviours(Player c) { }

    private void BeginOxygenDrain(Player c) { }

    private void EndOxygenDrain(Player c)
    {
        PlayerHighLogic.G.ModifyOxygen(PlayerHighLogic.G.MaxOxygen);
        oxygenDrainTimer = 0.0F;
    }
}
