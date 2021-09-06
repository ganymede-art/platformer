using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteractEventRangeTrigger : MonoBehaviour
{
    private GameMasterController master;
    private GameObject player_object;
    private PlayerMovementController player_controller;
    private ActorFaceDirectionController facing_controller;
    public float facing_range = 1.0f;

    private bool was_in_range = false;
    private bool is_in_range = false;

    public GameObject event_source;

    void Start()
    {
        master = GameMasterController.GetMasterController();
        player_object = GameMasterController.GetPlayerObject();
        player_controller = player_object.GetComponent<PlayerMovementController>();
        facing_controller = GetComponent<ActorFaceDirectionController>();
    }

    void Update()
    {
        if (master.game_state != GameState.Game)
            return;

        was_in_range = is_in_range;

        is_in_range = Vector3.Distance(this.transform.position, player_object.transform.position) <= facing_range;

        if (!is_in_range)
            return;

        if (player_controller.is_spherecast_grounded
            && !master.input_controller.Was_Input_Interact
            && master.input_controller.Is_Input_Interact)
        {
            master.cutscene_controller.StartCutscene(event_source,false);
        }
    }
}
