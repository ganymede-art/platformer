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
    public class PlayerStateSlam : MonoBehaviour, IPlayerState
    {
        const float HORIZONTAL_VELOCITY_MULTIPLIER = 0.25F;

        private bool hasSlamBegun;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            hasSlamBegun = false;

            // play slam animation.

            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger(TRIGGER_SLAM_UP);

            // play slam up sound.

            mc.audioSource.clip = mc.slamUpSound;
            mc.audioSource.Play();

            // damp velocity and add upward force.

            mc.rigidBody.velocity = new Vector3(
                mc.rigidBody.velocity.x * HORIZONTAL_VELOCITY_MULTIPLIER,
                0, 
                mc.rigidBody.velocity.z * HORIZONTAL_VELOCITY_MULTIPLIER);

            mc.rigidBody.AddForce(Vector3.up * JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);

            // enable the attack forward trigger.

            mc.attackDown1Collider.enabled = true;

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            if (mc.isChecksphereGrounded)
            {
                Debug.Log("Bonked at " + Time.time + " angle: " + mc.spherecastGroundAngle + " hit: " + mc.isSpherecastHit + " normaly: " + mc.spherecastGroundNormal.y + " ypos: " + mc.spherecastHitInfo.point.y);
                // play slam impact sound.

                mc.audioSource.clip = mc.slamImpactSound;
                mc.audioSource.Play();

                // play slam impact fx.

                mc.impactDownFx.Play();

                // add a small upward force.

                mc.rigidBody.velocity = new Vector3(
                mc.rigidBody.velocity.x,
                0,
                mc.rigidBody.velocity.z);

                mc.rigidBody.AddForce(Vector3.up * JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);

                // exit to default state.

                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            // disable the attack forward trigger.

            mc.attackDown1Collider.enabled = false;
        }

        public void FixedUpdateState(PlayerController mc)
        {
            if(mc.rigidBody.velocity.y < 0 && !hasSlamBegun)
            {
                // beginning the downward slam.

                hasSlamBegun = true;
                mc.playerAnimator.ResetAllAnimatorTriggers();
                mc.playerAnimator.SetTrigger(TRIGGER_SLAM_DOWN);

                // play slam down sound.

                mc.audioSource.clip = mc.slamDownSound;
                mc.audioSource.Play();
            }

            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_DIVE);
        }

        public void UpdateState(PlayerController mc)
        {

        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_SLAM ;
        }
    }
}
