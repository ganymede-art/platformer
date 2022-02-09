using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;
using static Assets.script.GameConstants;
using static Assets.script.PlayerConstants;

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
            Debug.DrawRay(mc.rigidBody.position, direction);
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
                    Debug.Log("stepping!");
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

        public static void UpdateInternalDirection(PlayerController pc)
        {
            // update player facing direction.
            pc.facingDirection = Quaternion.Euler(0, pc.cameraObject.transform.rotation.eulerAngles.y, 0) * pc.inputDirectional;
            pc.facingDirectionDelta = Vector3.RotateTowards(pc.rendererObject.transform.forward, pc.facingDirection, PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            pc.directionObject.transform.rotation = Quaternion.LookRotation(pc.facingDirectionDelta);
        }

        public static void UpdateRendererDirection(PlayerController pc)
        {
            // update player facing direction.
            pc.facingDirection = Quaternion.Euler(0, pc.cameraObject.transform.rotation.eulerAngles.y, 0) * pc.inputDirectional;
            pc.facingDirectionDelta = Vector3.RotateTowards(pc.rendererObject.transform.forward, pc.facingDirection, ANIMATION_TURNING_SPEED_MULTIPLIER, 0.0f);

            // Move our position a step closer to the target.
            pc.rendererObject.transform.rotation = Quaternion.LookRotation(pc.facingDirectionDelta);
        }

        public static void ApplyStaticFriction(PlayerController pc, float drag, float friction, PhysicMaterialCombine physicsMaterialCombine)
        {
            pc.rigidBody.drag = drag;
            pc.rbCollider.material.dynamicFriction = friction;
            pc.rbCollider.material.staticFriction = friction;

            pc.rbCollider.material.frictionCombine = physicsMaterialCombine;
        }

        public static void ApplyDynamicFriction(PlayerController pc)
        {
            // update based on circumstances.

            pc.rigidBody.drag = pc.isRaycastGrounded ? DRAG_GROUNDED : DRAG_AIR;

            // change physics combine mode.

            pc.rbCollider.material.frictionCombine = PhysicMaterialCombine.Average;

            //

            if (pc.isSpherecastGrounded && pc.isRaycastGrounded && !pc.isInputDirectional)
            {
                pc.rbCollider.material.dynamicFriction = 1;
                pc.rbCollider.material.staticFriction = 1;
            }
            else
            {
                pc.rbCollider.material.dynamicFriction = 0F;
                pc.rbCollider.material.staticFriction = 0F;
            }
        }

        public static void LimitSpeedThreeAxis(PlayerController pc, float speedLimit)
        {
            if (pc.rigidBody.velocity.magnitude > speedLimit)
            {
                pc.rigidBody.velocity = Vector3.ClampMagnitude(pc.rigidBody.velocity, speedLimit);
            }
        }

        public static void LimitSpeedTwoAxis(PlayerController pc, float speedLimit)
        {
            // limit speed while in the default state.

            Vector3 old_x_z = new Vector3(pc.rigidBody.velocity.x, 0, pc.rigidBody.velocity.z);
            Vector3 old_y = new Vector3(0, pc.rigidBody.velocity.y, 0);

            if (old_x_z.magnitude > speedLimit)
            {

                old_x_z = Vector3.ClampMagnitude(old_x_z, speedLimit);
                pc.rigidBody.velocity = old_x_z + old_y;
            }
        }

        public static void FixedUpdateSlide(PlayerController pc)
        {
            // move towards the sliding state
            // if the right criteria are met.

            if (pc.raycastGroundedSlopeAngle > SLIDE_ANGLE_MIN
                || pc.groundData.isGroundSlide)
            {
                // reduce the slide resistance
                pc.slideResistance -= (pc.raycastGroundedSlopeAngle * SLIDE_RESISTANCE_GROUND_ANGLE_MULTIPLIER);
            }
            else
            {
                pc.slideResistance = SLIDE_RESISTANCE_MAX;
            }

            pc.slideResistance = Mathf.Clamp(pc.slideResistance, 0.0f, SLIDE_RESISTANCE_MAX);

            if (pc.slideResistance <= 0.0f && pc.isSpherecastGrounded)
            {
                pc.ChangePlayerState(GameConstants.PLAYER_STATE_SLIDE);
                return;
            }
        }
    } 
}
