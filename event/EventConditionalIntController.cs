using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using System;
using static Assets.script.EventConstants;
using UnityEngine.Serialization;

public class EventConditionalIntController : MonoBehaviour, IEventController
{
    private GameMasterController master;
    private IReplacerController replacerComponent;
    private object replacerRawValue;
    private int replacerValue;
    private bool isValidConditional = false;

    [Header("Event sources.")]
    public GameObject nextEventSource = null;
    public GameObject conditionalEventSource = null;
    [Header("Source of comparison value.")]
    public GameObject replacerObject;
    [Header("Type of comparison.")]
    public ConditionalIntType conditionalType;
    [Header("Value to compare against.")]
    public int conditionalValue = 0;

    public void FinishEvent()
    {
        return;
    }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_CONDITIONAL_INT;
    }

    public bool GetIsEventComplete()
    {
        return true;
    }

    public bool GetIsGameEventComplete()
    {
        return true;
    }

    public bool GetIsProcessComplete()
    {
        return true;
    }

    public GameObject GetNextEventSource()
    {
        if(!isValidConditional)
            return nextEventSource;

        if (conditionalType == ConditionalIntType.isEqual)
        {
            if (replacerValue == conditionalValue)
                return conditionalEventSource;
        }
        else if (conditionalType == ConditionalIntType.isNotEqual)
        {
            if (replacerValue != conditionalValue)
                return conditionalEventSource;
        }
        else if (conditionalType == ConditionalIntType.isGreaterThan)
        {
            if (replacerValue > conditionalValue)
                return conditionalEventSource;
        }
        else if (conditionalType == ConditionalIntType.isGreaterThanOrEqual)
        {
            if (replacerValue >= conditionalValue)
                return conditionalEventSource;
        }
        else if (conditionalType == ConditionalIntType.isLessThan)
        {
            if (replacerValue < conditionalValue)
                return conditionalEventSource;
        }
        else if (conditionalType == ConditionalIntType.isLessThanOrEqual)
        {
            if (replacerValue <= conditionalValue)
                return conditionalEventSource;
        }

        return nextEventSource;
    }

    public void ProcessEvent()
    {
        return;
    }

    public void StartEvent()
    {
        replacerComponent = replacerObject.GetComponent<IReplacerController>();

        if(replacerComponent == null)
        {
            Debug.LogError("Replacer component missing.");
            return;
        }

        replacerRawValue = replacerComponent.GetReplacementValue();

        if(replacerRawValue == null)
        {
            Debug.LogError("Replacer raw value null.");
            return;
        }

        if(replacerRawValue.GetType() != typeof(int))
        {
            Debug.LogError("Replacer raw value is not an int.");
            return;
        }

        replacerValue = Convert.ToInt32(replacerRawValue);

        isValidConditional = true;
    }
}
