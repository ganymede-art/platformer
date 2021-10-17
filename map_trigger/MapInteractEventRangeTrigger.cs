using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteractEventRangeTrigger : MonoBehaviour
{
    private GameMasterController master;
    private GameObject player_object;
    private PlayerMovementController player_controller;

    public float interact_range = 1.0f;

    private bool was_in_range = false;
    private bool is_in_range = false;

    public GameObject event_source;

    void Start()
    {
        master = GameMasterController.GetMasterController();
        player_object = GameMasterController.GetPlayerObject();
        player_controller = player_object.GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        if (master.gameState != GameState.Game)
            return;

        was_in_range = is_in_range;

        is_in_range = Vector3.Distance(this.transform.position, player_object.transform.position) <= interact_range;

        if (!is_in_range)
            return;

        if (player_controller.is_spherecast_grounded
            && !master.input_controller.wasInputInteract
            && master.input_controller.isInputInteract)
        {
            master.cutscene_controller.StartCutscene(event_source,false);
        }
    }
}
