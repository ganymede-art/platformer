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
    public class PlayerStateJumpController : MonoBehaviour, IPlayerStateController
    {
        // variables.

        int update_count_jump = 0;

        RaycastHit movement_hit;
        RaycastHit step_movement_hit;

        public void BeginState(PlayerController mc)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("jump_up");

            // reset the update count.

            update_count_jump = 0;

            // if coming from the water jump state,
            // don't add any additional force.

            if (mc.previousStateType == PlayerStateType.playerWaterJump)
                return;

            // enter jump state.
            // reset jump power.

            mc.jumpPersistEnergy = PlayerConstants.JUMP_PERSIST_ENERGY_MAX;

            // add jumping force.

            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);

            mc.rigidBody.AddForce(Vector3.up * PlayerConstants.JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);

            // player sound.

            mc.audioSource.clip = mc.soundJump;
            mc.audioSource.Play();
        }

        public void CheckState(PlayerController mc)
        {
            // increment the update count.

            update_count_jump++;

            // exit to default state if right criteria are met.

            if (mc.rigidBody.velocity.y <= 0)
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }

            // exit to default state if grounded.

            if (update_count_jump >= PlayerConstants.UPDATE_COUNT_JUMP_RECOVERY_MIN
                && (mc.isRaycastGrounded || mc.isSpherecastGrounded))
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }

            // exit to dive state.

            if (mc.isRaisedInteract 
                && mc.master.playerController.canDive)
            {
                mc.ChangePlayerState(PlayerStateType.playerDive);
                return;
            }

            // exit to double jump state.

            if(mc.isRaisedPositive
                && mc.master.playerController.canDoubleJump)
            {
                mc.ChangePlayerState(PlayerStateType.PlayerDoubleJump);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            return;
        }

        public void UpdateState(PlayerController mc)
        {
            UpdateStateJump(mc);
            UpdateStateMovement(mc);
        }

        public void UpdateStateAnimator(PlayerController mc)
        {
            if (mc.stateUpdateCount ==  0)
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

            if (mc.master.inputController.isInputPositive && mc.jumpPersistEnergy > 0)
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

            PlayerStaticMethods.StepMovement(mc, camera_relative_movement, force);
        }

        public void UpdateStateSlide(PlayerController mc)
        {
            return;
        }

        public void UpdateStateSpeed(PlayerController mc)
        {
            Vector3 old_x_z = new Vector3(mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
            Vector3 old_y = new Vector3(0, mc.rigidBody.velocity.y, 0);

            if (old_x_z.magnitude > PlayerConstants.MAX_SPEED_GROUNDED)
            {

                old_x_z = Vector3.ClampMagnitude(old_x_z, PlayerConstants.MAX_SPEED_GROUNDED);
                mc.rigidBody.velocity = old_x_z + old_y;
            }
        }

        public void UpdateStateDragAndFriction(PlayerController mc)
        {
            mc.rigidBody.drag = DRAG_AIR;
            mc.rbCollider.material.dynamicFriction = 0f;
            mc.rbCollider.material.staticFriction = 0f;

            mc.rbCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.playerJump;
        }
    }
}
