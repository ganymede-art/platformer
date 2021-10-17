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
    public class PlayerStateAttackController : IPlayerStateController
    {
        public void BeginState(PlayerMovementController mc)
        {
            mc.player_animator.SetTrigger("anim_attack");

            // play attack sound.

            mc.audio_source.clip = mc.sfx_player_dive;
            mc.audio_source.Play();

            // zero out vertical velocity and add diving force.

            mc.rigid_body.velocity = new Vector3
                (mc.rigid_body.velocity.x, 0, mc.rigid_body.velocity.z);
            mc.rigid_body.velocity = new Vector3
                (0, 0, 0);

            mc.rigid_body.AddForce(Vector3.up * JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);
            mc.rigid_body.AddForce(mc.player_direction_object.transform.forward * (JUMP_FORCE_MULTIPLIER), ForceMode.VelocityChange);

            // enable the attack forward trigger.

            mc.player_attack_forward_collider.enabled = true;
        }

        public void CheckState(PlayerMovementController mc)
        {
            if (mc.state_update_count >= 15)
            {
                mc.ChangePlayerState(PlayerState.player_default);
                return;
            }
        }

        public void FinishState(PlayerMovementController mc)
        {
            // disable the attack forward trigger.

            mc.player_attack_forward_collider.enabled = false;
        }

        public void UpdateState(PlayerMovementController mc)
        {
            mc.state_default.UpdateStateSpeed(mc);
        }

        public void UpdateStateAnimator(PlayerMovementController mc)
        {
            
        }

        public void UpdateStateDragAndFriction(PlayerMovementController mc)
        {
            mc.state_jump.UpdateStateDragAndFriction(mc);
        }

        public void UpdateStateSlide(PlayerMovementController mc)
        {
            return;
        }

        public void UpdateStateSpeed(PlayerMovementController mc)
        {
            mc.state_default.UpdateStateSpeed(mc);
        }

        
    }
}
