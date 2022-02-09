using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;

using static Assets.script.PlayerConstants;
using Assets.script.attribute;

namespace Assets.script
{
    public class PlayerStateDamageController : MonoBehaviour, IPlayerState
    {
        int update_count_damage = 0;
        private Vector3 damageVector = Vector3.zero;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            update_count_damage = 0;

            // zero out velocities.
            mc.rigidBody.velocity = Vector3.zero;

            // apply damage vector, from params.

            Vector3 damageVector = Vector3.zero;

            if (parameters == null || parameters.Length == 0)
                damageVector = Vector3.up;
            else
            {
                var damageSourceObject = (GameObject)parameters[0];
                var damageData = (AttributeDamageData)parameters[1];
                damageVector = AttributeStaticMethods.GetAttributeDamageVector
                    (damageData, damageSourceObject, gameObject);
            }

            mc.rigidBody.AddForce(damageVector, ForceMode.VelocityChange);

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            update_count_damage++;

            if (update_count_damage >= 100)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            
        }

        public void FixedUpdateState(PlayerController mc)
        {
            damageMovement(mc);
            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_GROUNDED);
        }

        private void damageMovement(PlayerController mc)
        {
            if (mc.isSpherecastGrounded)
                return;

            // input movement relative to camera.

            var camera_relative_movement = Quaternion.Euler(0, mc.cameraObject.transform.eulerAngles.y, 0) * mc.inputDirectional;

            // force

            var force = camera_relative_movement * PlayerConstants.ACCELERATION_AIR;

            // move.

            PlayerStaticMethods.Movement(mc, camera_relative_movement, force);
        }

        public void UpdateState(PlayerController mc)
        {
            if (mc.rigidBody.velocity.y > 0)
            {
                mc.playerAnimator.ResetAllAnimatorTriggers();
                mc.playerAnimator.SetTrigger("damage_up");
            }
            else
            {
                mc.playerAnimator.ResetAllAnimatorTriggers();
                mc.playerAnimator.SetTrigger("damage_down");
            }
        }

        public void FixedUpdateStateSlide(PlayerController mc)
        {
            return;
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_DAMAGE;
        }
    }
}
