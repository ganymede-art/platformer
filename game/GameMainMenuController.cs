using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMainMenuController : MonoBehaviour
{
    GameMasterController master;

    void Start()
    {
        master = GameObject.FindObjectOfType<GameMasterController>();
        master.input_controller.actionStart.performed += LoadTestScene;
        master.input_controller.actionSelect.performed += loadGame;
    }


    void OnDestroy()
    {
        master.input_controller.actionStart.performed -= LoadTestScene;
        master.input_controller.actionSelect.performed -= loadGame;
    }

    private void LoadTestScene(InputAction.CallbackContext context)
    {
        Debug.Log("Loading Test Scene");
        master.load_level_controller.StartLoadLevel("scene_test_1", "player_start_1", "camera_start_1");
    }

    private void loadGame(InputAction.CallbackContext context)
    {
        master.data_controller.LoadData();
    }
}
