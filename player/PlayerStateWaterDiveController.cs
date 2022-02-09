using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Assets.script.PlayerConstants;

namespace Assets.script
{
    public class PlayerStateWaterDiveController : MonoBehaviour, IPlayerState
    {
        int updateCountInteractReleased = 0;
        int updateCountPositiveReleased = 0;

        float input_horizontal = 0F;
        float input_vertical = 0F;

        float turning_vertical = 0F;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            // set the animation.

            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("water_dive");

            updateCountInteractReleased = 0;
            updateCountPositiveReleased = 0;

            // set camera to start auto rotation.

            mc.cameraObject.GetComponent<CameraController>().SetAutoRotation();

            // set physics.

            mc.isUnderGravity = false;

            mc.audioSource.clip = mc.waterJumpSound;
            mc.audioSource.Play();

            mc.diveDirection = mc.rendererObject.transform.forward.normalized;

            // zero out pitch, unless previous
            // state was also water dive.

            if (mc.previousStateType != GameConstants.PLAYER_STATE_WATER_DIVE)
                turning_vertical = 0.0F;

            // zero out vertical velocity and add diving force.

            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);

            mc.rigidBody.AddForce(-Physics.gravity, ForceMode.Acceleration);
            mc.rigidBody.AddForce(mc.diveDirection * (JUMP_FORCE_MULTIPLIER * 2), ForceMode.VelocityChange);

            // enable the attack forward trigger.

            mc.attackForward1Collider.enabled = true;

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            if (!mc.master.inputController.inInputWest)
                updateCountInteractReleased++;
            else
                updateCountInteractReleased = 0;

            if (!mc.master.inputController.isInputSouth)
                updateCountPositiveReleased++;
            else
                updateCountPositiveReleased = 0;

            // water dive again if button raised.

            if (mc.isRaisedWest 
                && mc.stateFixedUpdateCount >= UPDATE_COUNT_WATER_DIVE_RESTART_MIN)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_WATER_DIVE);
                return;
            }

            // stop diving if button released for a time.

            if (updateCountInteractReleased >= UPDATE_COUNT_WATER_DIVE_RECOVERY_MIN
                && updateCountPositiveReleased >= UPDATE_COUNT_WATER_DIVE_RECOVERY_MIN)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_WATER_DEFAULT);
                return;
            }

            // stop diving if outside of water.

            if (!mc.isPartialSubmerged)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_JUMP);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            // set camera to start auto rotation.

            mc.cameraObject.GetComponent<CameraController>().UnsetAutoRotation();

            // set physics.

            mc.isUnderGravity = true;

            // disable the attack forward trigger.

            mc.attackForward1Collider.enabled = false;
        }

        public void FixedUpdateState(PlayerController mc)
        {
            UpdateStateMovement(mc);
            FixedUpdateStateSpeed(mc);
        }

        public void UpdateState(PlayerController mc)
        {
            // update the animation speed.

            mc.playerAnimator.SetFloat("speed_multiplier", mc.rigidBody.velocity.magnitude / 2.0F);

            // update the internal direction transform.

            mc.facingDirection = new Vector3(mc.diveDirection.x, 0, mc.diveDirection.z);
            mc.facingDirectionDelta = Vector3.RotateTowards(mc.directionObject.transform.forward, mc.facingDirection, ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move direction transform a step closer to the target.
            mc.directionObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);

            // tilt the renderer to the swimming direction.

            mc.facingDirection = mc.diveDirection;
            mc.facingDirectionDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, mc.facingDirection, ANIMATION_TURNING_SPEED_WATER_DIVE_MULTIPLIER, 0.0f);

            mc.rendererObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
        }

        public void UpdateStateMovement(PlayerController mc)
        {
            input_horizontal = mc.inputDirectional.x;
            input_vertical = mc.inputDirectional.z;

            float horizontal_turning_rate = input_horizontal * 0.075F;
            float vertical_turning_rate = input_vertical * 0.05F;

            turning_vertical += vertical_turning_rate;
            turning_vertical = Mathf.Clamp(turning_vertical, -2, 2);

            mc.diveDirection
                = mc.directionObject.transform.forward
                + mc.directionObject.transform.right * horizontal_turning_rate
                + mc.directionObject.transform.up * turning_vertical;

            if (!mc.master.inputController.isInputSouth)
                return;

            mc.rigidBody.AddForce(mc.diveDirection.normalized * 0.1f, ForceMode.VelocityChange);
        }

        public void FixedUpdateStateSpeed(PlayerController mc)
        {
            if (mc.stateFixedUpdateCount <= UPDATE_COUNT_WATER_DIVE_RESTART_MIN)
            {
                if (mc.rigidBody.velocity.magnitude > MAX_SPEED_WATER_DIVE)
                    mc.rigidBody.velocity = mc.rigidBody.velocity.normalized * MAX_SPEED_WATER_DIVE;
            }
            else
            {
                if (mc.rigidBody.velocity.magnitude > MAX_SPEED_WATER)
                    mc.rigidBody.velocity = mc.rigidBody.velocity.normalized * MAX_SPEED_WATER;
            } 
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_WATER_DIVE;
        }
    }
}
