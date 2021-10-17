using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;
using static Assets.script.PlayerEnums;
using static Assets.script.PlayerConstants;
using Assets.script.attribute;

namespace Assets.script
{
    public class PlayerStateRepelController : IPlayerStateController
    {
        int update_count_repel = 0;

        public void BeginState(PlayerMovementController mc)
        {
            update_count_repel = 0;

            // zero out velocities.
            mc.rigid_body.velocity = Vector3.zero;

            Vector3 damageVector = AttributeStaticMethods.GetAttributeDamageVector(mc.repel_type, mc.repel_source, mc.gameObject);

            mc.rigid_body.AddForce(damageVector, ForceMode.VelocityChange);
        }

        public void CheckState(PlayerMovementController mc)
        {
            update_count_repel++;

            if (update_count_repel >= 10)
            {
                mc.ChangePlayerState(PlayerEnums.PlayerState.player_default);
                return;
            }
        }

        public void FinishState(PlayerMovementController mc)
        {

        }

        public void UpdateState(PlayerMovementController mc)
        {
            //mc.state_jump.UpdateStateMovement(mc);
        }

        public void UpdateStateAnimator(PlayerMovementController mc)
        {

        }

        public void UpdateStateDragAndFriction(PlayerMovementController mc)
        {
            mc.state_jump.UpdateStateDragAndFriction(mc);
        }

        public void UpdateStateSlide(PlayerMovementController mc)
        {
            return;
        }

        public void UpdateStateSpeed(PlayerMovementController mc)
        {
            mc.state_jump.UpdateStateSpeed(mc);
        }
    }
}
