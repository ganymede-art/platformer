using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using System;

public class MapInstantEventTrigger : MonoBehaviour
{
    private GameMasterController master;
    private BoxCollider trigger;
    private bool is_triggered = false;

    public GameObject event_source;
    public bool is_one_shot = false;

    public bool is_game_cutscene = false;

    private void Start()
    {
        master = GameObject.FindObjectOfType<GameMasterController>();
        trigger = this.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(is_triggered && !is_one_shot)
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
                master.cutsceneController.StartCutscene(event_source, is_game_cutscene);
            }
        }
    }
}
