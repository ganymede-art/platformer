using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public static class MobStatics
{
    public static void UpdateInternalDirection(Mob c, Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        direction.y = 0.0F;
        direction.Normalize();

        c.mobDirectionObject.transform.rotation = Quaternion.LookRotation(direction);
    }

    public static void UpdateRendererDirection(Mob c, Vector3 direction, float turningSpeedMult = ANIMATION_TURNING_SPEED_MULT_FAST)
    {
        if (direction == Vector3.zero)
            return;

        direction.y = 0.0F;
        direction.Normalize();

        var facingDelta = Vector3.RotateTowards
            (c.mobRendererObject.transform.forward, direction, turningSpeedMult * Time.deltaTime, 0.0f);
        c.mobRendererObject.transform.rotation = Quaternion.LookRotation(facingDelta);
    }

    public static bool IsMobInState(Mob c, MobStateIdConstant[] validStates)
    {
        bool isInState = false;
        for (int i = 0; i < validStates.Length; i++)
        {
            if (c.ActiveState == validStates[i].mobStateId)
            {
                isInState = true;
                break;
            }
        }
        return isInState;
    }

    public static T GetMobBehaviour<T>(Mob c, MobBehaviourId behaviourId)
    {
        return (T)c.Behaviours.GetValueOrDefault(behaviourId);
    }
}
