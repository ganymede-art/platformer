using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.script.PlayerEnums;
using static Assets.script.PlayerConstants;

namespace Assets.script
{
    public class PlayerStateWaterDiveController : IPlayerStateController
    {
        int update_count_water_dive = 0;
        int update_count_interact_released = 0;
        int update_count_positive_released = 0;

        float input_horizontal = 0f;
        float input_vertical = 0f;

        float turning_vertical = 0f;

        public void BeginState(PlayerMovementController mc)
        {
            update_count_water_dive = 0;
            update_count_interact_released = 0;
            update_count_positive_released = 0;

            // set camera to start auto rotation.

            mc.camera_object.GetComponent<CameraController>().SetAutoRotation();

            // set physics.

            mc.is_under_gravity = false;

            mc.audio_source.clip = mc.master.audio_controller.a_player_water_jump;
            mc.audio_source.Play();

            mc.dive_direction = mc.player_renderer_object.transform.forward.normalized;

            // zero out pitch, unless previous
            // state was also water dive.

            if (mc.player_state_previous != PlayerState.player_water_dive)
                turning_vertical = 0.0f;

            // zero out vertical velocity and add diving force.

            mc.rigid_body.velocity = new Vector3
                (mc.rigid_body.velocity.x, 0, mc.rigid_body.velocity.z);

            mc.rigid_body.AddForce(-Physics.gravity, ForceMode.Acceleration);
            mc.rigid_body.AddForce(mc.dive_direction * (JUMP_FORCE_MULTIPLIER * 2), ForceMode.VelocityChange);

            // enable the attack forward trigger.

            mc.player_attack_forward_collider.enabled = true;
        }

        public void CheckState(PlayerMovementController mc)
        {
            update_count_water_dive++;

            if (!mc.master.input_controller.Is_Input_Interact)
                update_count_interact_released++;
            else
                update_count_interact_released = 0;

            if (!mc.master.input_controller.Is_Input_Positive)
                update_count_positive_released++;
            else
                update_count_positive_released = 0;

            // water dive again if button raised.

            if (mc.is_raised_interact 
                && update_count_water_dive >= UPDATE_COUNT_WATER_DIVE_REENTRY_MIN)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_water_dive);
                return;
            }

            // stop diving if button released for a time.

            if (update_count_interact_released >= UPDATE_COUNT_WATER_DIVE_RECOVERY_MIN
                && update_count_positive_released >= UPDATE_COUNT_WATER_DIVE_RECOVERY_MIN)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_water_default);
                return;
            }

            // stop diving if outside of water.

            if (!mc.is_partial_submerged)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_jump);
                return;
            }
        }

        public void FinishState(PlayerMovementController mc)
        {
            // set camera to start auto rotation.

            mc.camera_object.GetComponent<CameraController>().UnsetAutoRotation();

            // set physics.

            mc.is_under_gravity = true;

            // disable the attack forward trigger.

            mc.player_attack_forward_collider.enabled = false;
        }

        public void UpdateState(PlayerMovementController mc)
        {
            UpdateStateMovement(mc);
            UpdateStateSpeed(mc);
        }

        public void UpdateStateAnimator(PlayerMovementController mc)
        {
            // update the internal direction transform.

            mc.facing_direction = new Vector3(mc.dive_direction.x, 0, mc.dive_direction.z);
            mc.facing_direction_delta = Vector3.RotateTowards(mc.player_direction_object.transform.forward, mc.facing_direction, ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move direction transform a step closer to the target.
            mc.player_direction_object.transform.rotation = Quaternion.LookRotation(mc.facing_direction_delta);

            // tilt the renderer to the swimming direction.

            mc.facing_direction = mc.dive_direction;
            mc.facing_direction_delta = Vector3.RotateTowards(mc.player_renderer_object.transform.forward, mc.facing_direction, ANIMATION_TURNING_SPEED_WATER_DIVE_MULTIPLIER, 0.0f);

            mc.player_renderer_object.transform.rotation = Quaternion.LookRotation(mc.facing_direction_delta);

        }

        public void UpdateStateMovement(PlayerMovementController mc)
        {
            input_horizontal = mc.input_directional.x;
            input_vertical = mc.input_directional.z;

            float horizontal_turning_rate = input_horizontal * 0.075f;
            float vertical_turning_rate = input_vertical * 0.05f;

            turning_vertical += vertical_turning_rate;
            turning_vertical = Mathf.Clamp(turning_vertical, -2, 2);

            mc.dive_direction
                = mc.player_direction_object.transform.forward
                + mc.player_direction_object.transform.right * horizontal_turning_rate
                + mc.player_direction_object.transform.up * turning_vertical;

            if (!mc.master.input_controller.Is_Input_Positive)
                return;

            mc.rigid_body.AddForce(mc.dive_direction.normalized * 0.1f, ForceMode.VelocityChange);
        }

        public void UpdateStateSpeed(PlayerMovementController mc)
        {
            if (update_count_water_dive <= UPDATE_COUNT_WATER_DIVE_REENTRY_MIN)
            {
                if (mc.rigid_body.velocity.magnitude > MAX_SPEED_WATER*2)
                    mc.rigid_body.velocity = mc.rigid_body.velocity.normalized * MAX_SPEED_WATER*2;
            }
            else
            {
                if (mc.rigid_body.velocity.magnitude > MAX_SPEED_WATER)
                    mc.rigid_body.velocity = mc.rigid_body.velocity.normalized * MAX_SPEED_WATER;
            }

            
        }
    }
}
