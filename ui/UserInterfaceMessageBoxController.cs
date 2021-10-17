using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using TMPro;
using UnityEngine.UI;
using System;
using static Assets.script.UserInterfaceConstants;

public class UserInterfaceMessageBoxController : MonoBehaviour
{
    GameMasterController master;

    // root.

    bool is_active = false;
    bool is_visible = false;
    GameObject ui_object;
    Canvas ui_canvas;
    CanvasScaler ui_canvas_scaler;

    // cutscene.

    GameObject ui_message_box;
    RectTransform ui_message_box_rect;
    TextMeshProUGUI ui_message_box_text;

    GameObject frame_object;

    GameObject vox_object;
    Image vox_image;
    Sprite vox_sprite;

    GameObject continue_object;
    GameObject positive_object;
    GameObject negative_object;

    // Start is called before the first frame update
    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // add event hooks.

        master.GameStateChange += ChangeGameState;

        // load ui resources.

        vox_sprite = Resources.Load<Sprite>("texture/vox/default");

        // initialise UI.

        Initialise();
    }

    private void OnDestroy()
    {
        master.GameStateChange -= ChangeGameState;
    }

    void Initialise()
    {
        // create UI.

        ui_object = this.gameObject;
        
        ui_message_box = ui_object.transform.Find("ui_message_box_text").gameObject;
        ui_message_box_text = ui_message_box.GetComponent<TextMeshProUGUI>();

        frame_object = ui_object.transform.Find("ui_message_box_frame").gameObject;

        vox_object = ui_object.transform.Find("ui_message_box_icon").gameObject;
        vox_image = vox_object.GetComponent<Image>();

        continue_object = ui_object.transform.Find("ui_continue").gameObject;
        positive_object = ui_object.transform.Find("ui_positive").gameObject;
        negative_object = ui_object.transform.Find("ui_negative").gameObject;

        ui_object.SetActive(false);
    }

    void Update()
    {
        if (!is_active)
            return;

        // set visible if active.

        is_visible =
        (
            (master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX
                || master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION)
            &&
            (master.gameState == GameState.Cutscene
                || master.gameState == GameState.GameCutscene)
        );

        ui_object.SetActive(is_visible);

        // set visible only if the current event type is message box.

        if (master.cutscene_controller.currentEventSource == null)
            return;

        continue_object.SetActive(master.cutscene_controller.Is_Current_Event_Process_Complete
            && master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX
            && master.gameState == GameState.Cutscene);

        positive_object.SetActive(master.cutscene_controller.Is_Current_Event_Process_Complete
            && master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION
            && master.gameState == GameState.Cutscene);

        negative_object.SetActive(master.cutscene_controller.Is_Current_Event_Process_Complete
            && master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION
            && master.gameState == GameState.Cutscene);
    }

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;
    }

    public void SetMessageBox(Sprite message_icon)
    {
        is_active = true;

        ui_object.SetActive(true);

        vox_image.sprite = message_icon;
    }

    public void UpdateMessageBox(string message_text)
    {
        ui_message_box_text.text = message_text;
    }

    public void UnsetMessageBox()
    {
        is_active = false;

        ui_object.SetActive(false);

        ui_message_box_text.text = string.Empty;
        vox_image.sprite = vox_sprite;
    }
}
