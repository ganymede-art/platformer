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
    public class PlayerStateWaterJumpController : MonoBehaviour, IPlayerStateController
    {
        int update_count_water_jump = 0;

        private PlayerStateJumpController state_jump;

        private void Start()
        {
            state_jump = GameMasterController.GlobalPlayerController
                .stateControllers[PlayerStateType.playerJump] as PlayerStateJumpController;
        }

        public void BeginState(PlayerController mc)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("water_jump_up");

            update_count_water_jump = 0;

            // enter jump state.
            // reset jump power.

            mc.jumpPersistEnergy = JUMP_PERSIST_ENERGY_MAX;

            // add jumping force.

            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);

            mc.rigidBody.AddForce(Vector3.up * WATER_JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);

            // player sound.

            mc.audioSource.clip = mc.soundWaterJump;
            mc.audioSource.Play();
        }

        public void CheckState(PlayerController mc)
        {
            update_count_water_jump++;

            // exit to water dive if pressing interact.

            if (mc.isRaisedInteract && mc.master.playerController.canWaterDive)
            {
                mc.ChangePlayerState(PlayerStateType.playerWaterDive);
                return;
            }

            if (mc.rigidBody.velocity.y <= 0)
            {
                mc.ChangePlayerState(PlayerStateType.playerWaterDefault);
                return;
            }

            if (!mc.isPartialSubmerged)
            {
                mc.ChangePlayerState(PlayerStateType.playerJump);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            
        }

        public void UpdateState(PlayerController mc)
        {
            state_jump.UpdateStateJump(mc);
            state_jump.UpdateStateMovement(mc);
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

            if (old_y.y < -MAX_SPEED_WATER)
            {
                old_y = Vector3.ClampMagnitude(old_y, MAX_SPEED_WATER);
            }

            mc.rigidBody.velocity = old_x_z + old_y;
        }

        public void UpdateStateDragAndFriction(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerJump].UpdateStateDragAndFriction(mc);
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.playerWaterJump;
        }
    }


}
