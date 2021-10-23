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
    public class PlayerStateRepelController : MonoBehaviour, IPlayerStateController
    {
        int update_count_repel = 0;

        public void BeginState(PlayerController mc)
        {
            update_count_repel = 0;

            // zero out velocities.
            mc.rigidBody.velocity = Vector3.zero;

            Vector3 damageVector = AttributeStaticMethods.GetAttributeDamageVector(mc.repelData, mc.repelSourceObject, mc.gameObject);

            mc.rigidBody.AddForce(damageVector, ForceMode.VelocityChange);
        }

        public void CheckState(PlayerController mc)
        {
            update_count_repel++;

            if (update_count_repel >= 10)
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
            //mc.state_jump.UpdateStateMovement(mc);
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
            mc.stateControllers[PlayerStateType.playerJump].UpdateStateSpeed(mc);
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.playerRepel;
        }
    }
}
