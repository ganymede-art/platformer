using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteractEventRangeTrigger : MonoBehaviour
{
    private GameMasterController master;
    private GameObject player_object;
    private PlayerController player_controller;

    public float interact_range = 1.0f;

    private bool was_in_range = false;
    private bool is_in_range = false;

    public GameObject event_source;

    void Start()
    {
        master = GameMasterController.GlobalMasterController;
        player_object = GameMasterController.GlobalPlayerObject;
        player_controller = player_object.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (master.gameState != GameState.Game)
            return;

        was_in_range = is_in_range;

        is_in_range = Vector3.Distance(this.transform.position, player_object.transform.position) <= interact_range;

        if (!is_in_range)
            return;

        if (player_controller.isSpherecastGrounded
            && !master.inputController.wasInputInteract
            && master.inputController.isInputInteract)
        {
            master.cutsceneController.StartCutscene(event_source,false);
        }
    }
}
