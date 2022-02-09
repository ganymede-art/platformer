using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class MobStaticMethods
{
    public static void UpdateInternalDirection(MobController mc, Vector3 newDirection, float rotationSpeed)
    {
        // update transform facing direction.
        var newRotationDelta = Vector3.RotateTowards(mc.directionTransform.forward, newDirection, rotationSpeed * Time.deltaTime, 0.0F);

        // Move our position a step closer to the target.
        mc.directionTransform.rotation = Quaternion.LookRotation(newRotationDelta);
    }

    public static void UpdateRendererDirection(MobController mc, Vector3 newDirection, float rotationSpeed)
    {
        // update transform facing direction.
        var newRotationDelta = Vector3.RotateTowards(mc.rendererObject.transform.forward, newDirection, rotationSpeed * Time.deltaTime, 0.0F);

        // Move our position a step closer to the target.
        mc.rendererObject.transform.rotation = Quaternion.LookRotation(newRotationDelta);
    }
}

