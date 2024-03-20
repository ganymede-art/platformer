using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteActor : MonoBehaviour
{
    // Consts.
    private enum BlinkStatus
    {
        Open,
        Closing,
        Closed,
        Opening,
    }
    private const float MIN_BLINK_INTERVAL = 1.0F;
    private const float MAX_BLINK_INTERVAL = 3.0F;
    private const float BLINK_INTERVAL = 0.05F;

    // Private fields.
    private EmoteType activeEmoteType;

    private float blinkTimer;
    private float blinkInterval;
    private BlinkStatus blinkStatus;

    private Material[] actorRendererMaterials;

    // Public fields.
    public Renderer actorRenderer;
    public int emoteMaterialIndex;

    public Material DefaultEmoteMaterial;
    public Material blinkingEmoteMaterial;
    public Material sleepingEmoteMaterial;
    public Material happyEmoteMaterial;
    public Material sadEmoteMaterial;
    public Material calmEmoteMaterial;
    public Material angryEmoteMaterial;
    public Material shockedEmoteMaterial;
    public Material deadEmoteMaterial;

    private void Start()
    {
        activeEmoteType = EmoteType.Default;

        blinkTimer = 0.0F;
        blinkInterval = Random.Range(MIN_BLINK_INTERVAL, MAX_BLINK_INTERVAL);
        blinkStatus = BlinkStatus.Open;

        actorRendererMaterials = actorRenderer.materials;
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        if (activeEmoteType != EmoteType.Default)
            return;

        if(blinkTimer > blinkInterval)
        {
            blinkTimer = 0.0F;
            Blink();
        }

        blinkTimer += Time.deltaTime;
    }

    private void Blink()
    {
        if(blinkStatus == BlinkStatus.Open)
        {
            blinkStatus = BlinkStatus.Closing;
            actorRendererMaterials[emoteMaterialIndex] = blinkingEmoteMaterial;
            actorRenderer.materials = actorRendererMaterials;
            blinkInterval = BLINK_INTERVAL;
        }
        else if(blinkStatus == BlinkStatus.Closing)
        {
            blinkStatus = BlinkStatus.Closed;
            actorRendererMaterials[emoteMaterialIndex] = sleepingEmoteMaterial;
            actorRenderer.materials = actorRendererMaterials;
            blinkInterval = BLINK_INTERVAL;
        }
        else if(blinkStatus == BlinkStatus.Closed)
        {
            blinkStatus = BlinkStatus.Opening;
            actorRendererMaterials[emoteMaterialIndex] = blinkingEmoteMaterial;
            actorRenderer.materials = actorRendererMaterials;
            blinkInterval = BLINK_INTERVAL;
        }
        else if(blinkStatus == BlinkStatus.Opening)
        {
            blinkStatus = BlinkStatus.Open;
            actorRendererMaterials[emoteMaterialIndex] = DefaultEmoteMaterial;
            actorRenderer.materials = actorRendererMaterials;
            blinkInterval = Random.Range(MIN_BLINK_INTERVAL, MAX_BLINK_INTERVAL);
        }
    }

    public void BeginEmote(EmoteType emoteType)
    {
        activeEmoteType = emoteType;
        actorRendererMaterials[emoteMaterialIndex] = GetEmoteMaterial(emoteType);
        actorRenderer.materials = actorRendererMaterials;
    }

    public void EndEmote()
    {
        activeEmoteType = EmoteType.Default;
        actorRendererMaterials[emoteMaterialIndex] = DefaultEmoteMaterial;
        actorRenderer.materials = actorRendererMaterials;
    }

    public Material GetEmoteMaterial(EmoteType emoteType)
    {
        return
            (emoteType == EmoteType.Blinking) ? blinkingEmoteMaterial :
            (emoteType == EmoteType.Sleeping) ? sleepingEmoteMaterial :
            (emoteType == EmoteType.Happy) ? happyEmoteMaterial :
            (emoteType == EmoteType.Sad) ? sadEmoteMaterial :
            (emoteType == EmoteType.Calm) ? calmEmoteMaterial :
            (emoteType == EmoteType.Angry) ? angryEmoteMaterial :
            (emoteType == EmoteType.Shocked) ? shockedEmoteMaterial :
            (emoteType == EmoteType.Dead) ? deadEmoteMaterial :
            DefaultEmoteMaterial;
    }
}
