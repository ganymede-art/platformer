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
    public class PlayerStateRepel : MonoBehaviour, IPlayerState
    {
        int update_count_repel = 0;
        private Vector3 repelVector = Vector3.zero;

        public void BeginState(PlayerController mc, params object[] parameters)
        {
            update_count_repel = 0;

            // play dive animation.

            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger(TRIGGER_HURT_UP);

            // zero out velocities.
            mc.rigidBody.velocity = Vector3.zero;

            if (parameters == null || parameters.Length == 0)
                repelVector = Vector3.up;
            else
            {
                var repelSourceObject = (GameObject)parameters[0];
                var repelData = (DamageData)parameters[1];

                Vector3 verticalVector = Vector3.zero;
                verticalVector.y = repelData.verticalForceMultiplier;

                Vector3 horizontalVector = (mc.transform.position - repelSourceObject.transform.position).normalized;
                horizontalVector *= repelData.horizontalForceMultiplier;

                repelVector = verticalVector + horizontalVector;
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
