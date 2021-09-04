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
    [NonSerialized] public TMP_FontAsset ui_font;
    [NonSerialized] public GUIStyle ui_style;

    // ui controllers.

    [NonSerialized] public UserInterfaceMessageBoxController ui_controller_message_box;

    // ui figures.

    int resolution_x = 0;
    int resolution_y = 0;

    public float ui_x_unit = 1f;
    public float ui_y_unit = 1f;

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

        // add controllers.

        ui_controller_message_box = this.gameObject.AddComponent<UserInterfaceMessageBoxController>();

        // load ui resources.

        ui_font = Resources.Load<TMP_FontAsset>("font/game_font");

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

        ui_x_unit = Screen.width / 100;
        ui_y_unit = Screen.height / 100;
    }

    // state change.

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;
    }
}
