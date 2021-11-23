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
    public class PlayerStateDefaultController : MonoBehaviour, IPlayerStateController
    {
        int update_count_default = 0;

        // variables.

        public void BeginState(PlayerController mc)
        {
            update_count_default = 0;
        }

        public void CheckState(PlayerController mc)
        {
            update_count_default++;

            // enter jumping state if right criteria are met.

            if (mc.isRaisedSouth && mc.isSpherecastGrounded)
            {
                mc.ChangePlayerState(PlayerStateType.playerJump);
                return;
            }

            if (mc.isPartialSubmerged)
            {
                mc.ChangePlayerState(PlayerStateType.playerWaterDefault);
                return;
            }

            // exit to attack state if attack is pressed
            // and grounded.

            if(mc.isRaisedWest
                && mc.isSpherecastGrounded
                && mc.master.playerController.canAttack)
            {
                mc.ChangePlayerState(PlayerStateType.playerAttack);
                return;
            }

            // exit to crouch state if positive 3 is pressed
            // and grounded.

            if(mc.master.inputController.isInputEastExtra
                && mc.isSpherecastGrounded)
            {
                mc.ChangePlayerState(PlayerStateType.playerCrouch);
                return;
            }

            // exit to diving state, if previous state was jump,
            // if not grounded since entering this state, 
            // attack is pressed, and in air.

            if (mc.isRaisedWest
                && mc.previousStateType == PlayerStateType.playerJump
                && !mc.isSpherecastGroundedSinceStateBegin
                && !mc.isSpherecastGrounded
                && mc.master.playerController.canDive)
            {
                mc.ChangePlayerState(PlayerStateType.playerDive);
                return;
            }

            // exit to diving state, if previous state was jump,
            // if not grounded since entering this state, 
            // jump is pressed, and in air.

            if (mc.isRaisedSouth
                && mc.previousStateType == PlayerStateType.playerJump
                && !mc.isSpherecastGroundedSinceStateBegin
                && !mc.isSpherecastGrounded
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
            UpdateStateMovement(mc);
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

        public void UpdateStateSlide(PlayerController mc)
        {
            // move towards the sliding state
            // if the right criteria are met.

            if (mc.raycastGroundedSlopeAngle > SLIDE_ANGLE_MIN
                || mc.groundData.isGroundSlide)
            {
                // reduce the slide resistance
                mc.slideResistance -= (mc.raycastGroundedSlopeAngle * SLIDE_RESISTANCE_GROUND_ANGLE_MULTIPLIER);
            }
            else
            {
                mc.slideResistance = SLIDE_RESISTANCE_MAX;
            }

            mc.slideResistance = Mathf.Clamp(mc.slideResistance, 0.0f, SLIDE_RESISTANCE_MAX);

            if (mc.slideResistance <= 0.0f && mc.isSpherecastGrounded)
            {
                mc.ChangePlayerState(PlayerStateType.playerSlide);
                return;
            }
        }

        public void UpdateStateSpeed(PlayerController mc)
        {
            // limit speed while in the default state.
            // maximum speed depends on criteria.

            if (mc.isPartialSubmerged)
            {
                if (mc.rigidBody.velocity.magnitude > PlayerConstants.MAX_SPEED_WATER)
                {
                    mc.rigidBody.velocity = Vector3.ClampMagnitude(mc.rigidBody.velocity, PlayerConstants.MAX_SPEED_WATER);
                }
            }
            else
            {
                if (mc.isSpherecastGrounded)
                {
                    if (mc.rigidBody.velocity.magnitude > PlayerConstants.MAX_SPEED_GROUNDED)
                    {
                        mc.rigidBody.velocity = Vector3.ClampMagnitude(mc.rigidBody.velocity, PlayerConstants.MAX_SPEED_GROUNDED);
                    }
                }
                else
                {
                    Vector3 old_x_z = new Vector3(mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
                    Vector3 old_y = new Vector3(0, mc.rigidBody.velocity.y, 0);

                    if (old_x_z.magnitude > PlayerConstants.MAX_SPEED_GROUNDED)
                    {

                        old_x_z = Vector3.ClampMagnitude(old_x_z, PlayerConstants.MAX_SPEED_GROUNDED);
                        mc.rigidBody.velocity = old_x_z + old_y;
                    }
                }
            }
        }

        public void UpdateStateAnimator(PlayerController mc)
        {
            // update the animator.

            if (mc.isSpherecastGrounded)
            {
                if(mc.isInputDirectional)
                {
                    mc.playerAnimator.ResetAllAnimatorTriggers();
                    mc.playerAnimator.SetTrigger("move");
                }
                else
                {
                    mc.playerAnimator.ResetAllAnimatorTriggers();
                    mc.playerAnimator.SetTrigger("idle");
                }
            }
            else
            {
                if (mc.rigidBody.velocity.y < -3)
                {
                    mc.playerAnimator.ResetAllAnimatorTriggers();
                    mc.playerAnimator.SetTrigger("jump_down");
                }
            }

            // update player facing direction.

            mc.facingDirection = Quaternion.Euler(0, mc.cameraObject.transform.rotation.eulerAngles.y, 0) * mc.inputDirectional;
            mc.facingDirectionDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, mc.facingDirection, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            mc.rendererObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
            mc.directionObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
        }

        public void UpdateStateDragAndFriction(PlayerController mc)
        {
            // update based on circumstances.

            mc.rigidBody.drag = mc.isRaycastGrounded ? DRAG_GROUNDED : DRAG_AIR;

            if (mc.isInputDirectional || !mc.isRaycastGrounded)
            {
                mc.rbCollider.material.dynamicFriction = 0f;
                mc.rbCollider.material.staticFriction = 0f;
            }
            else
            {
                if (mc.collidingMovingObjects.Count == 0)
                {
                    mc.rbCollider.material.dynamicFriction = 100;
                    mc.rbCollider.material.staticFriction = 100;
                }
                else
                {
                    mc.rbCollider.material.dynamicFriction = 1;
                    mc.rbCollider.material.staticFriction = 1;
                }
            }

            // change physics combine mode.

            mc.rbCollider.material.frictionCombine = PhysicMaterialCombine.Average;
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.playerDefault;
        }
    }
}
