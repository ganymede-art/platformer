using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using TMPro;
using UnityEngine.UI;
using System;

public class GameUserInterfaceController : MonoBehaviour
{
    // ui constants.

    const float MESSAGE_BOX_FONT_SIZE = 24f;

    // ui variables

    GameMasterController master;
    [NonSerialized] public GUIStyle ui_style;

    // ui controllers.

    [NonSerialized] public UserInterfaceMessageBoxController ui_controller_message_box;
    [NonSerialized] public UserInterfaceGameController ui_controller_game;
    [NonSerialized] public UserInterfaceMenuController ui_controller_menu;

    [NonSerialized] public GameObject uiMessageBoxObject;
    [NonSerialized] public GameObject uiGameObject;
    [NonSerialized] public GameObject uiMenuObject;

    public GameObject uiMessageBoxPrefab;
    public GameObject uiGamePrefab;
    public GameObject uiMenuPrefab;

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
        ui_controller_message_box = uiMessageBoxObject.GetComponent<UserInterfaceMessageBoxController>();

        // init game.

        uiGameObject = Instantiate(uiGamePrefab, this.transform);
        ui_controller_game = uiGameObject.GetComponent<UserInterfaceGameController>();

        // init menu.

        uiMenuObject = Instantiate(uiMenuPrefab,this.transform);
        ui_controller_menu = uiMenuObject.GetComponent<UserInterfaceMenuController>();

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
