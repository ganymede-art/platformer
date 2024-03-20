using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWaterScroll : MonoBehaviour
{
    // Private fields.
    private Vector2 firstOffset;
    private Vector2 secondOffset;

    // Public fields.
    public Renderer waterRenderer;
    public float xFirstLayerSpeedMult;
    public float yFirstLayerSpeedMult;
    public float xSecondLayerSpeedMult;
    public float ySecondLayerSpeedmult;

    private void Start()
    {
        firstOffset = Vector2.zero;
        secondOffset = Vector2.zero;
    }

    void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        firstOffset.x += Time.deltaTime * xFirstLayerSpeedMult;
        firstOffset.y += Time.deltaTime * yFirstLayerSpeedMult;

        secondOffset.x += Time.deltaTime * xSecondLayerSpeedMult;
        secondOffset.y += Time.deltaTime * ySecondLayerSpeedmult;

        waterRenderer.material.SetTextureOffset("_FirstTex", firstOffset);
        waterRenderer.material.SetTextureOffset("_SecondTex", secondOffset);
        if(waterRenderer.material.shader.name == "Project/DistortionWater")
            waterRenderer.material.SetTextureOffset("_DistortionTex", firstOffset);
    }
}
