using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Assets.script.PlayerConstants;

namespace Assets.script
{
    public class PlayerStateAttackController : MonoBehaviour, IPlayerStateController
    {
        public void BeginState(PlayerController mc)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("attack");

            // play attack sound.

            mc.audioSource.clip = mc.soundAttack;
            mc.audioSource.Play();

            // zero out vertical velocity and add diving force.

            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
            mc.rigidBody.velocity = new Vector3
                (0, 0, 0);

            mc.rigidBody.AddForce(Vector3.up * JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);
            mc.rigidBody.AddForce(mc.directionObject.transform.forward * (JUMP_FORCE_MULTIPLIER), ForceMode.VelocityChange);

            // enable the attack forward trigger.

            mc.attackForward1Collider.enabled = true;
        }

        public void CheckState(PlayerController mc)
        {
            if (mc.stateUpdateCount >= 15)
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            // disable the attack forward trigger.

            mc.attackForward1Collider.enabled = false;
        }

        public void UpdateState(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerDefault].UpdateStateSpeed(mc);
        }

        public void UpdateStateAnimator(PlayerController mc)
        {
            
        }

        public void UpdateStateDragAndFriction(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerJump].UpdateStateDragAndFriction(mc);
        }

        public void UpdateStateSlide(PlayerController mc)
        {
            return;
        }

        public void UpdateStateSpeed(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerDefault].UpdateStateSpeed(mc);
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.playerAttack;
        }
    }
}
