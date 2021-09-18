using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class UserInterfaceGameController : MonoBehaviour
{
    // constants.

    const float DESC_APPEAR_TIME = 1f;
    const float DESC_DISAPPEAR_TIME = 5f;
    const float DESC_FINISH_TIME = 6f;

    GameMasterController master;

    // root.

    GameObject ui_object;
    private GameObject ui_health_container_object;
    private GameObject ui_scene_description_object;
    private TextMeshProUGUI ui_scene_description_text;
    private GameObject[] ui_health_objects;

    public GameObject ui_prefab;

    // scene description variables.

    private bool is_display_desc;
    private float display_desc_timer;
    private Color display_desc_colour;

    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // add event hooks.

        master.GameStateChange += ChangeGameState;

        // scene description variables.

        is_display_desc = false;
        display_desc_timer = 0.0f;
        display_desc_colour = new Color(1, 1, 1, 0);

        // initialise ui.

        ui_object = Instantiate(ui_prefab, this.transform);
        ui_object.layer = 5;
        DontDestroyOnLoad(ui_object);

        // container.

        ui_health_container_object = ui_object.transform.Find("ui_health_container").gameObject;

        // health objects.

        ui_health_objects = new GameObject[9];

        for (int i = 0; i < ui_health_objects.Length; i++)
        {
            ui_health_objects[i]
                = ui_health_container_object.transform.Find("ui_health_" + i).gameObject;
        }

        // scene description.

        ui_scene_description_object = ui_object.transform.Find("ui_scene_description").gameObject;
        ui_scene_description_text = ui_scene_description_object.GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        master.GameStateChange -= ChangeGameState;
    }

    void Update()
    {
        if (master.game_state != GameState.Game
            && master.game_state != GameState.GameCutscene)
            return;

        for (int i = 0; i < ui_health_objects.Length; i++)
        {
            ui_health_objects[i].SetActive(master.player_controller.player_health > i);
        }

        if (is_display_desc)
        {
            display_desc_timer += Time.deltaTime;

            if (display_desc_timer >= DESC_FINISH_TIME)
            {
                ui_scene_description_object.SetActive(false);
                is_display_desc = false;
            }

            if(display_desc_colour.a <= 1 
                && display_desc_timer <= DESC_APPEAR_TIME)
            {
                display_desc_colour.a = Mathf.InverseLerp(0.0f, DESC_APPEAR_TIME, display_desc_timer);
            }

            if(display_desc_colour.a >= 0 
                && display_desc_timer >= DESC_DISAPPEAR_TIME)
            {
                display_desc_colour.a -= 0.01f;
            }

            ui_scene_description_text.color = display_desc_colour;
        }
        else
        {
            ui_scene_description_object.SetActive(false);
        }
    }

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        ui_object.SetActive(args.game_state == GameState.Game
            || args.game_state == GameState.GameCutscene);
    }



    public void StartSceneDataDisplay()
    {
        ui_scene_description_object.SetActive(true);
        ui_scene_description_text.text = master.game_scene_data.scene_description;

        is_display_desc = true;

        display_desc_colour.a = 0.0f;
        display_desc_timer = 0.0f;
    }
}
