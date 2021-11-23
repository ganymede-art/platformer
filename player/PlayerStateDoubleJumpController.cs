using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    public class PlayerStateDoubleJumpController : MonoBehaviour, IPlayerStateController
    {
        private const int DOUBLE_JUMP_PERSIST_ENERGY_MAX = 70;
        private const float DOUBLE_JUMP_ANIMATION_SPEED_MIN = 0.5f;
        private const float DOUBLE_JUMP_ANIMATION_SPEED_MAX = 5f;
        private readonly Vector3 DOUBLE_JUMP_FORCE = new Vector3(0, 0.3f, 0);
        private int doubleJumpPersistEnergy = 0;

        private float doubleJumpPersistPercentage = 1.0f;
        private float doubleJumpAnimationSpeed = 1.0f;

        private float soundTimer = 0.0f;
        private float soundInterval = 0.2f;
        

        public void BeginState(PlayerController mc)
        {
            // set animation.

            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("double_jump_up");

            // set the persist energy for the jump.

            doubleJumpPersistEnergy = DOUBLE_JUMP_PERSIST_ENERGY_MAX;

            // zero out current vertical velocity.

            mc.rigidBody.angularVelocity = Vector3.zero;
            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);

            // play double jump sound.

            mc.audioSource.clip = mc.soundDoubleJump;
            mc.audioSource.Play();

            // set the sound timer.

            soundTimer = 0.0f;
        }

        public void CheckState(PlayerController mc)
        {
            if (doubleJumpPersistEnergy <= 0 || mc.isSpherecastGrounded)
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.PlayerDoubleJump;
        }

        public void UpdateState(PlayerController mc)
        {
            UpdateStateMovement(mc);

            doubleJumpPersistPercentage = Mathf.InverseLerp
                (0, DOUBLE_JUMP_PERSIST_ENERGY_MAX, doubleJumpPersistEnergy);

            if (GameInputController.Global.isInputSouth)
            {
                doubleJumpPersistEnergy--;

                mc.rigidBody.AddForce(DOUBLE_JUMP_FORCE * doubleJumpPersistPercentage, ForceMode.VelocityChange);
            }
            else
            {
                doubleJumpPersistEnergy = 0;
            }
        }

        public void UpdateStateMovement(PlayerController mc)
        {
            // input movement relative to camera.

            var camera_relative_movement = Quaternion.Euler(0, mc.cameraObject.transform.eulerAngles.y, 0) * mc.inputDirectional;

            // force

            var force = camera_relative_movement * PlayerConstants.ACCELERATION_AIR;

            PlayerStaticMethods.StepMovement(mc, camera_relative_movement, force);
        }

        public void UpdateStateAnimator(PlayerController mc)
        {
            // update the animation speed.

            doubleJumpAnimationSpeed = Mathf.Lerp
                (DOUBLE_JUMP_ANIMATION_SPEED_MIN, DOUBLE_JUMP_ANIMATION_SPEED_MAX, doubleJumpPersistPercentage);

            mc.playerAnimator.SetFloat("speed_multiplier", doubleJumpAnimationSpeed);

            // update player facing direction.

            mc.facingDirection = Quaternion.Euler(0, mc.cameraObject.transform.rotation.eulerAngles.y, 0) * mc.inputDirectional;

            mc.facingDirectionDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, mc.facingDirection, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER * 0.3f, 0.0f);

            // Move our position a step closer to the target.
            mc.rendererObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
            mc.directionObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);

            // play sound.

            soundTimer += Time.deltaTime;
            if (soundTimer >= soundInterval)
            {
                soundTimer = 0.0f;
                mc.audioSource.clip = mc.soundDoubleJump;
                mc.audioSource.Play();
            }
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
    }
}
