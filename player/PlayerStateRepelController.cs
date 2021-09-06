using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;
using static Assets.script.PlayerEnums;
using static Assets.script.PlayerConstants;

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

            if (mc.repel_type.repel_direction_type
                == GameConstants.DamageDirectionType.type_up)
            {
                mc.rigid_body.AddForce(Vector3.up * mc.repel_type.repel_force_multiplier, ForceMode.VelocityChange);
            }
            else if (mc.repel_type.repel_direction_type
                == GameConstants.DamageDirectionType.type_push)
            {
                mc.rigid_body.AddForce(Vector3.up * JUMP_FORCE_MULTIPLIER, ForceMode.VelocityChange);
                mc.rigid_body.AddForce((mc.transform.position - mc.repel_source.transform.position) * mc.repel_type.repel_force_multiplier, ForceMode.VelocityChange);
            }
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
    }
}
