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
    public class PlayerStateDefaultController : MonoBehaviour, IPlayerState
    {
        int update_count_default = 0;

        // variables.

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            update_count_default = 0;
        }

        public void CheckState(PlayerController mc)
        {
            update_count_default++;

            // enter jumping state if right criteria are met.

            if (mc.isRaisedSouth && mc.isSpherecastGrounded)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_JUMP);
                return;
            }

            if (mc.isPartialSubmerged)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_WATER_DEFAULT);
                return;
            }

            // exit to attack state if attack is pressed
            // and grounded.

            if(mc.isRaisedWest
                && mc.isSpherecastGrounded
                && mc.master.playerController.canAttack)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_ATTACK);
                return;
            }

            // exit to crouch state if positive 3 is pressed
            // and grounded.

            if(mc.master.inputController.isInputEastExtra
                && mc.isSpherecastGrounded)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_CROUCH);
                return;
            }

            // exit to diving state, if previous state was jump,
            // if not grounded since entering this state, 
            // attack is pressed, and in air.

            if (mc.isRaisedWest
                && mc.previousStateType == GameConstants.PLAYER_STATE_JUMP
                && !mc.isSpherecastGroundedSinceStateBegin
                && !mc.isSpherecastGrounded
                && mc.master.playerController.canDive)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DIVE);
                return;
            }

            // exit to diving state, if previous state was jump,
            // if not grounded since entering this state, 
            // jump is pressed, and in air.

            if (mc.isRaisedSouth
                && mc.previousStateType == GameConstants.PLAYER_STATE_JUMP
                && !mc.isSpherecastGroundedSinceStateBegin
                && !mc.isSpherecastGrounded
                && mc.master.playerController.canDoubleJump)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DOUBLE_JUMP);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            return;
        }

        public void FixedUpdateState(PlayerController mc)
        {
            UpdateStateMovement(mc);
            PlayerStaticMethods.FixedUpdateSlide(mc);
            PlayerStaticMethods.ApplyDynamicFriction(mc);
            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_GROUNDED);
        }

        public void UpdateStateMovement(PlayerController mc)
        {
            // input movement relative to camera.
            var camera_relative_movement = Quaternion.Euler(0, mc.cameraObject.transform.eulerAngles.y, 0) * mc.inputDirectional;

            // camera movement relative to slope.
            var slope_relative_movement = Vector3.ProjectOnPlane(camera_relative_movement, mc.spherecastGroundedSlopeNormal);

            // acceleration
            float acceleration = mc.isSpherecastGrounded ? PlayerConstants.ACCELERATION_GROUNDED : PlayerConstants.ACCELERATION_AIR;

            // force
            var force = slope_relative_movement * acceleration;

            PlayerStaticMethods.StepMovement(mc, slope_relative_movement, force);
        }

        public void UpdateState(PlayerController mc)
        {
            // update the animator.

            if (mc.isNearSpherecaseGrounded)
            {
                if(mc.isInputDirectional)
                {
                    mc.playerAnimator.ResetAllAnimatorTriggers();
                    mc.playerAnimator.SetTrigger("move");
                    mc.playerAnimator.SetFloat("speed_multiplier", mc.rigidBody.velocity.magnitude);
                }
                else
                {
                    mc.playerAnimator.ResetAllAnimatorTriggers();
                    mc.playerAnimator.SetTrigger("idle");
                }
            }
            else
            {
                mc.playerAnimator.ResetAllAnimatorTriggers();
                mc.playerAnimator.SetTrigger("jump_down");
            }

            PlayerStaticMethods.UpdateInternalDirection(mc);
            PlayerStaticMethods.UpdateRendererDirection(mc);
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_DEFAULT;
        }
    }
}
