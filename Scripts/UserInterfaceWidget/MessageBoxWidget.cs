using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class MessageBoxWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Private fields.
    private UserInterfaceWidgetStatus status;
    private float translateLerpTimer;
    private Vector3 beginStatusPosition;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.MessageBox;
    public UserInterfaceWidgetStatus Status => status;
    public GameObject WidgetGameObject => gameObject;
    public bool IsAutomatic => isAutomatic;

    // Public fields.
    [Header("Widget Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string widgetId;
    public bool isAutomatic;

    [Header("Lerp Attributes")]
    public Transform startTransform;
    public Transform finishTransform;

    [Header("Message Box Attributes")]
    public Image voxImage;
    public Image continuePromptImage;
    public TextMeshProUGUI messageBoxTextbox;

    [Header("Sound Attributes")]
    public AudioSource voxAudioSource;
    public AudioSource nextCharAudioSource;

    // Context methods.
    private void SetRandomWidgetId() => widgetId = Guid.NewGuid().ToString();

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
        gameObject.transform.position = startTransform.position;
        translateLerpTimer = 0.0F;
    }

    private void Update()
    {
        if (status == UserInterfaceWidgetStatus.BeginEnable)
        {
            float lerp = Mathf.InverseLerp(0.0F, WIDGET_ENABLE_INTERVAL, translateLerpTimer);
            var pos = Vector3.Lerp(beginStatusPosition, finishTransform.position, lerp);
            gameObject.transform.position = pos;
            translateLerpTimer += Time.deltaTime;
            if (translateLerpTimer > WIDGET_ENABLE_INTERVAL)
            {
                status = UserInterfaceWidgetStatus.Enabled;
            }
        }
        else if (status == UserInterfaceWidgetStatus.BeginDisable)
        {
            float lerp = Mathf.InverseLerp(0.0F, WIDGET_ENABLE_INTERVAL, translateLerpTimer);
            var pos = Vector3.Lerp(beginStatusPosition, startTransform.position, lerp);
            gameObject.transform.position = pos;
            translateLerpTimer += Time.deltaTime;
            if (translateLerpTimer > WIDGET_ENABLE_INTERVAL)
            {
                status = UserInterfaceWidgetStatus.Disabled;
                gameObject.SetActive(false);
            }
        }
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        gameObject.SetActive(true);

        gameObject.transform.position = startTransform.position;
        status = UserInterfaceWidgetStatus.BeginEnable;
        beginStatusPosition = transform.position;
        translateLerpTimer = 0.0F;

        voxImage.sprite = null;
        voxImage.enabled = false;
        continuePromptImage.enabled = false;
        messageBoxTextbox.text = string.Empty;

        args = args ?? new Dictionary<string, object>();

        var voxSprite = args[WIDGET_ARG_MESSAGE_BOX_VOX_SPRITE] as Sprite;

        if (voxSprite != null)
        {
            voxImage.sprite = voxSprite;
            voxImage.enabled = true;
        }
    }

        public void RefreshWidget(Dictionary<string, object> args = null)
    {
        string messageBoxText = args[WIDGET_ARG_MESSAGE_BOX_TEXT] as string;

        if (messageBoxText != null)
            messageBoxTextbox.text = messageBoxText;

        bool? isContinuePromptEnabled = args[WIDGET_ARG_MESSAGE_BOX_IS_CONTINUE_PROMPT_ENABLED] as bool?;

        if (isContinuePromptEnabled != null)
            continuePromptImage.enabled = isContinuePromptEnabled.Value;
    }

    public void EndWidget()
    {
        status = UserInterfaceWidgetStatus.BeginDisable;
        beginStatusPosition = transform.position;
        translateLerpTimer = 0.0F;
    } 
}
