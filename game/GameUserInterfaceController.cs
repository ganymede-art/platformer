using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using TMPro;
using UnityEngine.UI;
using System;

public class GameUserInterfaceController : MonoBehaviour
{
    private static GameUserInterfaceController global;
    public static GameUserInterfaceController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.userInterfaceController;
            }
            return global;
        }
    }

    // ui constants.

    const float MESSAGE_BOX_FONT_SIZE = 24F;

    // ui variables

    GameMasterController master;
    [NonSerialized] public GUIStyle ui_style;

    // ui controllers.

    [NonSerialized] public UserInterfaceMessageBoxController uiControllerMessageBox;
    [NonSerialized] public UserInterfaceGameController uiControllerGame;
    [NonSerialized] public UserInterfaceMenuPauseController uiControllerMenuPause;
    [NonSerialized] public UserInterfaceTransitionController uiControllerTransition;
    [NonSerialized] public UserInterfaceSceneTitleController uiControllerSceneTitle;

    [NonSerialized] public GameObject uiMessageBoxObject;
    [NonSerialized] public GameObject uiGameObject;
    [NonSerialized] public GameObject uiMenuPauseObject;
    [NonSerialized] public GameObject uiTransitionObject;
    [NonSerialized] public GameObject uiSceneTitleObject;

    public GameObject uiMessageBoxPrefab;
    public GameObject uiGamePrefab;
    public GameObject uiMenuPrefab;
    public GameObject uiTransitionPrefab;
    public GameObject uiSceneTitlePrefab;

    // ui figures.

    int resolution_x = 0;
    int resolution_y = 0;

    private void Awake()
    {
        resolution_x = Screen.width;
        resolution_y = Screen.height;
        AdjustLayout();
    }

    private void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // add event hooks.

        master.GameStateChange += ChangeGameState;

        // init message box.

        uiMessageBoxObject = Instantiate(uiMessageBoxPrefab, this.transform);
        uiControllerMessageBox = uiMessageBoxObject.GetComponent<UserInterfaceMessageBoxController>();

        // init game.

        uiGameObject = Instantiate(uiGamePrefab, this.transform);
        uiControllerGame = uiGameObject.GetComponent<UserInterfaceGameController>();

        // init menu.

        uiMenuPauseObject = Instantiate(uiMenuPrefab,this.transform);
        uiControllerMenuPause = uiMenuPauseObject.GetComponent<UserInterfaceMenuPauseController>();

        // init tran.

        uiTransitionObject = Instantiate(uiTransitionPrefab, this.transform);
        uiControllerTransition = uiTransitionObject.GetComponent<UserInterfaceTransitionController>();

        // init scene title.

        uiSceneTitleObject = Instantiate(uiSceneTitlePrefab, this.transform);
        uiControllerSceneTitle = uiSceneTitleObject.GetComponent<UserInterfaceSceneTitleController>();

        // initialise UI.

        Initialise();
        AdjustLayout();
    }

    private void OnDestroy()
    {
        master.GameStateChange -= ChangeGameState;
    }

    void Initialise()
    {
        ui_style = new GUIStyle();
    }

    void Update()
    {
        if(resolution_x != Screen.width || resolution_y != Screen.height)
        {
            AdjustLayout();
        }
    }

    void AdjustLayout()
    {
        resolution_x = Screen.width;
        resolution_y = Screen.height;
    }

    // state change.

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;
    }
}
