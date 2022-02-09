using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.script.AttributeConstants;

public class AttributeRepelController : MonoBehaviour
{
    public AttributeDamageData data;

    private void Start()
    {
        if (data.damageAmount < DAMAGE_AMOUNT_MIN)
            data.damageAmount = DAMAGE_AMOUNT_MIN;

        if (data.horizontalForceMultiplier < DAMAGE_HORIZONTAL_MULTIPLIER_MIN)
            data.horizontalForceMultiplier = DAMAGE_HORIZONTAL_MULTIPLIER_MIN;

        if (data.verticalForceMultiplier < DAMAGE_VERTICAL_MULTIPLIER_MIN)
            data.verticalForceMultiplier = DAMAGE_VERTICAL_MULTIPLIER_MIN;
    }
}
