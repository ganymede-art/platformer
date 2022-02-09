using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class ActorDamageEffectController : MonoBehaviour
{

    const float FLASH_INTERVAL = 0.1F;
    const string EMISSION_MATERIAL_PROPERTY = "_Emission";

    bool isActive = false;
    float flashTimer = 0F;
    bool isFlashing = false;
    private Renderer actorRenderer;

    public GameObject actorRendererObject;

    // Start is called before the first frame update
    void Start()
    {
        if (actorRendererObject == null)
            this.enabled = false;

        actorRenderer = actorRendererObject.GetComponent<Renderer>();

        if (actorRenderer == null)
            this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if (!isActive)
            return;

        flashTimer += Time.deltaTime;
        if (flashTimer >= FLASH_INTERVAL)
        {
            flashTimer = 0F;
            isFlashing = !isFlashing;
            foreach(var material in actorRenderer.materials)
                material.SetColor(EMISSION_MATERIAL_PROPERTY, isFlashing ? Color.red : Color.black);
        }
    }

    public void SetDamageEffect()
    {
        isActive = true;
    }

    public void UnsetDamageEffect()
    {
        isActive = false;

        foreach (var material in actorRenderer.materials)
            material.SetColor(EMISSION_MATERIAL_PROPERTY, Color.black);
    }
}
