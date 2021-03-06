using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.script;
using System;
public class GameLoadLevelController : MonoBehaviour
{
    private static GameLoadLevelController global;
    public static GameLoadLevelController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.loadLevelController;
            }
            return global;
        }
    }

    // load constants.

    const float LOAD_TIMER_MULTIPLIER = 1;

    // load variables.

    private float load_timer = 0.0f;
    private bool isLoading = false;

    [NonSerialized] public string loadSceneName = string.Empty;
    [NonSerialized] public string loadPlayerStartTransformName = string.Empty;
    [NonSerialized] public string loadCameraStartTransformName = string.Empty;

    // transition variables.

    Rect transitionRectangle = new Rect(0, 0, 9999, 9999);
    Color transitionColour = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    Texture2D transitionTexture;

    private void Start()
    {
        SceneManager.sceneLoaded += EndLoadLevel;

        // initialise transition variables.

        transitionTexture  = new Texture2D(1, 1);
    }

    private void Update()
    {
        if(isLoading)
        {
            // increment the load timer while loading.

            load_timer += LOAD_TIMER_MULTIPLIER * Time.deltaTime;

            if(load_timer >= 1.0f)
            {
                DoLoadLevel();
            }
        }
        else
        {
            // decrement the load timer if no longer loading.

            if(load_timer > 0.0f)
            {
                load_timer -= LOAD_TIMER_MULTIPLIER * Time.deltaTime;
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= EndLoadLevel;
    }

    public void StartLoadLevel(string scene_name, string player_start_transform_name, string camera_start_transform_name, UserInterfaceTransitionData transitionData = null)
    {
        // Begin loading a level.

        GameMasterController.Global.ChangeState(GameState.Loading);
       
        isLoading = true;

        load_timer = 0.0f;

        loadSceneName = scene_name;
        loadPlayerStartTransformName = player_start_transform_name;
        loadCameraStartTransformName = camera_start_transform_name;

        // set the transition ui.

        GameUserInterfaceController.Global.uiControllerTransition.SetMenu(transitionData);
    }

    private void DoLoadLevel()
    {
        SceneManager.LoadScene(loadSceneName, LoadSceneMode.Single);
    }

    private void EndLoadLevel(Scene scene, LoadSceneMode load_scene_mode)
    {
        // get the player and camera start transforms.

        var player_start_transform = GameObject.Find
            (loadPlayerStartTransformName).transform;

        var camera_start_transform = GameObject.Find
            (loadCameraStartTransformName).transform;

        // initialise the player and camera.

        var player_prefab = GameMasterController.Global.playerPrefab;
        var camera_prefab = GameMasterController.Global.cameraPrefab;

        var player = Instantiate(player_prefab, 
            player_start_transform.position, 
            player_start_transform.rotation);
        var camera = Instantiate(camera_prefab, 
            camera_start_transform.position, 
            camera_start_transform.rotation);

        // name the prefabs.

        player.name = GameConstants.NAME_PLAYER;
        camera.name = GameConstants.NAME_PLAYER_CAMERA;

        // reset after loading.

        GameMasterController.Global.ChangeState(GameState.Game);

        isLoading = false;

        loadSceneName = string.Empty;
        loadPlayerStartTransformName = string.Empty;
        loadCameraStartTransformName = string.Empty;

        // unset the transition ui.

        GameUserInterfaceController.Global.uiControllerTransition.UnsetMenu();
    }   
}
