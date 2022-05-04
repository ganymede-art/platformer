using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Script;

using static Assets.Script.PlayerConstants;

namespace Assets.Script
{
    public class PlayerStateWaterJump : MonoBehaviour, IPlayerState
    {
        int update_count_water_jump = 0;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger(TRIGGER_WATER_JUMP_UP);

            update_count_water_jump = 0;

            // enter jump state.
            // reset jump power.

            mc.jumpPersistEnergy = JUMP_PERSIST_ENERGY_MAX;

            // add jumping force.

            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);

            mc.rigidBody.AddForce(Vector3.up * WATER_JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);

            // player sound.

            mc.audioSource.clip = mc.waterJumpSound;
            mc.audioSource.Play();

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            update_count_water_jump++;

            // exit to water dive if pressing interact.

            if (mc.isRaisedWest && mc.master.playerController.canSwim)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_SWIM);
                return;
            }

            if (mc.rigidBody.velocity.y <= 0)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_WATER_DEFAULT);
                return;
            }

            if (!mc.behaviourWater.isPartialSubmerged)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_JUMP);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            
        }

        public void FixedUpdateState(PlayerController mc)
        {
            UpdateStateJump(mc);
            UpdateStateMovement(mc);
            FixedUpdateStateSpeed(mc);
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

        public void UpdateState(PlayerController mc)
        {
            
        }

        public void FixedUpdateStateSpeed(PlayerController mc)
        {
            Vector3 old_x_z = new Vector3(mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
            Vector3 old_y = new Vector3(0, mc.rigidBody.velocity.y, 0);

            if (old_x_z.magnitude > MAX_SPEED_WATER)
            {
                old_x_z = Vector3.ClampMagnitude(old_x_z, MAX_SPEED_WATER);
            }

            if (old_y.y < -MAX_SPEED_WATER)
            {
                old_y = Vector3.ClampMagnitude(old_y, MAX_SPEED_WATER);
            }

            mc.rigidBody.velocity = old_x_z + old_y;
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_WATER_JUMP;
        }
    }


}
