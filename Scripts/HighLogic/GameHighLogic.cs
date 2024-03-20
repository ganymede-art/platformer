using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameHighLogic : MonoBehaviour
{
    private static GameHighLogic g;

    private static bool isApplicationQuitting;

    private AssetsHighLogic assetsHighLogic;
    private TextsHighLogic textsHighLogic;
    private SettingsHighLogic settingsHighLogic;
    private StateHighLogic stateHighLogic;
    private InputHighLogic inputHighLogic;
    private PersistenceHighLogic persistenceHighLogic;
    private LoadSceneHighLogic loadSceneHighLogic;
    private ActiveSceneHighLogic activeSceneHighLogic;
    private PlayerHighLogic playerHighLogic;
    private UserInterfaceHighLogic userInterfaceHighLogic;
    private ActionHighLogic actionHighLogic;
    private TimeHighLogic timeHighLogic;
    private MusicHighLogic musicHighLogic;
    private TimerHighLogic timerHighLogic;

    public static GameHighLogic G
    {
        get
        {
            if (g == null)
                g = FindOrCreateGameHighLogic();

            return g;
        }
    }

    public static bool IsApplicationQuitting => isApplicationQuitting;

    public AssetsHighLogic AssetsHighLogic => assetsHighLogic;
    public TextsHighLogic TextsHighLogic => textsHighLogic;
    public SettingsHighLogic SettingsHighLogic => settingsHighLogic;
    public UserInterfaceHighLogic UserInterfaceHighLogic => userInterfaceHighLogic;
    public StateHighLogic StateHighLogic => stateHighLogic;
    public InputHighLogic InputHighLogic => inputHighLogic;
    public PersistenceHighLogic PersistenceHighLogic => persistenceHighLogic;
    public LoadSceneHighLogic LoadSceneHighLogic => loadSceneHighLogic;
    public ActiveSceneHighLogic ActiveSceneHighLogic => activeSceneHighLogic;
    public PlayerHighLogic PlayerHighLogic => playerHighLogic;
    public ActionHighLogic ActionHighLogic => actionHighLogic;
    public TimeHighLogic TimeHighLogic => timeHighLogic;
    public MusicHighLogic MusicHighLogic => musicHighLogic;
    public TimerHighLogic TimerHighLogic => timerHighLogic;

    private void Awake()
    {
        Application.targetFrameRate = 240;

        DontDestroyOnLoad(this);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_PLAYER);
        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_MOB);
        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_MOB_ONLY);
        //Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_ITEM);
        Physics.IgnoreLayerCollision(LAYER_PLAYER, LAYER_ITEM_ONLY);

        Shader.SetGlobalFloat(SHADER_PROPERTY_NAME_STATE_SPEED, 1.0F);

        assetsHighLogic = gameObject.AddComponent<AssetsHighLogic>();
        textsHighLogic = gameObject.AddComponent<TextsHighLogic>();
        settingsHighLogic = gameObject.AddComponent<SettingsHighLogic>();
        userInterfaceHighLogic = gameObject.AddComponent<UserInterfaceHighLogic>();
        stateHighLogic = gameObject.AddComponent<StateHighLogic>();
        inputHighLogic = gameObject.AddComponent<InputHighLogic>();
        persistenceHighLogic = gameObject.AddComponent<PersistenceHighLogic>();
        loadSceneHighLogic = gameObject.AddComponent<LoadSceneHighLogic>();
        activeSceneHighLogic = gameObject.AddComponent<ActiveSceneHighLogic>();
        playerHighLogic = gameObject.AddComponent<PlayerHighLogic>();
        actionHighLogic = gameObject.AddComponent<ActionHighLogic>();
        timeHighLogic = gameObject.AddComponent<TimeHighLogic>();
        musicHighLogic = gameObject.AddComponent<MusicHighLogic>();
        timerHighLogic = gameObject.AddComponent<TimerHighLogic>();

        Application.quitting += () => isApplicationQuitting = true;
    }

    private static GameHighLogic FindOrCreateGameHighLogic()
    {
        if (isApplicationQuitting)
            return null;

        var highLogic = GameObject.FindObjectOfType<GameHighLogic>();

        if (highLogic != null)
            return highLogic;

        var highLogicPrefab = Resources.Load<GameObject>(RESOURCE_PATH_GAME_HIGH_LOGIC_PREFAB);
        var highLogicObject = Instantiate(highLogicPrefab);

        highLogic = highLogicObject.GetComponent<GameHighLogic>();

        LoadSceneHighLogic.G.LoadNewScene(SceneManager.GetActiveScene().name, HighLogicStateId.Play);

        return highLogic;

    }

    public static void ResetGame()
    {
        var gameObjects = GameObject.FindObjectsOfType<GameObject>(includeInactive: true);
        foreach (GameObject o in gameObjects)
            Destroy(o);
        SceneManager.LoadScene("MenuStartup1");
    }

    #if UNITY_EDITOR
    private void Update()
    {
        if(Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            PlayerHighLogic.G.ModifyCanAttack(true);
            PlayerHighLogic.G.ModifyCanDoubleJump(true);
            PlayerHighLogic.G.ModifyCanDiveUnderwater(true);
            PlayerHighLogic.G.ModifyCanAttackUnderwater(true);
            PlayerHighLogic.G.ModifyCanLunge(true);
            PlayerHighLogic.G.ModifyCanSlam(true);
            PlayerHighLogic.G.ModifyCanHighJump(true);
            PlayerHighLogic.G.ModifyCanShoot(true);
        }

        if(Keyboard.current.digit4Key.isPressed)
        {
            ActiveSceneHighLogic.G.CachedPlayer.playerRigidBody.velocity = Vector3.zero;
            ActiveSceneHighLogic.G.CachedPlayer.playerRigidBody.MovePosition
                (ActiveSceneHighLogic.G.CachedPlayer.transform.position + (Vector3.up * 0.1F));
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            PlayerHighLogic.G.ModifyHealth(1);
            PlayerHighLogic.G.ModifyOxygen(1);
            PlayerHighLogic.G.ModifyAmmo(1);
            PlayerHighLogic.G.ModifyMoney(1);
        }
    }
    #endif
}
