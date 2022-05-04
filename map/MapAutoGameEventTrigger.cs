using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapAutoGameEventTrigger : MonoBehaviour
{
    [Header("Event Attributes")]
    public GameObject gameEventTriggerObject;

    void Start()
    {
        if (gameEventTriggerObject == null)
        {
            Debug.LogError("Missing event trigger object.");
            return;
        }

        var triggerComponent = gameEventTriggerObject
            .GetComponent<GameEventTrigger>();

        if (triggerComponent == null)
        {
            Debug.LogError("Missing event trigger component.");
            return;
        }

        triggerComponent.StartGameEvent();
    }
}
