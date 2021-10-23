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
    public class PlayerStateDamageController : MonoBehaviour, IPlayerStateController
    {
        int update_count_damage = 0;

        public void BeginState(PlayerController mc)
        {
            update_count_damage = 0;

            // zero out velocities.
            mc.rigidBody.velocity = Vector3.zero;

            Vector3 damageVector = AttributeStaticMethods.GetAttributeDamageVector(mc.damageData, mc.damageSourceObject, mc.gameObject);

            mc.rigidBody.AddForce(damageVector, ForceMode.VelocityChange);

            // play  damage sound if defined.
            // or player default damage sound.

            mc.audioSource.clip = mc.damageData.damageSound ?? mc.soundHurt;
            mc.audioSource.Play();
        }

        public void CheckState(PlayerController mc)
        {
            update_count_damage++;

            if (update_count_damage >= 100)
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
            
        }

        public void UpdateState(PlayerController mc)
        {
            damageMovement(mc);
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

        public void UpdateStateAnimator(PlayerController mc)
        {
            return;
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
            return PlayerStateType.playerDamage;
        }
    }
}
