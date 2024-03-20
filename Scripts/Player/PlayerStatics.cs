using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public static class PlayerStatics
{
    public static Vector3 GetFlatDirectionForMovement(Player c)
    {
        var cameraRelativeDirection = Quaternion.Euler
            (0.0F, ActiveSceneHighLogic.G.CachedCamcorderObject.transform.eulerAngles.y, 0.0F)
            * InputHighLogic.G.Move3d;
        return cameraRelativeDirection;
    }

    public static Vector3 GetDirectionForMovement(Player c)
    {
        var cameraRelativeDirection = Quaternion.Euler
            (0.0F, ActiveSceneHighLogic.G.CachedCamcorderObject.transform.eulerAngles.y, 0.0F)
            * InputHighLogic.G.Move3d;

        var slopeRelativeDirection = (c.GroundCheck.IsCheckSphereGrounded)
            ? Vector3.ProjectOnPlane(cameraRelativeDirection, c.GroundCheck.SpherecastGroundNormal)
            : cameraRelativeDirection;

        slopeRelativeDirection.Normalize();

        return slopeRelativeDirection;
    }

    public static Vector3 GetForceForMovement(Player c, Vector3 direction)
    {
        float acceleration = (c.GroundCheck.IsCheckSphereGrounded)
            ? ACCELERATION_GROUNDED
            : ACCELERATION_AIR;
        var force = direction * acceleration * InputHighLogic.G.Move3d.magnitude;
        return force;
    }

    public static void FixedUpdateMovement(Player c, Vector3 direction, Vector3 force)
    {
        RaycastHit movementHit;
        bool isMovementHit = Physics.SphereCast
            ( c.transform.position
            , MOVEMENT_SPHERECAST_RADIUS
            , direction
            , out movementHit
            , MOVEMENT_SPHERECAST_DISTANCE
            , LAYER_MASK_PLAYER_IGNORES
            , QueryTriggerInteraction.Ignore);

        if (isMovementHit)
        {
            force = Vector3.ProjectOnPlane(force, movementHit.normal);

            float movementHitAngle = Vector3.Angle
                (movementHit.normal, Vector3.up);

            if (movementHitAngle > GROUND_CHECK_MAX_GROUNDED_ANGLE)
                force.y = 0.0F;

            c.playerRigidBody.AddForce(force, ForceMode.VelocityChange);
        }
        else
        {
            c.playerRigidBody.AddForce(force, ForceMode.VelocityChange);
        }
    }

    public static void UpdateInternalDirection(Player c, Vector3 direction)
    {
        direction.y = 0.0F;
        direction.Normalize();

        c.playerDirectionObject.transform.rotation = Quaternion.LookRotation(direction);
    }

    public static void UpdateRendererDirection(Player c, Vector3 direction)
    {
        direction.y = 0.0F;
        direction.Normalize();

        var facingDelta = Vector3.RotateTowards
            (c.playerRendererObject.transform.forward, direction, ANIMATION_TURNING_SPEED_MULT * Time.deltaTime, 0.0f);
        c.playerRendererObject.transform.rotation = Quaternion.LookRotation(facingDelta);
    }

    public static void FixedUpdateDynamicFriction(Player c, float lowFriction, float highFriction)
    {
        if(c.GroundCheck.IsCheckSphereGrounded
            && !InputHighLogic.G.IsMove3dPressed)
        {
            c.playerCollider.material.staticFriction = highFriction;
            c.playerCollider.material.dynamicFriction = highFriction;
        }
        else
        {
            c.playerCollider.material.staticFriction = lowFriction;
            c.playerCollider.material.dynamicFriction = lowFriction;
        }
    }

    public static void FixedUpdateLimitVelocityTwoAxis(Player c, float velocityLimit)
    {
        Vector3 xzVector = new Vector3(c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
        Vector3 yVector = new Vector3(0, c.playerRigidBody.velocity.y, 0);

        if (xzVector.magnitude > velocityLimit)
        {

            xzVector = Vector3.ClampMagnitude(xzVector, velocityLimit);
            c.playerRigidBody.velocity = xzVector + yVector;
        }
    }

    public static void FixedUpdateLimitVelocityThreeAxis
        (Player c
        , float horizontalVelocityLimit
        , float sinkVelocityLimit
        , float riseVelocityLimit)
    {
        Vector3 xzVector = new Vector3(c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
        Vector3 yVector = new Vector3(0, c.playerRigidBody.velocity.y, 0);

        if (xzVector.magnitude > horizontalVelocityLimit)
            xzVector = Vector3.ClampMagnitude(xzVector, horizontalVelocityLimit);
        yVector.y = Mathf.Clamp(yVector.y, sinkVelocityLimit, riseVelocityLimit);
        c.playerRigidBody.velocity = xzVector + yVector;
    }

    public static void UpdateBuoyancy(Player c)
    {
        if (c.playerRigidBody.position.y > c.Water.WaterHeight - WD_SURFACE_CLAMP_HEIGHT_OFFSET
            && !c.GroundCheck.IsCheckSphereHit)
        {
            var clampPosition = c.playerRigidBody.position;
            clampPosition.y = c.Water.WaterHeight - WD_SURFACE_CLAMP_HEIGHT_OFFSET;
            c.playerRigidBody.MovePosition(clampPosition);
        }
    }

    public static void FixedUpdateWeakBuoyancy(Player c)
    {
        if (c.playerRigidBody.position.y < c.Water.WaterHeight)
        {
            c.playerRigidBody.AddForce(Vector3.up * WD_WEAK_BUOYANT_FORCE, ForceMode.Acceleration);
        }
    }

    public static void FixedUpdateBuoyancy(Player c)
    {
        if (c.playerRigidBody.position.y < c.Water.WaterHeight)
        {
            bool doUseWeakForce = (c.Water.DidBeginBehaviourFullSubmerged
                && !c.Water.DidEmergeSinceBehaviourBegan
                && PlayerHighLogic.G.CanDiveUnderwater
                && !InputHighLogic.G.IsSouthPressed);

            if (doUseWeakForce)
            {
                c.playerRigidBody.AddForce(Vector3.up * WD_WEAK_BUOYANT_FORCE, ForceMode.Acceleration);
            }
            else
            {
                float distToWaterSurf = Mathf.Abs(c.playerRigidBody.position.y - c.Water.WaterHeight);
                distToWaterSurf = Mathf.Clamp(distToWaterSurf, 0.0F, 1.0F);
                float buoyantForceMult = 0.0F;

                if (!c.Water.IsFullSubmerged)
                    buoyantForceMult = Mathf.SmoothStep
                        (WD_MIN_STRONG_BUOYANT_FORCE
                        , WD_MAX_STRONG_BUOYANT_FORCE
                        , distToWaterSurf);
                else
                    buoyantForceMult = Mathf.Lerp
                        (WD_MIN_SUBMERGED_BUOYANT_FORCE
                        , WD_MAX_STRONG_BUOYANT_FORCE
                        , distToWaterSurf);

                c.playerRigidBody.AddForce(Vector3.up * buoyantForceMult, ForceMode.Acceleration);
            }
        }
    }

    public static void SimpleRepel(Player c, GameObject repelObject, float repelForceMult)
    {
        c.playerRigidBody.velocity = Vector3.zero;
        var repelVector = (c.transform.position - repelObject.transform.position).normalized;
        c.playerRigidBody.AddForce(repelVector * repelForceMult, ForceMode.VelocityChange);
    }
}
