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
    public class PlayerStateCrouchController : IPlayerStateController
    {
        public void BeginState(PlayerMovementController mc)
        {
            mc.player_animator.SetTrigger("anim_crouch");
        }

        public void CheckState(PlayerMovementController mc)
        {
            // exit to default if no long grounded.

            if(!mc.is_spherecast_grounded)
            {
                mc.ChangePlayerState(PlayerState.player_default);
                return;
            }

            // exit to default if crouch
            // button released.

            if(!mc.master.input_controller.isInputPositive2)
            {
                mc.ChangePlayerState(PlayerState.player_default);
                return;
            }

            // exit to crouch jump 
            // if jump putton pressed.

            if(mc.is_raised_positive)
            {
                mc.ChangePlayerState(PlayerState.player_crouch_jump);
                return;
            }
        }

        public void FinishState(PlayerMovementController mc)
        {
            return;
        }

        public void UpdateState(PlayerMovementController mc)
        {
            mc.state_default.UpdateStateSpeed(mc);
        }

        public void UpdateStateAnimator(PlayerMovementController mc)
        {
            return;
        }

        public void UpdateStateDragAndFriction(PlayerMovementController mc)
        {
            mc.rigid_body.drag = DRAG_AIR;
            mc.player_sphere_collider.material.dynamicFriction = 0.1f;
            mc.player_sphere_collider.material.staticFriction = 0.1f;
            mc.player_sphere_collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        }

        public void UpdateStateSlide(PlayerMovementController mc)
        {
            mc.state_default.UpdateStateSlide(mc);
        }

        public void UpdateStateSpeed(PlayerMovementController mc)
        {
            mc.state_default.UpdateStateSpeed(mc);
        }
    }
}
