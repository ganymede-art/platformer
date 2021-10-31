using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using System;
using UnityEngine.Serialization;

public class MapInstantEventTrigger : MonoBehaviour
{
    private GameMasterController master;
    private BoxCollider trigger;
    private bool is_triggered = false;

    [FormerlySerializedAs("event_source")]
    public GameObject eventSource;
    [FormerlySerializedAs("is_one_shot")]
    public bool isOneShot = false;
    public GameState gameState = GameState.Cutscene;
    public bool isOrdered;
    public bool isPriority;

    private void Start()
    {
        master = GameObject.FindObjectOfType<GameMasterController>();
        trigger = this.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(is_triggered && !isOneShot)
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
        
        if (other.tag == GameConstants.TAG_PLAYER)
        {
            if (!is_triggered)
            {
                is_triggered = true;

                var gameEvent = new GameEvent(gameState, eventSource);

                if (isOrdered)
                {
                    if (isPriority)
                        master.cutsceneController.InsertOrderedGameEvent(gameEvent);
                    else
                        master.cutsceneController.AddOrderedGameEvent(gameEvent);
                }
                else
                {
                    master.cutsceneController.AddGeneralGameEvent(gameEvent);
                }
            }
        }
    }
}
