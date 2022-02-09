using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.script;
using UnityEngine.Serialization;
using static Assets.script.GameConstants;

public class GameMasterController : MonoBehaviour
{
    // private state variables.

    private float gameStateTimer = 0.0F;

    // private event handler variables.

    private GameStateChangeEventArgs gameStateChangeEventArgs;

    // private global variables.

    private static GameMasterController globalMasterController;
    private static GameObject globalCameraObject;
    private static GameObject globalPlayerObject;
    private static PlayerController globalPlayerController;

    // public state variables.

    [NonSerialized] public string gameStatePrevious;
    [NonSerialized] public string gameState;

    public float GameStateTime
    { get => gameStateTimer; }

    // public master variables.

    [NonSerialized] public GameLoadSceneController loadSceneController;
    [NonSerialized] public GameInputController inputController;
    [NonSerialized] public GameAudioController audioController;
    [NonSerialized] public GamePlayerController playerController;
    [NonSerialized] public GameDataController dataController;
    [NonSerialized] public GameEventController cutsceneController;
    [NonSerialized] public GameUserInterfaceController userInterfaceController;
    [NonSerialized] public GameSceneController sceneController;
    [NonSerialized] public GameSettingsController settingsController;

    // public event handler variables.

    public event EventHandler GameStateChange;

    // prefabs.

    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    // global random variables.

    [NonSerialized] public static System.Random staticRandom = new System.Random();

    // static references.

    public static GameMasterController Global
    {
        get
        {
            if (globalMasterController == null)
                globalMasterController = GameObject.FindObjectOfType<GameMasterController>();

            return globalMasterController;
        }
    }

    public static GameObject GlobalCameraObject
    {
        get
        {
            if (globalCameraObject == null)
                globalCameraObject = GameObject.Find(GameConstants.NAME_PLAYER_CAMERA);

            return globalCameraObject;
        }
    }

    public static GameObject GlobalPlayerObject
    {
        get
        {
            if (globalPlayerObject == null)
                globalPlayerObject = GameObject.Find(GameConstants.NAME_PLAYER);

            return globalPlayerObject;
        }
    }

    public static PlayerController GlobalPlayerController
    {
        get
        {
            if (globalPlayerController == null)
                globalPlayerController = GlobalPlayerObject.GetComponent<PlayerController>();

            return globalPlayerController;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameStatePrevious = GAME_STATE_MENU_MAIN;
        gameState = GAME_STATE_MENU_MAIN;
        gameStateChangeEventArgs = new GameStateChangeEventArgs();
        gameStateChangeEventArgs.gameState = gameState;

        loadSceneController = gameObject.AddComponent<GameLoadSceneController>();
        inputController = gameObject.GetComponent<GameInputController>();
        audioController = gameObject.GetComponent<GameAudioController>();
        playerController = gameObject.GetComponent<GamePlayerController>();
        dataController = gameObject.GetComponent<GameDataController>();
        cutsceneController = gameObject.GetComponent<GameEventController>();
        userInterfaceController = gameObject.GetComponentInChildren<GameUserInterfaceController>();
        sceneController = gameObject.GetComponent<GameSceneController>();
        settingsController = gameObject.GetComponent<GameSettingsController>();

        // setup physics.

        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_PLAYER);
        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_MOB);
        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_MOB_ONLY); ;

        Physics.IgnoreLayerCollision(LAYER_MOB, LAYER_PLAYER_ONLY);
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

        if(gameState == GAME_STATE_GAME)
        {
            if(!inputController.wasInputStart && inputController.isInputStart)
            {
                ChangeState(GAME_STATE_MENU_PAUSE);
            }
        }
    }

    public void ChangeState(string new_game_state)
    {
        gameStatePrevious = gameState;
        gameState = new_game_state;
        gameStateChangeEventArgs.gameState = gameState;
        gameStateChangeEventArgs.game_state_previous = gameStatePrevious;
        gameStateTimer = 0.0F;

        EventHandler handler = GameStateChange;
        if (handler != null) handler(this, gameStateChangeEventArgs);
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
    public string gameState;
    public string game_state_previous;
}
