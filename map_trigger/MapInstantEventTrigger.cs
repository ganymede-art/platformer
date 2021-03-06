using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using System;
using UnityEngine.Serialization;
using static Assets.Script.GameConstants;

public class MapInstantEventTrigger : MonoBehaviour
{
    private GameMasterController master;
    private BoxCollider trigger;
    private bool is_triggered = false;

    [Header("Event Attributes")]
    public GameObject gameEventTriggerObject;

    private void Start()
    {
        master = GameObject.FindObjectOfType<GameMasterController>();
        trigger = this.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(is_triggered)
        {
            // unset trigger if player leaves trigger bounds.

            if (!Physics.CheckBox(gameObject.transform.TransformPoint(trigger.center),
                trigger.bounds.size,
                this.transform.rotation,
                GameConstants.MASK_ONLY_PLAYER))
                is_triggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.name == NAME_PLAYER_COLLIDER)
        {
            if (!is_triggered)
            {
                is_triggered = true;

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
    }
}
