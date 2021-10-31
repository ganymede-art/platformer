using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.script;
using UnityEngine.Serialization;
using static Assets.script.GameConstants;

public enum GameState
{
    MainMenu,
    Menu,
    Game,
    Loading,
    GameOver,
    Cutscene
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

    public GameLoadLevelController loadLevelController;
    public GameInputController inputController;
    public GameAudioController audioController;
    public GamePlayerController playerController;
    public GameDataController dataController;
    public GameCutsceneController cutsceneController;
    public GameUserInterfaceController userInterfaceController;

    // event handler

    public event EventHandler GameStateChange;
    private GameStateChangeEventArgs gameStateChangeEventArgs;

    // prefabs.

    [FormerlySerializedAs("player_prefab")]
    public GameObject playerPrefab;
    [FormerlySerializedAs("camera_prefab")]
    public GameObject cameraPrefab;

    // static references.

    private static GameMasterController globalMasterController;
    private static GameObject globalCameraObject;
    private static GameObject globalPlayerObject;
    private static PlayerController globalPlayerController;

    public static GameMasterController GlobalMasterController
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

        loadLevelController = this.gameObject.AddComponent<GameLoadLevelController>();
        inputController = this.gameObject.GetComponent<GameInputController>();
        audioController = this.gameObject.GetComponent<GameAudioController>();
        playerController = this.gameObject.GetComponent<GamePlayerController>();
        dataController = this.gameObject.GetComponent<GameDataController>();
        cutsceneController = this.gameObject.GetComponent<GameCutsceneController>();
        userInterfaceController = this.gameObject.GetComponentInChildren<GameUserInterfaceController>();

        // setup physics.

        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_PLAYER);

        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_ENEMY);
        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_ACTOR);
        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_ENEMY_BOUNDARY); ;
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

        if(gameState == GameState.Game)
        {
            if(!inputController.wasInputStart && inputController.isInputStart)
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
