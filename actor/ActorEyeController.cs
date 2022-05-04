using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;

public class ActorEyeController : MonoBehaviour
{
    const float BLINK_INTERVAL = 0.075F;
    const float RANDOM_BLINK_INTERVAL_MIN = 1.0F;
    const float RANDOM_BLINK_INTERVAL_MAX = 3.0F;

    private ActorEyeMode eyeMode;

    private Renderer actorRenderer;
    private Material[] rendererMaterials;

    private float blinkInterval = 0.0F;
    private float blinkTimer = 0.0F;
    private int blinkIndex = 0;

    private bool isBlinkingStarted = false;
    private bool isBlinkingFinished = false;

    [FormerlySerializedAs("actor_renderer_object")]
    public GameObject actorRendererObject;
    [FormerlySerializedAs("eye_material_index")]
    public int eyeMaterialIndex = 0;

    [FormerlySerializedAs("blink_materials")]
    public Material[] blinkMaterials;
    public Material[] emoteMaterials;

    private void Start()
    {
        actorRenderer = actorRendererObject.GetComponent<Renderer>();
        rendererMaterials = actorRenderer.materials;
        eyeMode = ActorEyeMode.eyeDefault;
    }

    private void Update()
    {
        if (eyeMode == ActorEyeMode.eyeDefault)
        {
            blinkTimer += Time.deltaTime;

            if (!isBlinkingStarted && blinkTimer >= blinkInterval)
            {
                blinkTimer = 0.0F;
                blinkInterval = BLINK_INTERVAL;
                blinkIndex = 0;
                isBlinkingStarted = true;
                isBlinkingFinished = false;
            }

            if (isBlinkingStarted && blinkTimer >= blinkInterval)
            {
                blinkTimer = 0.0F;

                rendererMaterials[eyeMaterialIndex] = blinkMaterials[blinkIndex];
                actorRenderer.materials = rendererMaterials;

                if (blinkIndex == blinkMaterials.Length - 1)
                    isBlinkingFinished = true;

                blinkIndex += isBlinkingFinished ? -1 : 1;

                if (blinkIndex == -1)
                {
                    isBlinkingStarted = false;
                    isBlinkingFinished = false;
                    blinkIndex = 0;
                    blinkInterval = Random.Range(RANDOM_BLINK_INTERVAL_MIN, RANDOM_BLINK_INTERVAL_MAX);
                }

            }
        }
    }

    public void SetEmote(int emoteMaterialIndex)
    {
        eyeMode = ActorEyeMode.eyeEmote;

        rendererMaterials[eyeMaterialIndex] = emoteMaterials[emoteMaterialIndex];
        actorRenderer.materials = rendererMaterials;
    }

    public void UnsetEmote()
    {
        eyeMode = ActorEyeMode.eyeDefault;
        blinkTimer = 0.0F;
    }


}
