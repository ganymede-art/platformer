using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Script.PlayerConstants;

namespace Assets.Script
{
    public class PlayerStateFlutter : MonoBehaviour, IPlayerState
    {
        private const int DOUBLE_JUMP_PERSIST_ENERGY_MAX = 70;
        private const float DOUBLE_JUMP_ANIMATION_SPEED_MIN = 0.5F;
        private const float DOUBLE_JUMP_ANIMATION_SPEED_MAX = 5F;
        private readonly Vector3 DOUBLE_JUMP_FORCE = new Vector3(0, 0.3f, 0);
        private int doubleJumpPersistEnergy = 0;

        private float doubleJumpPersistPercentage = 1.0F;
        private float doubleJumpAnimationSpeed = 1.0F;

        private float soundTimer = 0.0F;
        private float soundInterval = 0.2F;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            // set animation.

            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger(TRIGGER_FLUTTER_UP);

            // set the persist energy for the jump.

            doubleJumpPersistEnergy = DOUBLE_JUMP_PERSIST_ENERGY_MAX;

            // zero out current vertical velocity.

            mc.rigidBody.angularVelocity = Vector3.zero;
            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);

            // play double jump sound.

            mc.audioSource.clip = mc.flutterSound;
            mc.audioSource.Play();

            // set the sound timer.

            soundTimer = 0.0F;

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            if (doubleJumpPersistEnergy <= 0 || mc.isSpherecastGrounded)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_FLUTTER;
        }

        public void FixedUpdateState(PlayerController mc)
        {
            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_GROUNDED);

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

            PlayerStaticMethods.FullMovement(mc, camera_relative_movement, force);
        }

        public void UpdateState(PlayerController mc)
        {
            // update the animation speed.

            doubleJumpAnimationSpeed = Mathf.Lerp
                (DOUBLE_JUMP_ANIMATION_SPEED_MIN, DOUBLE_JUMP_ANIMATION_SPEED_MAX, doubleJumpPersistPercentage);

            mc.playerAnimator.SetFloat(TRIGGER_SPEED_MULTIPLIER, doubleJumpAnimationSpeed);

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
                soundTimer = 0.0F;
                mc.audioSource.clip = mc.flutterSound;
                mc.audioSource.Play();
            }
        }
    }
}
