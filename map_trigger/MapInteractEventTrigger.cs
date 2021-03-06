using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class MapInteractEventTrigger : MonoBehaviour
{
    private GameMasterController master;

    public GameObject event_source;
    private BoxCollider trigger;

    private bool is_triggered = false;

    void Start()
    {
        master = GameObject.FindObjectOfType<GameMasterController>();
        trigger = this.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (!is_triggered)
            return;

        if (master.gameState != GameState.Game)
            return;

        if (!master.inputController.wasInputInteract
            && master.inputController.isInputInteract)
            master.cutsceneController.StartCutscene(event_source,false);

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == GameConstants.TAG_PLAYER)
        {
            if (!is_triggered)
            {
                is_triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == GameConstants.TAG_PLAYER)
        {
            if (is_triggered)
            {
                is_triggered = false;
            }
        }
    }
}
