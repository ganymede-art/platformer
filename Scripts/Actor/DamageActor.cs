using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActor : MonoBehaviour
{
    // Consts.
    private enum DamageEffectStatus
    {
        Disabled,
        EnabledVisible,
        EnabledInvisible,
    }

    private const float DAMAGE_INTERVAL = 0.1F;

    // Private fields.
    private DamageEffectStatus status;
    private float damageTimer;

    // Public fields.
    public Renderer actorRenderer;

    private void Awake()
    {
        status = DamageEffectStatus.Disabled;
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if (status == DamageEffectStatus.Disabled)
            return;

        damageTimer += Time.deltaTime;

        if (damageTimer < DAMAGE_INTERVAL)
            return;

        damageTimer = 0.0F;

        if (status == DamageEffectStatus.EnabledVisible)
            status = DamageEffectStatus.EnabledInvisible;
        else if (status == DamageEffectStatus.EnabledInvisible)
            status = DamageEffectStatus.EnabledVisible;

        actorRenderer.enabled = status == DamageEffectStatus.EnabledVisible;
    }

    public void BeginDamage()
    {
        status = DamageEffectStatus.EnabledVisible;
    }

    public void EndDamage()
    {
        status = DamageEffectStatus.Disabled;
        actorRenderer.enabled = true;
    }
}
