using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Script;
using static Assets.Script.PlayerConstants;
using static Assets.Script.GameConstants;

namespace Assets.Script
{
    public class PlayerStateJump : MonoBehaviour, IPlayerState
    {
        // variables.

        int update_count_jump = 0;

        RaycastHit movement_hit;
        RaycastHit step_movement_hit;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger(TRIGGER_JUMP_UP);

            // reset the update count.

            update_count_jump = 0;

            // if coming from the water jump state,
            // don't add any additional force.

            if (mc.previousStateType == GameConstants.PLAYER_STATE_WATER_JUMP)
                return;

            // enter jump state.
            // reset jump power.

            mc.jumpPersistEnergy = PlayerConstants.JUMP_PERSIST_ENERGY_MAX;

            // add jumping force.

            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);

            mc.rigidBody.AddForce(Vector3.up * PlayerConstants.JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);

            // player sound.

            mc.audioSource.clip = mc.jumpSound;
            mc.audioSource.Play();

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            // increment the update count.

            update_count_jump++;

            // exit to default state if right criteria are met.

            if (mc.rigidBody.velocity.y <= 0)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }

            // exit to default state if grounded.

            if (update_count_jump >= PlayerConstants.UPDATE_COUNT_JUMP_RECOVERY_MIN
                && (mc.isChecksphereCollision))
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }

            // exit to dive state.

            if (mc.isRaisedWest 
                && mc.master.playerController.canDive)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DIVE);
                return;
            }

            // exit to slam state.

            if (mc.isRaisedEast
                && mc.master.playerController.canSlam)
            {
                mc.ChangePlayerState(PLAYER_STATE_SLAM);
                return;
            }

            // exit to double jump state.

            if (mc.isRaisedSouth
                && mc.master.playerController.canFlutter)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_FLUTTER);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            return;
        }

        public void FixedUpdateState(PlayerController mc)
        {
            UpdateStateJump(mc);
            UpdateStateMovement(mc);
            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_GROUNDED);
        }

        public void UpdateState(PlayerController mc)
        {
            if (mc.stateFixedUpdateCount ==  0)
                mc.rendererObject.transform.rotation = Quaternion.LookRotation(mc.directionObject.transform.forward);

            // update player facing direction.

            mc.facingDirection = Quaternion.Euler(0, mc.cameraObject.transform.rotation.eulerAngles.y, 0) * mc.inputDirectional;

            mc.facingDirectionDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, mc.facingDirection, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            mc.directionObject.transform.rotation = Quaternion.LookRotation(mc.facingDirection);
            
        }

        public void UpdateStateJump(PlayerController mc)
        {
            // decrement the remaining jump persist energy.

            mc.jumpPersistEnergy -= 1;

            if (mc.master.inputController.isInputSouth && mc.jumpPersistEnergy > 0)
            {
                // if the jump input is given, and persist energy > 0, add extra jump force.

                mc.rigidBody.AddForce(Vector3.up * PlayerConstants.JUMP_PERSIST_FORCE_MULTIPLIER, ForceMode.VelocityChange);
            }
            else
            {
                // if the jump input is let go, zero out the jump persist energy.

                mc.jumpPersistEnergy = 0;
            }
        }

        public void UpdateStateMovement(PlayerController mc)
        {
            // input movement relative to camera.

            var camera_relative_movement = Quaternion.Euler(0, mc.cameraObject.transform.eulerAngles.y, 0) * mc.inputDirectional;

            // force

            var force = camera_relative_movement * PlayerConstants.ACCELERATION_AIR;

            PlayerStaticMethods.FullMovement(mc, camera_relative_movement, force);
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_JUMP;
        }
    }
}
