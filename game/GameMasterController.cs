using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.script;

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

    [System.NonSerialized] public GameState game_state_previous;
    [System.NonSerialized] public GameState game_state;
    private float game_state_time = 0.0f;

    public float Game_State_Time
    { get => game_state_time; }

    [System.NonSerialized] public GameSceneData game_scene_data;

    // master components.

    public GameLoadLevelController load_level_controller;
    public GameInputController input_controller;
    public GameAudioController audio_controller;
    public GamePlayerController player_controller;
    public GameDataController data_controller;
    public GameCutsceneController cutscene_controller;
    public GameUserInterfaceController user_interface_controller;
    public GameResourceController resource_controller;

    // event handler

    public event EventHandler GameStateChange;
    private GameStateChangeEventArgs game_state_change_event_args;

    // prefabs.

    public GameObject player_prefab;
    public GameObject camera_prefab;

    // static references.

    private static GameMasterController global_master_controller;
    private static GameObject global_camera_object;
    private static GameObject global_player_object;
    private static PlayerMovementController global_player_controller;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        game_state_previous = GameState.MainMenu;
        game_state = GameState.MainMenu;
        game_state_change_event_args = new GameStateChangeEventArgs();
        game_state_change_event_args.game_state = game_state;

        load_level_controller = this.gameObject.AddComponent<GameLoadLevelController>();
        input_controller = this.gameObject.GetComponent<GameInputController>();
        audio_controller = this.gameObject.GetComponent<GameAudioController>();
        player_controller = this.gameObject.GetComponent<GamePlayerController>();
        data_controller = this.gameObject.GetComponent<GameDataController>();
        cutscene_controller = this.gameObject.GetComponent<GameCutsceneController>();
        user_interface_controller = this.gameObject.GetComponentInChildren<GameUserInterfaceController>();
        resource_controller = this.gameObject.AddComponent<GameResourceController>();
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
        game_state_time += Time.deltaTime;
    }

    public void ChangeState(GameState new_game_state)
    {
        game_state_previous = game_state;
        game_state = new_game_state;
        game_state_change_event_args.game_state = game_state;
        game_state_time = 0.0f;

        EventHandler handler = GameStateChange;
        if (handler != null) handler(this, game_state_change_event_args);
    }

    public static GameMasterController GetMasterController()
    {
        if(global_master_controller == null)
            global_master_controller = GameObject.FindObjectOfType<GameMasterController>();

        return global_master_controller;
    }

    public static GameObject GetPlayerObject()
    {
        if (global_player_object == null)
            global_player_object = GameObject.Find(GameConstants.NAME_PLAYER);

        return global_player_object;
    }

    public static GameObject GetPlayerCameraObject()
    {
        if(global_camera_object == null)
            global_camera_object = GameObject.Find(GameConstants.NAME_PLAYER_CAMERA);

        return global_camera_object;
    }

    public static PlayerMovementController GetPlayerController()
    {
        if (global_player_object == null)
            GetPlayerObject();

        if (global_player_controller == null)
            global_player_controller = global_player_object.GetComponent<PlayerMovementController>();

        return global_player_controller;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode load_scene_mode)
    {
        global_master_controller = null;
        global_player_object = null;
        global_camera_object = null;
        global_player_controller = null;
    }
}

public class GameStateChangeEventArgs : EventArgs
{
    public GameState game_state;
}
