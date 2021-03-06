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
    public class PlayerStateSlideController : MonoBehaviour, IPlayerState
    {
        int update_count_slide = 0;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            update_count_slide = 0;

            mc.slideForce = SLIDE_FORCE_MULIPLIER;
            mc.slideDirection = mc.raycastGroundedSlopeDirection;
            mc.rigidBody.AddForce(mc.raycastGroundedSlopeDirection * mc.slideForce, ForceMode.VelocityChange);

            mc.audioSource.clip = mc.slideSound;
            mc.audioSource.Play();

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            update_count_slide++;

            // exit if entering water.
            if (mc.isPartialSubmerged)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_WATER_DEFAULT);
                return;
            }

            // exit if slide resistance recovered.
            if (mc.slideResistance >= SLIDE_RESISTANCE_MAX)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }

            // exit if fully in air.
            if (!mc.isSpherecastGrounded && !mc.isRaycastGrounded)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }

            // exit if slow enough to jump.
            if (mc.isRaisedSouth
                && mc.isSpherecastGrounded
                && mc.rigidBody.velocity.magnitude < SLIDE_SPEED_RECOVERY_MAX)
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
            UpdateStateMovement(mc);
            UpdateStateLateralMovement(mc);
            PlayerStaticMethods.LimitSpeedThreeAxis(mc, MAX_SPEED_SLIDE);
            FixedUpdateStateSlide(mc);

        }

        public void UpdateStateMovement(PlayerController mc)
        {
            // slide forward.

            if (mc.raycastGroundedSlopeAngle >= SLIDE_FORCE_ANGLE_MIN)
            {
                // update the slide vector.

                mc.slideDirection = Vector3.RotateTowards
                    (mc.slideDirection, mc.raycastGroundedSlopeDirection.normalized, SLIDE_DIRECTION_ROTATION_MULTIPLIER, 0.0f);

                // add the regular slide force, if no obstacle.

                mc.isSlideHit = Physics.SphereCast
                    (mc.transform.position, GROUNDED_SPHERECAST_RADIUS, mc.slideDirection, out mc.movementHit, MOVEMENT_SPHERECAST_DISTANCE);


                if (!mc.isSlideHit)
                {
                    mc.rigidBody.AddForce(mc.slideDirection * SLIDE_FORCE_MULIPLIER, ForceMode.VelocityChange);
                }
            }
        }

        public void UpdateStateLateralMovement(PlayerController mc)
        {
            // add sideways slide force.

            if (mc.raycastGroundedSlopeAngle >= SLIDE_FORCE_ANGLE_MIN)
            {
                // input movement relative to camera.

                var camera_relative_movement = Quaternion.Euler(0, mc.cameraObject.transform.eulerAngles.y, 0) * mc.inputDirectional;

                // camera movement relative to slope.

                var slope_relative_movement = Vector3.ProjectOnPlane(camera_relative_movement, mc.raycastGroundedSlopeNormal);

                // project movement relative to plane of slide direction.

                slope_relative_movement = Vector3.ProjectOnPlane(slope_relative_movement, mc.slideDirection);

                // force.

                var force = slope_relative_movement * ACCELERATION_AIR;

                // do raycasts.

                mc.isMovementHit = Physics.SphereCast
                    (mc.transform.position, GROUNDED_SPHERECAST_RADIUS, slope_relative_movement, out mc.movementHit, MOVEMENT_SPHERECAST_DISTANCE);

                Debug.DrawRay(mc.transform.position, slope_relative_movement, Color.red);

                // apply forces based on raycast hits.

                if (!mc.isMovementHit)
                {
                    mc.rigidBody.AddForce(force, ForceMode.VelocityChange);
                }
            }
        }

        public void FixedUpdateStateSlide(PlayerController mc)
        {
            // recover, or continue sliding
            // based on current situation.

            if (mc.raycastGroundedSlopeAngle < SLIDE_ANGLE_RECOVERY_MAX
                && !mc.groundData.isGroundSlide
                && mc.isSpherecastGrounded)
            {
                // increase the slide resistance
                mc.slideResistance += SLIDE_RESISTANCE_RECOVERY;
            }
            else
            {
                mc.slideResistance = 0.0F;
            }

            mc.slideResistance = Mathf.Clamp(mc.slideResistance, 0.0f, SLIDE_RESISTANCE_MAX);
        }

        public void UpdateState(PlayerController mc)
        {
            mc.facingDirection = new Vector3(mc.slideDirection.x, 0, mc.slideDirection.z);

            mc.facingDirectionDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, mc.facingDirection, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            mc.rendererObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_SLIDE;
        }
    }
}
