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
    public class PlayerStateDiveController : MonoBehaviour, IPlayerStateController
    {
        int update_count_dive = 0;

        public void BeginState(PlayerController mc)
        {
            update_count_dive = 0;

            mc.audioSource.clip = mc.soundDive;
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
        }

        public void CheckState(PlayerController mc)
        {
            update_count_dive++;

            if (update_count_dive >= UPDATE_COUNT_DIVE_RECOVERY_MIN)
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            // disable the attack forward trigger.

            mc.attackForward1Collider.enabled = false;
        }

        public void UpdateState(PlayerController mc)
        {
        }

        public void UpdateStateAnimator(PlayerController mc)
        {
            mc.facingDirection.x = mc.diveDirection.x;
            mc.facingDirection.y = 0;
            mc.facingDirection.z = mc.diveDirection.z;

            mc.facingDirectionDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, mc.facingDirection, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            mc.rendererObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
        }

        public void UpdateStateSlide(PlayerController mc)
        {
            return;
        }

        public void UpdateStateSpeed(PlayerController mc)
        {
            Vector3 old_x_z = new Vector3(mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
            Vector3 old_y = new Vector3(0, mc.rigidBody.velocity.y, 0);

            if (old_x_z.magnitude > MAX_SPEED_DIVE)
            {

                old_x_z = Vector3.ClampMagnitude(old_x_z, MAX_SPEED_DIVE);
                mc.rigidBody.velocity = old_x_z + old_y;
            }
        }

        public void UpdateStateDragAndFriction(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerJump].UpdateStateDragAndFriction(mc);
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.playerDive;
        }
    }
}
