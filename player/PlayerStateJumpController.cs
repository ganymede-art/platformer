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
    public class PlayerStateJumpController : IPlayerStateController
    {
        // variables.

        int update_count_jump = 0;

        RaycastHit movement_hit;
        RaycastHit step_movement_hit;

        bool is_movement_hit = false;
        bool is_step_movement_hit = false;

        public void BeginState(PlayerMovementController mc)
        {
            // reset the update count.

            update_count_jump = 0;

            // if coming from the water jump state,
            // don't add any additional force.

            if (mc.player_state_previous == PlayerState.player_water_jump)
                return;

            // enter jump state.
            // reset jump power.

            mc.jump_persist_energy = PlayerConstants.JUMP_PERSIST_ENERGY_MAX;

            // add jumping force.

            mc.rigid_body.velocity = new Vector3
                (mc.rigid_body.velocity.x, 0, mc.rigid_body.velocity.z);

            mc.rigid_body.AddForce(Vector3.up * PlayerConstants.JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);

            // player sound.

            mc.audio_source.clip = mc.sfx_player_jump;
            mc.audio_source.Play();
        }

        public void CheckState(PlayerMovementController mc)
        {
            // increment the update count.

            update_count_jump++;

            // enter default state if right criteria are met.

            if (mc.rigid_body.velocity.y <= 0)
            {
                mc.ChangePlayerState(PlayerState.player_default);
                return;
            }

            // enter default state if grounded.

            if (update_count_jump >= PlayerConstants.UPDATE_COUNT_JUMP_RECOVERY_MIN
                && (mc.is_raycast_grounded || mc.is_spherecast_grounded))
            {
                mc.ChangePlayerState(PlayerState.player_default);
                return;
            }

            // enter dive state.

            if (mc.is_raised_interact)
            {
                mc.ChangePlayerState(PlayerState.player_dive);
                return;
            }
        }

        public void FinishState(PlayerMovementController mc)
        {
            return;
        }

        public void UpdateState(PlayerMovementController mc)
        {
            UpdateStateJump(mc);
            UpdateStateMovement(mc);
        }

        public void UpdateStateAnimator(PlayerMovementController mc)
        {
            if (mc.state_update_count ==  0)
                mc.player_renderer_object.transform.rotation = Quaternion.LookRotation(mc.player_direction_object.transform.forward);

            // update player facing direction.

            mc.facing_direction = Quaternion.Euler(0, mc.camera_object.transform.rotation.eulerAngles.y, 0) * mc.input_directional;

            mc.facing_direction_delta = Vector3.RotateTowards(mc.player_renderer_object.transform.forward, mc.facing_direction, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            mc.player_direction_object.transform.rotation = Quaternion.LookRotation(mc.facing_direction);
            
        }

        public void UpdateStateJump(PlayerMovementController mc)
        {
            // decrement the remaining jump persist energy.

            mc.jump_persist_energy -= 1;

            if (mc.master.input_controller.isInputPositive && mc.jump_persist_energy > 0)
            {
                // if the jump input is given, and persist energy > 0, add extra jump force.

                mc.rigid_body.AddForce(Vector3.up * PlayerConstants.JUMP_PERSIST_FORCE_MULTIPLIER, ForceMode.VelocityChange);
            }
            else
            {
                // if the jump input is let go, zero out the jump persist energy.

                mc.jump_persist_energy = 0;
            }
        }

        public void UpdateStateMovement(PlayerMovementController mc)
        {
            // input movement relative to camera.

            var camera_relative_movement = Quaternion.Euler(0, mc.camera_object.transform.eulerAngles.y, 0) * mc.input_directional;

            // force

            var force = camera_relative_movement * PlayerConstants.ACCELERATION_AIR;

            PlayerStaticMethods.StepMovement(mc, camera_relative_movement, force);
        }

        public void UpdateStateSlide(PlayerMovementController mc)
        {
            return;
        }

        public void UpdateStateSpeed(PlayerMovementController mc)
        {
            Vector3 old_x_z = new Vector3(mc.rigid_body.velocity.x, 0, mc.rigid_body.velocity.z);
            Vector3 old_y = new Vector3(0, mc.rigid_body.velocity.y, 0);

            if (old_x_z.magnitude > PlayerConstants.MAX_SPEED_GROUNDED)
            {

                old_x_z = Vector3.ClampMagnitude(old_x_z, PlayerConstants.MAX_SPEED_GROUNDED);
                mc.rigid_body.velocity = old_x_z + old_y;
            }
        }

        public void UpdateStateDragAndFriction(PlayerMovementController mc)
        {
            mc.rigid_body.drag = DRAG_AIR;
            mc.player_sphere_collider.material.dynamicFriction = 0f;
            mc.player_sphere_collider.material.staticFriction = 0f;

            mc.player_sphere_collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        }


    }
}
