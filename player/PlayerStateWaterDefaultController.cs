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
    public class PlayerStateWaterDefaultController : MonoBehaviour, IPlayerStateController
    {
        int update_count_water_default = 0;

        private PlayerStateDefaultController playerDefault;

        private void Start()
        {
            playerDefault = GameMasterController.GlobalPlayerController
                .stateControllers[PlayerStateType.playerDefault] as PlayerStateDefaultController;
        }

        public void BeginState(PlayerController mc)
        {
            update_count_water_default = 0;
        }

        public void CheckState(PlayerController mc)
        {
            update_count_water_default++;

            // exit to water dive if pressing interact.

            if (mc.isRaisedInteract
                && mc.master.playerController.canDive)
            {
                mc.ChangePlayerState(PlayerStateType.playerWaterDive);
                return;
            }

            // exit to water jump if pressing jump 
            // and grounded, or descending in water.

            if
            (
                mc.isRaisedPositive
                && (mc.isSpherecastGrounded || mc.rigidBody.velocity.y <= MINIMUM_WATER_JUMP_Y_SPEED)
            )
            {
                if (mc.isSpherecastGrounded || mc.master.playerController.canWaterJump)
                {
                    mc.ChangePlayerState(PlayerStateType.playerWaterJump);
                    return;
                }
            }

            if (!mc.isPartialSubmerged)
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            
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

        public void UpdateStateAnimator(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerDefault].UpdateStateAnimator(mc);
        }

        public void UpdateStateSlide(PlayerController mc)
        {
            return;
        }

        public void UpdateStateSpeed(PlayerController mc)
        {
            Vector3 old_x_z = new Vector3(mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
            Vector3 old_y = new Vector3(0, mc.rigidBody.velocity.y, 0);

            if (old_x_z.magnitude > MAX_SPEED_WATER)
            {
                old_x_z = Vector3.ClampMagnitude(old_x_z, MAX_SPEED_WATER);
            }

            if (old_y.y < -MAX_SPEED_WATER_SINK)
            {
                //old_y = Vector3.ClampMagnitude(old_y, MAX_SPEED_WATER_SINK);
                old_y.y = -MAX_SPEED_WATER_SINK;
            }

            mc.rigidBody.velocity = old_x_z + old_y;
        }

        public void UpdateStateDragAndFriction(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerDefault].UpdateStateDragAndFriction(mc);
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.playerWaterDefault;
        }
    }
}
