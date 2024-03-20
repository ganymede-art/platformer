using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WireProp : MonoBehaviour
{
    const float ENTER_SAG_AMOUNT = 0.375F;
    const float MIDDLE_SAG_AMOUNT = 0.5F;
    const float EXIT_SAG_AMOUNT = 0.375F;

    private LineRenderer line;

    public Transform[] wireTransforms;
    public Material wireMaterial;
    public float wireThickness;
    public bool isDynamic;

    void Start()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.material = wireMaterial;

        var wireVectors = new List<Vector3>();

        for(int i = 0; i < wireTransforms.Length - 1; i++)
        {
            var startPoint = wireTransforms[i].position;
            var endPoint = wireTransforms[i+1].position;

            var enterSagPoint = Vector3.Lerp(startPoint, endPoint, 0.25F);
            var middleSagPoint = Vector3.Lerp(startPoint, endPoint, 0.5F);
            var exitSagPoint = Vector3.Lerp(startPoint, endPoint, 0.75F);

            enterSagPoint.y -= ENTER_SAG_AMOUNT;
            middleSagPoint.y -= MIDDLE_SAG_AMOUNT;
            exitSagPoint.y -= EXIT_SAG_AMOUNT;

            wireVectors.Add(startPoint);
            wireVectors.Add(enterSagPoint);
            wireVectors.Add(middleSagPoint);
            wireVectors.Add(exitSagPoint);
            wireVectors.Add(endPoint);
        }

        line.positionCount = wireVectors.Count;
        line.SetPositions(wireVectors.ToArray());
        line.startWidth = wireThickness;
        line.endWidth = wireThickness;
    }


    void Update()
    {
        if (!isDynamic)
            return;

        line.positionCount = wireTransforms.Length;
        line.SetPositions(wireTransforms.Select(x => x.position).ToArray());
    }
}
