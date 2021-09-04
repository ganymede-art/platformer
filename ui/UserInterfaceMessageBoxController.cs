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

    // cutscene.

    GameObject ui_message_box;
    RectTransform ui_message_box_rect;
    TextMeshProUGUI ui_message_box_text;

    GameObject frame_object;
    Image frame_image;
    RectTransform frame_rect;
    Sprite frame_sprite;

    GameObject vox_object;
    Image vox_image;
    RectTransform vox_rect;
    Sprite vox_sprite;

    GameObject continue_object;
    Image continue_image;
    RectTransform continue_rect;
    Sprite continue_sprite;

    GameObject positive_object;
    Image positive_image;
    RectTransform positive_rect;
    Sprite positive_sprite;

    GameObject negative_object;
    Image negative_image;
    RectTransform negative_rect;
    Sprite negative_sprite;

    // vox sprites.

    public Dictionary<string, Sprite> vox_sprite_dictionary;


    // Start is called before the first frame update
    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // add event hooks.

        master.GameStateChange += ChangeGameState;

        // load ui resources.

        frame_sprite = Resources.Load<Sprite>("texture/ui/tmenu_message_box_image");
        continue_sprite = Resources.Load<Sprite>("texture/ui/tmenu_message_box_continue");
        positive_sprite = Resources.Load<Sprite>("texture/ui/tmenu_message_box_positive");
        negative_sprite = Resources.Load<Sprite>("texture/ui/tmenu_message_box_negative");
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

        ui_object = new GameObject("ui_object");
        ui_object.layer = 5;
        DontDestroyOnLoad(ui_object);

        ui_canvas = ui_object.AddComponent<Canvas>();
        ui_canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        ui_canvas_scaler = ui_object.AddComponent<CanvasScaler>();
        ui_canvas_scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        ui_canvas_scaler.referenceResolution = new Vector2(640, 480);
        ui_canvas_scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        ui_canvas_scaler.matchWidthOrHeight = 640;

        ui_object.AddComponent<GraphicRaycaster>();

        // frame.

        frame_object = new GameObject("ui_message_box_frame");
        frame_object.layer = 5;
        frame_object.transform.SetParent(ui_object.transform);

        frame_image = frame_object.AddComponent<Image>();
        frame_image.sprite = frame_sprite;

        frame_rect = frame_image.GetComponent<RectTransform>();
        frame_rect.localPosition = new Vector3(0, -170, 0);
        frame_rect.sizeDelta = new Vector2(600, 64);

        // vox icon.

        vox_object = new GameObject("ui_message_box_icon");
        vox_object.layer = 5;
        vox_object.transform.SetParent(ui_object.transform);

        vox_image = vox_object.AddComponent<Image>();
        vox_image.sprite = vox_sprite;

        vox_rect = vox_image.GetComponent<RectTransform>();
        vox_rect.localPosition = new Vector3(-250, -170, 0);
        vox_rect.sizeDelta = new Vector2(48, 48);

        // continue icon.

        continue_object = new GameObject("ui_continue");
        continue_object.layer = 5;
        continue_object.transform.SetParent(ui_object.transform);

        continue_image = continue_object.AddComponent<Image>();
        continue_image.sprite = continue_sprite;

        continue_rect = continue_image.GetComponent<RectTransform>();
        continue_rect.localPosition = new Vector3(250, -190, 0);
        continue_rect.sizeDelta = new Vector2(32, 32);

        // positive icon.

        positive_object = new GameObject("ui_positive");
        positive_object.layer = 5;
        positive_object.transform.SetParent(ui_object.transform);

        positive_image = positive_object.AddComponent<Image>();
        positive_image.sprite = positive_sprite ;

        positive_rect = positive_image.GetComponent<RectTransform>();
        positive_rect.localPosition = new Vector3(250, -190, 0);
        positive_rect.sizeDelta = new Vector2(32, 32);

        // negative icon.

        negative_object = new GameObject("ui_positive");
        negative_object.layer = 5;
        negative_object.transform.SetParent(ui_object.transform);

        negative_image = negative_object.AddComponent<Image>();
        negative_image.sprite = negative_sprite;

        negative_rect = negative_image.GetComponent<RectTransform>();
        negative_rect.localPosition = new Vector3(220, -190, 0);
        negative_rect.sizeDelta = new Vector2(32, 32);

        // message box.

        ui_message_box = new GameObject("ui_message_box_text");
        ui_message_box.layer = 5;
        ui_message_box.transform.SetParent(ui_object.transform);

        ui_message_box_text = ui_message_box.AddComponent<TextMeshProUGUI>();
        ui_message_box_text.color = Color.white;
        ui_message_box_text.font = master.user_interface_controller.ui_font;
        ui_message_box_text.fontSize = MESSAGE_BOX_FONT_SIZE;
        ui_message_box_text.text = string.Empty;

        ui_message_box_rect = ui_message_box_text.GetComponent<RectTransform>();
        ui_message_box_rect.localPosition = new Vector3(20, -175, 0);
        ui_message_box_rect.sizeDelta = new Vector2(480, 50);
    }

    void Update()
    {
        if (master.game_state != GameState.Cutscene)
            return;

        // set visible only if the current event type is message box.

        if (master.cutscene_controller.event_source == null)
            return;

        continue_object.SetActive(master.cutscene_controller.Is_Current_Event_Process_Complete
            && master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX);

        positive_object.SetActive(master.cutscene_controller.Is_Current_Event_Process_Complete
            && master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION);

        negative_object.SetActive(master.cutscene_controller.Is_Current_Event_Process_Complete
            && master.cutscene_controller.Current_Event_Type == GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION);

        ui_object.SetActive(
            master.cutscene_controller.current_event.GetEventType() == GameConstants.EVENT_TYPE_MESSAGE_BOX
            || master.cutscene_controller.current_event.GetEventType() == GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION);
    }

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        ui_object.SetActive(args.game_state == GameState.Cutscene);

        if(args.game_state != GameState.Cutscene)
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
