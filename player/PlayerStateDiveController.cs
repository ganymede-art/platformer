using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;

using static Assets.script.PlayerConstants;

namespace Assets.script
{
    public class PlayerStateDiveController : MonoBehaviour, IPlayerState
    {
        int update_count_dive = 0;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            update_count_dive = 0;

            // play dive animation.

            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("dive");

            // play dive sound.

            mc.audioSource.clip = mc.diveSound;
            mc.audioSource.Play();

            if (mc.inputDirectional.magnitude >= DIVE_MIN_INPUT_DIRECTIONAL_MAGNITUDE)
                mc.diveDirection = Quaternion.Euler(0, mc.cameraObject.transform.eulerAngles.y, 0) * mc.inputDirectional.normalized;
            else
                mc.diveDirection = mc.rendererObject.transform.forward.normalized;

            // zero out vertical velocity and add diving force.

            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);

            mc.rigidBody.AddForce(Vector3.up * JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);
            mc.rigidBody.AddForce(mc.diveDirection * (JUMP_FORCE_MULTIPLIER * 2), ForceMode.VelocityChange);

            // enable the attack forward trigger.

            mc.attackForward1Collider.enabled = true;

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            update_count_dive++;

            if (update_count_dive >= UPDATE_COUNT_DIVE_RECOVERY_MIN)
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
            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_DIVE);
        }

        public void UpdateState(PlayerController mc)
        {
            mc.facingDirection.x = mc.diveDirection.x;
            mc.facingDirection.y = 0;
            mc.facingDirection.z = mc.diveDirection.z;

            mc.facingDirectionDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, mc.facingDirection, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            mc.rendererObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_DIVE;
        }
    }
}
