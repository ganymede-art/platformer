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
    public class PlayerStateWaterDefaultController : MonoBehaviour, IPlayerState
    {
        int update_count_water_default = 0;

        private PlayerStateDefaultController playerDefault;

        private void Start()
        {
            playerDefault = GameMasterController.GlobalPlayerController
                .states[GameConstants.PLAYER_STATE_DEFAULT] as PlayerStateDefaultController;
        }

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            update_count_water_default = 0;
        }

        public void CheckState(PlayerController mc)
        {
            update_count_water_default++;

            // exit to water dive if pressing interact.

            if (mc.isRaisedWest
                && mc.master.playerController.canDive)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_WATER_DIVE);
                return;
            }

            // exit to water jump if pressing jump 
            // and grounded, or descending in water.

            if
            (
                mc.isRaisedSouth
                && (mc.isSpherecastGrounded || mc.rigidBody.velocity.y <= MINIMUM_WATER_JUMP_Y_SPEED)
            )
            {
                if (mc.isSpherecastGrounded || mc.master.playerController.canWaterJump)
                {
                    mc.ChangePlayerState(GameConstants.PLAYER_STATE_WATER_JUMP);
                    return;
                }
            }

            if (!mc.isPartialSubmerged)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            
        }

        public void FixedUpdateState(PlayerController mc)
        {
            UpdateStateMovement(mc);
            PlayerStaticMethods.ApplyDynamicFriction(mc);
            FixedUpdateStateSpeed(mc);

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

            if (mc.isSpherecastGrounded)
            {
                if (mc.isInputDirectional)
                {
                    mc.playerAnimator.ResetAllAnimatorTriggers();
                    mc.playerAnimator.SetTrigger("water_move");
                    mc.playerAnimator.SetFloat("speed_multiplier", mc.rigidBody.velocity.magnitude);
                }
                else
                {
                    mc.playerAnimator.ResetAllAnimatorTriggers();
                    mc.playerAnimator.SetTrigger("water_idle");
                }
            }
            else
            {
                if (mc.rigidBody.velocity.y < -1f)
                {
                    mc.playerAnimator.ResetAllAnimatorTriggers();
                    mc.playerAnimator.SetTrigger("water_jump_down");
                }
            }

            // update player facing direction.

            mc.facingDirection = Quaternion.Euler(0, mc.cameraObject.transform.rotation.eulerAngles.y, 0) * mc.inputDirectional;

            mc.facingDirectionDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, mc.facingDirection, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            mc.rendererObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
            mc.directionObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
        }

        public void FixedUpdateStateSpeed(PlayerController mc)
        {
            Vector3 old_x_z = new Vector3(mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
            Vector3 old_y = new Vector3(0, mc.rigidBody.velocity.y, 0);

            if (old_x_z.magnitude > MAX_SPEED_WATER)
            {
                old_x_z = Vector3.ClampMagnitude(old_x_z, MAX_SPEED_WATER);
            }

            if (old_y.y < -MAX_SPEED_WATER_SINK)
            {
                old_y.y = -MAX_SPEED_WATER_SINK;
            }

            mc.rigidBody.velocity = old_x_z + old_y;
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_WATER_DEFAULT;
        }
    }
}
