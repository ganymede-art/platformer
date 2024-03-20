using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class NpcStatics
{
    public static void UpdateRendererDirection(Npc c, Vector3 direction, float turningSpeedMult)
    {
        if (direction == Vector3.zero)
            return;

        direction.y = 0.0F;
        direction.Normalize();

        var facingDelta = Vector3.RotateTowards
            (c.gameObject.transform.forward, direction, turningSpeedMult * Time.deltaTime, 0.0f);
        c.gameObject.transform.rotation = Quaternion.LookRotation(facingDelta);
    }
}