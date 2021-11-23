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
        master.inputController.buttonStart.performed += LoadTestScene;
        master.inputController.buttonSelect.performed += loadGame;
    }


    void OnDestroy()
    {
        master.inputController.buttonStart.performed -= LoadTestScene;
        master.inputController.buttonSelect.performed -= loadGame;
    }

    private void LoadTestScene(InputAction.CallbackContext context)
    {
        Debug.Log("Loading Test Scene");
        master.loadLevelController.StartLoadLevel("scene_test_1", "player_start_1", "camera_start_1");
    }

    private void loadGame(InputAction.CallbackContext context)
    {
        master.dataController.LoadData();
    }
}
