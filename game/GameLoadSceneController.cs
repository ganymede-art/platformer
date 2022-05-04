using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Script;
using System;
using static Assets.Script.GameConstants;

public class GameLoadSceneController : MonoBehaviour
{
    private static GameLoadSceneController global;
    public static GameLoadSceneController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.loadSceneController;
            }
            return global;
        }
    }

    // load constants.

    const float LOAD_TIMER_MULTIPLIER = 1;
    static readonly Vector3 PLAYER_SPAWN_OFFSET = new Vector3(0.0F, 0.1875F, 0.0F);

    // load variables.

    private string loadGameState = string.Empty;
    private string loadSetupMode = string.Empty;
    private float loadTimer = 0.0F;
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
        SceneManager.sceneLoaded += EndLoadGameScene;
        SceneManager.sceneLoaded += EndLoadMenuScene;

        // initialise transition variables.

        transitionTexture = new Texture2D(1, 1);
    }

    private void Update()
    {
        if (isLoading)
        {
            // increment the load timer while loading.

            loadTimer += LOAD_TIMER_MULTIPLIER * Time.deltaTime;

            if (loadTimer >= 1.0f)
            {
                DoLoadLevel();
            }
        }
        else
        {
            // decrement the load timer if no longer loading.

            if (loadTimer > 0.0f)
            {
                loadTimer -= LOAD_TIMER_MULTIPLIER * Time.deltaTime;
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= EndLoadGameScene;
    }

    public void StartLoadGameScene(string scene_name, string player_start_transform_name, string camera_start_transform_name, UserInterfaceTransitionData transitionData = null)
    {
        // Begin loading a game scene.

        Debug.Log("[GameLoadSceneController] Loading a game scene.");
        GameMasterController.Global.ChangeState(GAME_STATE_LOAD);

        isLoading = true;

        loadGameState = GAME_STATE_GAME;
        loadSetupMode = LOAD_SETUP_MODE_GAME;
        loadTimer = 0.0F;
        
        loadSceneName = scene_name;
        loadPlayerStartTransformName = player_start_transform_name;
        loadCameraStartTransformName = camera_start_transform_name;

        // set the transition ui.

        GameUserInterfaceController.Global.uiControllerTransition.SetMenu(transitionData);
    }

    public void StartLoadMenuScene(string sceneName, string gameState, UserInterfaceTransitionData transitionData = null)
    {
        // Begin loading a menu scene.

        Debug.Log("[GameLoadSceneController] Loading a menu scene.");
        GameMasterController.Global.ChangeState(GAME_STATE_LOAD);

        isLoading = true;

        loadGameState = gameState;
        loadSetupMode = GameConstants.LOAD_SETUP_MODE_MENU;
        loadTimer = 0.0F;

        loadSceneName = sceneName;
        loadPlayerStartTransformName = string.Empty;
        loadCameraStartTransformName = string.Empty;

        // set the transition ui.

        GameUserInterfaceController.Global.uiControllerTransition.SetMenu(transitionData);
    }

    private void DoLoadLevel()
    {
        SceneManager.LoadScene(loadSceneName, LoadSceneMode.Single);
    }

    private void EndLoadMenuScene(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (loadSetupMode != LOAD_SETUP_MODE_MENU)
            return;


        // reset after loading.

        Debug.Log("[GameLoadSceneController] Resetting after loading menu scene.");
        GameMasterController.Global.ChangeState(loadGameState);

        loadGameState = string.Empty;
        loadSetupMode = string.Empty;
        isLoading = false;

        loadSceneName = string.Empty;
        loadPlayerStartTransformName = string.Empty;
        loadCameraStartTransformName = string.Empty;

        // unset the transition ui.

        GameUserInterfaceController.Global.uiControllerTransition.UnsetMenu();
    }

    private void EndLoadGameScene(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (loadSetupMode != LOAD_SETUP_MODE_GAME)
            return;

        

        // get the player and camera start transforms.

        var player_start_transform = GameObject.Find
            (loadPlayerStartTransformName).transform;

        var camera_start_transform = GameObject.Find
            (loadCameraStartTransformName).transform;

        // initialise the player and camera.

        var player_prefab = GameMasterController.Global.playerPrefab;
        var camera_prefab = GameMasterController.Global.cameraPrefab;

        var player = Instantiate(player_prefab, 
            player_start_transform.position + PLAYER_SPAWN_OFFSET, 
            player_start_transform.rotation);
        var camera = Instantiate(camera_prefab, 
            camera_start_transform.position, 
            camera_start_transform.rotation);

        // name the prefabs.

        player.name = GameConstants.NAME_PLAYER;
        camera.name = GameConstants.NAME_PLAYER_CAMERA;

        // reset after loading.

        Debug.Log("[GameLoadSceneController] Resetting after loading menu scene.");
        GameMasterController.Global.ChangeState(GAME_STATE_GAME);

        loadGameState = string.Empty;
        loadSetupMode = string.Empty;
        isLoading = false;

        loadSceneName = string.Empty;
        loadPlayerStartTransformName = string.Empty;
        loadCameraStartTransformName = string.Empty;

        // unset the transition ui.

        GameUserInterfaceController.Global.uiControllerTransition.UnsetMenu();
    }   
}
