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
    public class PlayerStateRepelController : MonoBehaviour, IPlayerState
    {
        int update_count_repel = 0;
        private Vector3 repelVector = Vector3.zero;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            update_count_repel = 0;

            // play dive animation.

            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("repel_up");

            // zero out velocities.
            mc.rigidBody.velocity = Vector3.zero;

            if (parameters == null || parameters.Length == 0)
                repelVector = Vector3.up;
            else
            {
                var repelSourceObject = (GameObject)parameters[0];
                var repelData = (AttributeDamageData)parameters[1];
                repelVector = AttributeStaticMethods.GetAttributeDamageVector
                    (repelData, repelSourceObject, gameObject);
            }

            mc.rigidBody.AddForce(repelVector, ForceMode.VelocityChange);

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            update_count_repel++;

            if (update_count_repel >= 10)
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
            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_GROUNDED);
        }

        public void UpdateState(PlayerController mc)
        {

        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_REPEL;
        }
    }
}
