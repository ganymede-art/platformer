using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Assets.script.PlayerConstants;

namespace Assets.script
{
    public class PlayerStateAttackController : MonoBehaviour, IPlayerState
    {
        public void BeginState(PlayerController mc, params object[] parameters)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("attack");

            // play attack sound.

            mc.audioSource.clip = mc.attackSound;
            mc.audioSource.Play();

            // zero out vertical velocity and add diving force.

            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
            mc.rigidBody.velocity = new Vector3
                (0, 0, 0);

            mc.rigidBody.AddForce(Vector3.up * JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);
            mc.rigidBody.AddForce(mc.directionObject.transform.forward * (JUMP_FORCE_MULTIPLIER), ForceMode.VelocityChange);

            // enable the attack forward trigger.

            mc.attackForward1Collider.enabled = true;

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_GROUNDED, 1, PhysicMaterialCombine.Average);

        }

        public void CheckState(PlayerController mc)
        {
            if (mc.stateFixedUpdateCount >= 15)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            // disable the attack forward trigger.

            mc.attackForward1Collider.enabled = false;
        }

        public void FixedUpdateState(PlayerController mc)
        {
            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_GROUNDED);
        }

        public void UpdateState(PlayerController mc)
        {
            
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_ATTACK;
        }
    }
}
