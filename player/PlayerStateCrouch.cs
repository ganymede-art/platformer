using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Script;

using static Assets.Script.PlayerConstants;

namespace Assets.Script
{
    public class PlayerStateCrouch : MonoBehaviour, IPlayerState
    {
        public void BeginState(PlayerController mc, params object[] parameters)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger(TRIGGER_CROUCH);

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0.0F, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            // exit to default if no long grounded.

            if(!mc.isSpherecastGrounded)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }

            // exit to default if crouch
            // button released.

            if(!mc.master.inputController.isInputEastExtra)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }

            // exit to crouch jump 
            // if jump putton pressed.

            if(mc.isRaisedSouth
                && mc.master.playerController.canHighJump)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_HIGH_JUMP);
                return;
            }

            // exit to shoot state if attack is pressed
            // and grounded.

            if (mc.isRaisedWest
                && mc.isSpherecastGrounded
                && mc.master.playerController.canFireProjectile
                && GamePlayerController.Global.ammo > 0)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_SHOOT);
                return;
            }
        }

        public void FinishState(PlayerController mc) { }

        public void FixedUpdateState(PlayerController mc)
        {
            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_GROUNDED);
        }

        public void UpdateState(PlayerController mc)
        {
            PlayerStaticMethods.UpdateInternalDirection(mc);
            PlayerStaticMethods.UpdateRendererDirection(mc);
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_CROUCH;
        }
    }
}
