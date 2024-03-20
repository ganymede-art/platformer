using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class TransformsEditor
{
    [MenuItem("Tools/2 Set Selected Transform Random Rotation and Scale")]
    public static void SetPropRandomRotationAndScale()
    {
        float scaleMin = 0.5F;
        float scaleMax = 1.5F;

        float rotationMin = 0.0F;
        float rotationMax = 360.0F;

        var selectedObjects = Selection.gameObjects;

        foreach (var selectedObject in selectedObjects)
        {
            float rotationAngle = UnityEngine.Random.Range(rotationMin, rotationMax);

            float newScaleFactor = UnityEngine.Random.Range(scaleMin, scaleMax);
            var newScaleVector = new Vector3(newScaleFactor, newScaleFactor, newScaleFactor);

            selectedObject.transform.Rotate(Vector3.up, rotationAngle);
            selectedObject.transform.localScale = newScaleVector;
        }
    }

    [MenuItem("Tools/2 Set Selected Random Scale")]
    public static void SetSelectedRandomScale()
    {
        float scaleMin = 0.5F;
        float scaleMax = 1.5F;

        var selectedObjects = Selection.gameObjects;

        foreach (var selectedObject in selectedObjects)
        {
            float newScaleFactor = UnityEngine.Random.Range(scaleMin, scaleMax);
            var newScaleVector = new Vector3(newScaleFactor, newScaleFactor, newScaleFactor);
            selectedObject.transform.localScale = newScaleVector;
        }
    }

    [MenuItem("Tools/2 Set Selected Transform Grounded")]
    public static void SetPropGrounded()
    {
        var selectedObjects = Selection.gameObjects;

        foreach (var selectedObject in selectedObjects)
        {
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(selectedObject.transform.position, Vector3.down, out hitInfo);

            if (!isHit)
                continue;

            selectedObject.transform.position = hitInfo.point;
        }
    }
}