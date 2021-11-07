using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using TMPro;
using UnityEngine.UI;
using System;
using static Assets.script.UserInterfaceConstants;
using UnityEngine.SceneManagement;

public class UserInterfaceMessageBoxController : MonoBehaviour
{
    GameMasterController master;

    // root.

    bool isActive = false;
    bool isVisible = false;
    bool isComplete = false;
    bool isQuestion = false;
    bool isDisplayPrompts = false;
    GameObject uiObject;
    Canvas uiCanvas;
    CanvasScaler uiCanvasScaler;

    // cutscene.

    GameObject uiMessageBox;
    RectTransform uiMessageBoxRect;
    TextMeshProUGUI uiMessageBoxText;

    GameObject frameObject;

    GameObject voxObject;
    Image voxImage;
    Sprite voxSprite;

    GameObject continueObject;
    GameObject positiveObject;
    GameObject negativeObject;

    // Start is called before the first frame update
    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // add event hooks.

        master.GameStateChange += ChangeGameState;
        SceneManager.sceneLoaded += SceneLoaded;

        // load ui resources.

        voxSprite = Resources.Load<Sprite>("texture/vox/default");

        // initialise UI.

        Initialise();
    }

    private void OnDestroy()
    {
        master.GameStateChange -= ChangeGameState;
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    void Initialise()
    {
        // create UI.

        uiObject = this.gameObject;
        
        uiMessageBox = uiObject.transform.Find("ui_message_box_text").gameObject;
        uiMessageBoxText = uiMessageBox.GetComponent<TextMeshProUGUI>();

        frameObject = uiObject.transform.Find("ui_message_box_frame").gameObject;

        voxObject = uiObject.transform.Find("ui_message_box_icon").gameObject;
        voxImage = voxObject.GetComponent<Image>();

        continueObject = uiObject.transform.Find("ui_continue").gameObject;
        positiveObject = uiObject.transform.Find("ui_positive").gameObject;
        negativeObject = uiObject.transform.Find("ui_negative").gameObject;

        uiObject.SetActive(false);
    }

    void Update()
    {
        if (!isActive)
            return;

        // set visible if active.

        // set visible only if the current event type is message box.

        continueObject.SetActive(isComplete
            && !isQuestion
            && isDisplayPrompts);

        positiveObject.SetActive(isComplete
            && isQuestion
            && isDisplayPrompts);

        negativeObject.SetActive(isComplete
            && isQuestion
            && isDisplayPrompts);
    }

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode load_scene_mode)
    {
        // unset the message box when a new
        // scene is loaded.

        UnsetMessageBox();
    }

    public void SetMessageBox(Sprite message_icon, bool isQuestion, bool isDisplayPrompts)
    {
        isActive = true;

        uiObject.SetActive(true);
        continueObject.SetActive(false);
        positiveObject.SetActive(false);
        negativeObject.SetActive(false);

        voxImage.sprite = message_icon;

        this.isComplete = false;
        this.isQuestion = isQuestion;
        this.isDisplayPrompts = isDisplayPrompts;
    }

    public void UpdateMessageBox(string message_text, bool isComplete)
    {
        uiMessageBoxText.text = message_text;

        this.isComplete = isComplete;
    }

    public void UnsetMessageBox()
    {
        isActive = false;

        uiObject.SetActive(false);

        uiMessageBoxText.text = string.Empty;
        voxImage.sprite = voxSprite;
    }
}
