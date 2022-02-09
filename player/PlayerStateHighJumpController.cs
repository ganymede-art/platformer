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
    public class PlayerStateHighJumpController : MonoBehaviour, IPlayerState
    {
        public void BeginState(PlayerController mc, params object[] parameters)
        {
            // set the animation.

            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("crouch_jump_up");

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

            mc.rigidBody.AddForce(Vector3.up * PlayerConstants.FORCE_MULTIPLIER_HIGH_JUMP, ForceMode.VelocityChange);

            // player sound.

            mc.audioSource.clip = mc.highJumpSound;
            mc.audioSource.Play();

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            // enter default state if right criteria are met.

            if (mc.rigidBody.velocity.y <= 0)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }

            // enter default state if grounded.

            if (mc.stateFixedUpdateCount >= PlayerConstants.UPDATE_COUNT_JUMP_RECOVERY_MIN
                && (mc.isRaycastGrounded || mc.isSpherecastGrounded))
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
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
        }

        public void UpdateState(PlayerController mc)
        {
            mc.states[GameConstants.PLAYER_STATE_JUMP].UpdateState(mc);

        }

        public void UpdateStateJump(PlayerController mc)
        {
            // decrement the remaining jump persist energy.

            mc.jumpPersistEnergy -= 1;

            if (mc.master.inputController.isInputSouth && mc.jumpPersistEnergy > 0)
            {
                // if the jump input is given, and persist energy > 0, add extra jump force.

                mc.rigidBody.AddForce(Vector3.up * PlayerConstants.FORCE_MULTIPLIER_HIGH_JUMP_PERSIST, ForceMode.VelocityChange);
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

            var force = camera_relative_movement * PlayerConstants.ACCELERATION_CROUCH_JUMP;

            PlayerStaticMethods.StepMovement(mc, camera_relative_movement, force);
        }

        public void FixedUpdateStateSlide(PlayerController mc)
        {
            return;
        }

        public void FixedUpdateStateSpeed(PlayerController mc)
        {
            Vector3 old_x_z = new Vector3(mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
            Vector3 old_y = new Vector3(0, mc.rigidBody.velocity.y, 0);

            if (old_x_z.magnitude > PlayerConstants.MAX_SPEED_GROUNDED)
            {

                old_x_z = Vector3.ClampMagnitude(old_x_z, PlayerConstants.MAX_SPEED_GROUNDED);
                mc.rigidBody.velocity = old_x_z + old_y;
            }
        }

        public void FixedUpdateStateDragAndFriction(PlayerController mc)
        {
            mc.rigidBody.drag = DRAG_AIR;
            mc.rbCollider.material.dynamicFriction = 0F;
            mc.rbCollider.material.staticFriction = 0F;
            mc.rbCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_HIGH_JUMP;
        }
    }
}
