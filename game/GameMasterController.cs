using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.script;
using UnityEngine.Serialization;

public enum GameState
{
    MainMenu,
    Menu,
    Game,
    Loading,
    GameOver,
    Cutscene,
    GameCutscene
}

public class GameMasterController : MonoBehaviour
{
    // state variables.

    [System.NonSerialized] public GameState gameStatePrevious;
    [System.NonSerialized] public GameState gameState;
    private float gameStateTimer = 0.0f;

    public float GameStateTime
    { get => gameStateTimer; }

    // master components.

    public GameLoadLevelController load_level_controller;
    public GameInputController input_controller;
    public GameAudioController audio_controller;
    public GamePlayerController player_controller;
    public GameDataController data_controller;
    public GameCutsceneController cutscene_controller;
    public GameUserInterfaceController user_interface_controller;

    // event handler

    public event EventHandler GameStateChange;
    private GameStateChangeEventArgs gameStateChangeEventArgs;

    // prefabs.

    public GameObject player_prefab;
    public GameObject camera_prefab;

    // static references.

    private static GameMasterController globalMasterController;
    private static GameObject globalCameraObject;
    private static GameObject globalPlayerObject;
    private static PlayerMovementController globalPlayerController;

    // global random.

    [NonSerialized] public static System.Random staticRandom = new System.Random();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameStatePrevious = GameState.MainMenu;
        gameState = GameState.MainMenu;
        gameStateChangeEventArgs = new GameStateChangeEventArgs();
        gameStateChangeEventArgs.gameState = gameState;

        load_level_controller = this.gameObject.AddComponent<GameLoadLevelController>();
        input_controller = this.gameObject.GetComponent<GameInputController>();
        audio_controller = this.gameObject.GetComponent<GameAudioController>();
        player_controller = this.gameObject.GetComponent<GamePlayerController>();
        data_controller = this.gameObject.GetComponent<GameDataController>();
        cutscene_controller = this.gameObject.GetComponent<GameCutsceneController>();
        user_interface_controller = this.gameObject.GetComponentInChildren<GameUserInterfaceController>();

        // setup physics.

        Physics.IgnoreLayerCollision(8, 9);
        Physics.IgnoreLayerCollision(8, 10);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void Update()
    {
        gameStateTimer += Time.deltaTime;

        if(gameState == GameState.Game || gameState == GameState.GameCutscene)
        {
            if(!input_controller.wasInputStart && input_controller.isInputStart)
            {
                ChangeState(GameState.Menu);
            }
        }
    }

    public void ChangeState(GameState new_game_state)
    {
        gameStatePrevious = gameState;
        gameState = new_game_state;
        gameStateChangeEventArgs.gameState = gameState;
        gameStateChangeEventArgs.game_state_previous = gameStatePrevious;
        gameStateTimer = 0.0f;

        EventHandler handler = GameStateChange;
        if (handler != null) handler(this, gameStateChangeEventArgs);
    }

    public static GameMasterController GetMasterController()
    {
        if(globalMasterController == null)
            globalMasterController = GameObject.FindObjectOfType<GameMasterController>();

        return globalMasterController;
    }

    public static GameObject GetPlayerObject()
    {
        if (globalPlayerObject == null)
            globalPlayerObject = GameObject.Find(GameConstants.NAME_PLAYER);

        return globalPlayerObject;
    }

    public static GameObject GetPlayerCameraObject()
    {
        if(globalCameraObject == null)
            globalCameraObject = GameObject.Find(GameConstants.NAME_PLAYER_CAMERA);

        return globalCameraObject;
    }

    public static PlayerMovementController GetPlayerController()
    {
        if (globalPlayerObject == null)
            GetPlayerObject();

        if (globalPlayerController == null)
            globalPlayerController = globalPlayerObject.GetComponent<PlayerMovementController>();

        return globalPlayerController;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode load_scene_mode)
    {
        globalMasterController = null;
        globalPlayerObject = null;
        globalCameraObject = null;
        globalPlayerController = null;
    }
}

public class GameStateChangeEventArgs : EventArgs
{
    public GameState gameState;
    public GameState game_state_previous;
}
