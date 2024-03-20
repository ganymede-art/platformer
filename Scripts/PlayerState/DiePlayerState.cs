using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class DiePlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.Die;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_HURT_UP);

        c.playerRigidBody.velocity = Vector3.zero;
        c.playerRigidBody.AddForce(Vector3.up * DIE_UP_FORCE_MULT, ForceMode.VelocityChange);

        if (args != null)
        {
            var hitboxData = args[STATE_ARG_HITBOX_DATA] as HitboxData;
            var hitboxObject = args[STATE_ARG_HITBOX_OBJECT] as GameObject;

            var awayDirection = (c.transform.position - hitboxObject.transform.position);
            awayDirection.y = 0.0F;
            awayDirection.Normalize();

            PlayerStatics.UpdateInternalDirection(c, -awayDirection);
            c.playerRigidBody.AddForce(awayDirection * HURT_AWAY_FORCE_MULT, ForceMode.VelocityChange);
        }
    }

    public void FixedUpdateState(Player c)
    {
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, DYNAMIC_FRICTION);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, DIE_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        if (c.StateTimer >= DIE_MAX_INTERVAL)
        {
            GameHighLogic.ResetGame();
        }
    }

    public void EndState(Player c)
    {

    }
}
