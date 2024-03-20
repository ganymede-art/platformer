using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Constants;

public class ChoicesWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Private fields.
    private UserInterfaceWidgetStatus status;
    private float translateLerpTimer;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.Choices;
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

    [Header("Dropdown Attributes")]
    public TextMeshProUGUI choicesTextbox;
    public TextMeshProUGUI choiceTextbox;

    [Header("Sound Attributes")]
    public AudioSource navigateAudioSource;
    public AudioSource continueAudioSource;

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
            var pos = Vector3.Lerp(startTransform.position, finishTransform.position, lerp);
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
            var pos = Vector3.Lerp(finishTransform.position, startTransform.position, lerp);
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
        translateLerpTimer = 0.0F;
    }

    public void EndWidget()
    {
        status = UserInterfaceWidgetStatus.BeginDisable;
        translateLerpTimer = 0.0F;
    }

    public void RefreshWidget(Dictionary<string, object> args = null) 
    {
        if (args == null)
            return;

        choicesTextbox.text = args[WIDGET_ARG_CHOICES_CHOICES_TEXT].ToString();
        choiceTextbox.text = args[WIDGET_ARG_CHOICES_CHOICE_TEXT].ToString();
    }
}
