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
    public class PlayerStateCrouchController : MonoBehaviour, IPlayerStateController
    {
        public void BeginState(PlayerController mc)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("crouch");
        }

        public void CheckState(PlayerController mc)
        {
            // exit to default if no long grounded.

            if(!mc.isSpherecastGrounded)
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }

            // exit to default if crouch
            // button released.

            if(!mc.master.inputController.isInputEastExtra)
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }

            // exit to crouch jump 
            // if jump putton pressed.

            if(mc.isRaisedSouth
                && mc.master.playerController.canCrouchJump)
            {
                mc.ChangePlayerState(PlayerStateType.playerCrouchJump);
                return;
            }

            // exit to shoot state if attack is pressed
            // and grounded.

            if (mc.isRaisedWest
                && mc.isSpherecastGrounded
                && mc.master.playerController.canAttack)
            {
                mc.ChangePlayerState(PlayerStateType.PlayerShoot);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            return;
        }

        public void UpdateState(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerDefault].UpdateStateSpeed(mc);
        }

        public void UpdateStateAnimator(PlayerController mc)
        {
            return;
        }

        public void UpdateStateDragAndFriction(PlayerController mc)
        {
            mc.rigidBody.drag = DRAG_AIR;
            mc.rbCollider.material.dynamicFriction = 0.1f;
            mc.rbCollider.material.staticFriction = 0.1f;
            mc.rbCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        }

        public void UpdateStateSlide(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerDefault].UpdateStateSlide(mc);
        }

        public void UpdateStateSpeed(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerDefault].UpdateStateSpeed(mc);
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.playerCrouch;
        }
    }
}
