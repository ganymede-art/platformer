using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;
using static Assets.script.PlayerEnums;
using static Assets.script.AttributeDataClasses;

namespace Assets.script
{
    public class PlayerStaticMethods
    {
        public static void Movement(PlayerMovementController mc, Vector3 direction, Vector3 force)
        {
            // do raycasts.

            mc.is_movement_hit = Physics.SphereCast
                (mc.transform.position, PlayerConstants.MOVEMENT_SPHERECAST_RADIUS, direction, 
                out mc.movement_hit, PlayerConstants.MOVEMENT_SPHERECAST_DISTANCE,GameConstants.LAYER_MASK_ALL_BUT_ENTITIES);

            // apply forces based on raycast hits.

            if (!mc.is_movement_hit)
            {
                // no obstace directly ahead.

                mc.rigid_body.AddForce(force, ForceMode.VelocityChange);
            }
        }

        public static void StepMovement(PlayerMovementController mc, Vector3 direction, Vector3 force)
        {
            // do raycasts.

            mc.is_movement_hit = Physics.SphereCast
                (mc.transform.position, PlayerConstants.MOVEMENT_SPHERECAST_RADIUS, direction, 
                out mc.movement_hit, PlayerConstants.MOVEMENT_SPHERECAST_DISTANCE, GameConstants.LAYER_MASK_ALL_BUT_PLAYER);

            // apply forces based on raycast hits.

            if (mc.is_movement_hit)
            {
                // initial step cast.
                mc.is_step_movement_hit = Physics.SphereCast
                    (mc.transform.position + PlayerConstants.STEP_MOVEMENT_OFFSET, PlayerConstants.MOVEMENT_SPHERECAST_RADIUS, direction, out mc.step_movement_hit, PlayerConstants.MOVEMENT_SPHERECAST_DISTANCE);

                // addition step check for ceilings.
                if (Physics.CheckSphere(mc.transform.position + PlayerConstants.STEP_MOVEMENT_OFFSET, PlayerConstants.MOVEMENT_SPHERECAST_RADIUS, GameConstants.LAYER_MASK_ALL_BUT_PLAYER))
                    mc.is_step_movement_hit = true;

                if (!mc.is_step_movement_hit)
                {
                    // step obstace, move up and move directly ahead.
                    if (mc.rigid_body.velocity.y < PlayerConstants.STEP_MAX_VELOCITY)
                        mc.rigid_body.AddForce(Vector3.up, ForceMode.VelocityChange);

                    mc.rigid_body.AddForce(force, ForceMode.VelocityChange);

                    // force the sphere grounded status while moving up short steps.
                    mc.is_spherecast_grounded = true;
                }
                else
                {
                    // full obstacle, move on a plane to the collided surface.

                    force = Vector3.ProjectOnPlane(force, mc.movement_hit.normal);
                    mc.rigid_body.AddForce(force, ForceMode.VelocityChange);
                }
            }
            else
            {
                // no obstace directly ahead.

                mc.rigid_body.AddForce(force, ForceMode.VelocityChange);
            }
        }

        public static void HandleDamageObject(PlayerMovementController mc, GameObject damage_object, bool is_stay)
        {
            // get the objects damage attributes (or default)
            // the handle moving into the damage state.

            mc.damage_source = damage_object.gameObject;
            mc.damage_type = damage_object.gameObject.GetComponent<AttributeDamageController>()?.data;
            if (mc.damage_type == null)
                mc.damage_type = AttributeDamageData.GetDefault();

            // move to damage mode if not already in damage mode, or instant damage.
            // instant damage applied only if entering the trigger (not on stay).

            if (!mc.isDamaged || (mc.damage_type.isDamageInstant && !is_stay))
            {
                mc.SetDamaged(mc.damage_type);
            }
        }

        public static void HandleRepelObject(PlayerMovementController mc, GameObject repel_object)
        {
            // get the objects repel attributes (or default)
            // the handle moving into the repel state.

            mc.repel_source = repel_object.gameObject;
            mc.repel_type = repel_object.gameObject.GetComponent<AttributeRepelController>()?.data;
            if (mc.repel_type == null)
                mc.repel_type = AttributeDamageData.GetDefault();

            if (mc.player_state != PlayerState.player_repel)
                mc.ChangePlayerState(PlayerState.player_repel);
        }
    }

    
}
