using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;

using static Assets.script.AttributeDataClasses;
using static Assets.script.GameConstants;

namespace Assets.script
{
    public class PlayerStaticMethods
    {
        public static void Movement(PlayerController mc, Vector3 direction, Vector3 force)
        {
            // do raycasts.

            mc.isMovementHit = Physics.SphereCast
                (mc.transform.position, PlayerConstants.MOVEMENT_SPHERECAST_RADIUS, direction, 
                out mc.movementHit, PlayerConstants.MOVEMENT_SPHERECAST_DISTANCE,MASK_PLAYER_IGNORES);

            // apply forces based on raycast hits.

            if (!mc.isMovementHit)
            {
                // no obstace directly ahead.

                mc.rigidBody.AddForce(force, ForceMode.VelocityChange);
            }
        }

        public static void StepMovement(PlayerController mc, Vector3 direction, Vector3 force)
        {
            // do raycasts.

            mc.isMovementHit = Physics.SphereCast
                (mc.transform.position, PlayerConstants.MOVEMENT_SPHERECAST_RADIUS, direction, 
                out mc.movementHit, PlayerConstants.MOVEMENT_SPHERECAST_DISTANCE, MASK_PLAYER_IGNORES);

            // apply forces based on raycast hits.

            if (mc.isMovementHit)
            {
                // initial step cast.
                mc.isStepMovementHit = Physics.SphereCast
                    (mc.transform.position + PlayerConstants.STEP_MOVEMENT_OFFSET, PlayerConstants.MOVEMENT_SPHERECAST_RADIUS, direction, out mc.stepMovementHit, PlayerConstants.MOVEMENT_SPHERECAST_DISTANCE, MASK_PLAYER_IGNORES);

                // addition step check for ceilings.
                if (Physics.CheckSphere(mc.transform.position + PlayerConstants.STEP_MOVEMENT_OFFSET, PlayerConstants.MOVEMENT_SPHERECAST_RADIUS, MASK_PLAYER_IGNORES))
                    mc.isStepMovementHit = true;

                if (!mc.isStepMovementHit)
                {
                    // step obstace, move up and move directly ahead.
                    if (mc.rigidBody.velocity.y < PlayerConstants.STEP_MAX_VELOCITY)
                        mc.rigidBody.AddForce(Vector3.up, ForceMode.VelocityChange);

                    mc.rigidBody.AddForce(force, ForceMode.VelocityChange);

                    // force the sphere grounded status while moving up short steps.
                    mc.isSpherecastGrounded = true;
                }
                else
                {
                    // full obstacle, move on a plane to the collided surface.

                    force = Vector3.ProjectOnPlane(force, mc.movementHit.normal);
                    mc.rigidBody.AddForce(force, ForceMode.VelocityChange);
                }
            }
            else
            {
                // no obstace directly ahead.

                mc.rigidBody.AddForce(force, ForceMode.VelocityChange);
            }
        }

        public static void HandleDamageObject(PlayerController mc, GameObject damage_object, bool is_stay)
        {
            // get the objects damage attributes (or default)
            // the handle moving into the damage state.

            mc.damageSourceObject = damage_object.gameObject;
            mc.damageData = damage_object.gameObject.GetComponent<AttributeDamageController>()?.data;
            if (mc.damageData == null)
                mc.damageData = AttributeDamageData.GetDefault();

            // move to damage mode if not already in damage mode, or instant damage.
            // instant damage applied only if entering the trigger (not on stay).

            if (!mc.isDamaged || (mc.damageData.isDamageInstant && !is_stay))
            {
                mc.SetDamaged(mc.damageData);
            }
        }

        public static void HandleRepelObject(PlayerController mc, GameObject repel_object)
        {
            // get the objects repel attributes (or default)
            // the handle moving into the repel state.

            mc.repelSourceObject = repel_object.gameObject;
            mc.repelData = repel_object.gameObject.GetComponent<AttributeRepelController>()?.data;
            if (mc.repelData == null)
                mc.repelData = AttributeDamageData.GetDefault();

            if (mc.currentStateType != PlayerStateType.playerRepel)
                mc.ChangePlayerState(PlayerStateType.playerRepel);
        }

        public static void FaceDirection(PlayerController mc)
        {
            // update player facing direction.

            mc.facingDirection = Quaternion.Euler(0, mc.cameraObject.transform.rotation.eulerAngles.y, 0) * mc.inputDirectional;
            mc.facingDirectionDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, mc.facingDirection, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            mc.rendererObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
            mc.directionObject.transform.rotation = Quaternion.LookRotation(mc.facingDirectionDelta);
        }
    }

    
}
