using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;
using static Assets.script.PlayerEnums;
using static Assets.script.PlayerConstants;

namespace Assets.script
{
    public class PlayerStateDefaultController : IPlayerStateController
    {
        int update_count_default = 0;

        // slide constants.

        const float SLIDE_ANGLE_MIN = 50f;                              // minimum angle to start sliding
        const float SLIDE_RESISTANCE_GROUND_ANGLE_MULTIPLIER = 0.001f;  // multiplier for ground angle to subtract from resistance.
        const float SLIDE_RESISTANCE_MAX = 1.0f;                        // maximum slide resistance.

        // variables.

        public void BeginState(PlayerMovementController mc)
        {
            update_count_default = 0;
        }

        public void CheckState(PlayerMovementController mc)
        {
            update_count_default++;

            // enter jumping state if right criteria are met.

            if (mc.is_raised_positive && mc.is_spherecast_grounded)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_jump);
                return;
            }

            if (mc.is_partial_submerged)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_water_default);
                return;
            }

            // exit to attack state if attack is pressed
            // and grounded.

            if(mc.is_raised_interact
                && mc.is_spherecast_grounded)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_attack);
                return;
            }

            // exit to crouch state if positive 3 is pressed
            // and grounded.

            if(mc.master.input_controller.isInputPositive2
                && mc.is_spherecast_grounded)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_crouch);
                return;
            }

            // exit to diving state, if previous state was jump,
            // if not grounded since entering this state, 
            // attack is pressed, and in air.

            if (mc.is_raised_interact
                && mc.player_state_previous == PlayerState.player_jump
                && !mc.is_spherecast_grounded_since_state_begin
                && !mc.is_spherecast_grounded)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_dive);
                return;
            }
        }

        public void FinishState(PlayerMovementController mc)
        {
            return;
        }

        public void UpdateState(PlayerMovementController mc)
        {
            UpdateStateMovement(mc);
        }

        public void UpdateStateMovement(PlayerMovementController mc)
        {
            // input movement relative to camera.

            var camera_relative_movement = Quaternion.Euler(0, mc.camera_object.transform.eulerAngles.y, 0) * mc.input_directional;

            // camera movement relative to slope.

            var slope_relative_movement = Vector3.ProjectOnPlane(camera_relative_movement, mc.spherecast_grounded_slope_normal);

            // acceleration

            float acceleration = mc.is_spherecast_grounded ? PlayerConstants.ACCELERATION_GROUNDED : PlayerConstants.ACCELERATION_AIR;

            // force

            var force = slope_relative_movement * acceleration;

            PlayerStaticMethods.StepMovement(mc, slope_relative_movement, force);
        }

        public void UpdateStateSlide(PlayerMovementController mc)
        {
            // move towards the sliding state
            // if the right criteria are met.

            if (mc.raycast_grounded_slope_angle > SLIDE_ANGLE_MIN
                || mc.ground_type == GameConstants.GroundType.ground_slide)
            {
                // reduce the slide resistance
                mc.slide_resistance -= (mc.raycast_grounded_slope_angle * SLIDE_RESISTANCE_GROUND_ANGLE_MULTIPLIER);
            }
            else
            {
                mc.slide_resistance = SLIDE_RESISTANCE_MAX;
            }

            mc.slide_resistance = Mathf.Clamp(mc.slide_resistance, 0.0f, SLIDE_RESISTANCE_MAX);

            if (mc.slide_resistance <= 0.0f && mc.is_spherecast_grounded)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_slide);
                return;
            }
        }

        public void UpdateStateSpeed(PlayerMovementController mc)
        {
            // limit speed while in the default state.
            // maximum speed depends on criteria.

            if (mc.is_partial_submerged)
            {
                if (mc.rigid_body.velocity.magnitude > PlayerConstants.MAX_SPEED_WATER)
                {
                    mc.rigid_body.velocity = Vector3.ClampMagnitude(mc.rigid_body.velocity, PlayerConstants.MAX_SPEED_WATER);
                }
            }
            else
            {
                if (mc.is_spherecast_grounded)
                {
                    if (mc.rigid_body.velocity.magnitude > PlayerConstants.MAX_SPEED_GROUNDED)
                    {
                        mc.rigid_body.velocity = Vector3.ClampMagnitude(mc.rigid_body.velocity, PlayerConstants.MAX_SPEED_GROUNDED);
                    }
                }
                else
                {
                    Vector3 old_x_z = new Vector3(mc.rigid_body.velocity.x, 0, mc.rigid_body.velocity.z);
                    Vector3 old_y = new Vector3(0, mc.rigid_body.velocity.y, 0);

                    if (old_x_z.magnitude > PlayerConstants.MAX_SPEED_GROUNDED)
                    {

                        old_x_z = Vector3.ClampMagnitude(old_x_z, PlayerConstants.MAX_SPEED_GROUNDED);
                        mc.rigid_body.velocity = old_x_z + old_y;
                    }
                }
            }
        }

        public void UpdateStateAnimator(PlayerMovementController mc)
        {
            // update player facing direction.

            mc.facing_direction = Quaternion.Euler(0, mc.camera_object.transform.rotation.eulerAngles.y, 0) * mc.input_directional;

            mc.facing_direction_delta = Vector3.RotateTowards(mc.player_renderer_object.transform.forward, mc.facing_direction, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            mc.player_renderer_object.transform.rotation = Quaternion.LookRotation(mc.facing_direction_delta);
            mc.player_direction_object.transform.rotation = Quaternion.LookRotation(mc.facing_direction_delta);
        }

        public void UpdateStateDragAndFriction(PlayerMovementController mc)
        {
            // update based on circumstances.

            mc.rigid_body.drag = mc.is_raycast_grounded ? DRAG_GROUNDED : DRAG_AIR;

            if (mc.is_input_directional || !mc.is_raycast_grounded)
            {
                mc.player_sphere_collider.material.dynamicFriction = 0f;
                mc.player_sphere_collider.material.staticFriction = 0f;
            }
            else
            {
                if (mc.moving_object_collision_list.Count == 0)
                {
                    mc.player_sphere_collider.material.dynamicFriction = 100;
                    mc.player_sphere_collider.material.staticFriction = 100;
                }
                else
                {
                    mc.player_sphere_collider.material.dynamicFriction = 1;
                    mc.player_sphere_collider.material.staticFriction = 1;
                }
            }

            // change physics combine mode.

            mc.player_sphere_collider.material.frictionCombine = PhysicMaterialCombine.Average;
        }
    }
}
