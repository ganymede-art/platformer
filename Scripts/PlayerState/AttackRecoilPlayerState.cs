using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class AttackRecoilPlayerState : MonoBehaviour, IState<Player,PlayerStateId>
{
    public PlayerStateId StateId => PlayerStateId.AttackRecoil;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.playerRigidBody.velocity = Vector3.zero;
        c.playerRigidBody.AddForce(Vector3.up * ATTACK_RECOIL_UP_FORCE_MULT, ForceMode.VelocityChange);
        c.playerRigidBody.AddForce(-c.playerDirectionObject.transform.forward * ATTACK_RECOIL_REAR_FORCE_MULT, ForceMode.VelocityChange);

        c.attackHitbox.gameObject.SetActive(true);
    }

    public void FixedUpdateState(Player c)
    {
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, DEFAULT_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        if (c.StateTimer >= ATTACK_RECOIL_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }
    }

    public void EndState(Player c)
    {
        c.attackHitbox.gameObject.SetActive(false);
    }
}
