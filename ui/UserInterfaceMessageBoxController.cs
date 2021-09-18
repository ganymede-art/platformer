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

    GameObject ui_object;
    Canvas ui_canvas;
    CanvasScaler ui_canvas_scaler;

    public GameObject ui_prefab;

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

    // vox sprites.

    public Dictionary<string, Sprite> vox_sprite_dictionary;


    // Start is called before the first frame update
    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // add event hooks.

        master.GameStateChange += ChangeGameState;

        // load ui resources.

        vox_sprite = Resources.Load<Sprite>("texture/vox/default");

        // initialise and load vox resources.

        vox_sprite_dictionary = new Dictionary<string, Sprite>();

        var vox_sprites = Resources.LoadAll<Sprite>("texture/vox");

        foreach (var vox_sprite in vox_sprites)
        {
            vox_sprite_dictionary.Add(vox_sprite.name, vox_sprite);
        }

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

        ui_object = Instantiate(ui_prefab, this.transform);
        ui_object.layer = 5;
        DontDestroyOnLoad(ui_object);

        ui_message_box = ui_object.transform.Find("ui_message_box_text").gameObject;
        ui_message_box_text = ui_message_box.GetComponent<TextMeshProUGUI>();

        frame_object = ui_object.transform.Find("ui_message_box_frame").gameObject;

        vox_object = ui_object.transform.Find("ui_message_box_icon").gameObject;
        vox_image = vox_object.GetComponent<Image>();

        continue_object = ui_object.transform.Find("ui_continue").gameObject;
        positive_object = ui_object.transform.Find("ui_positive").gameObject;
        negative_object = ui_object.transform.Find("ui_negative").gameObject;
    }

    void Update()
    {
        if (master.game_state != GameState.Cutscene
            && master.game_state != GameState.GameCutscene)
            return;

        // set visible only if the current event type is message box.

        if (master.cutscene_controller.event_source == null)
            return;

        continue_object.SetActive(master.cutscene_controller.Is_Current_Event_Process_Complete
            && master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX
            && master.game_state == GameState.Cutscene);

        positive_object.SetActive(master.cutscene_controller.Is_Current_Event_Process_Complete
            && master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION
            && master.game_state == GameState.Cutscene);

        negative_object.SetActive(master.cutscene_controller.Is_Current_Event_Process_Complete
            && master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION
            && master.game_state == GameState.Cutscene);

        ui_object.SetActive(
            master.cutscene_controller.current_event.GetEventType() == GameConstants.EVENT_TYPE_MESSAGE_BOX
            || master.cutscene_controller.current_event.GetEventType() == GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION);
    }

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;
        
        ui_object.SetActive(args.game_state == GameState.Cutscene 
            || args.game_state == GameState.GameCutscene);

        if(args.game_state != GameState.Cutscene 
            && args.game_state != GameState.GameCutscene)
        {
            UnsetMessageBox();
        }
    }

    public void SetMessageBox(string message_icon)
    {
        vox_image.sprite = vox_sprite_dictionary[message_icon];
    }

    public void UpdateMessageBox(string message_text)
    {
        ui_message_box_text.text = message_text;
    }

    public void UnsetMessageBox()
    {
        ui_message_box_text.text = string.Empty;
        vox_sprite = vox_sprite_dictionary["default"];
    }
}
