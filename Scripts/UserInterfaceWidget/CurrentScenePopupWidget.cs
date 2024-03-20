using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using static Constants;

public class CurrentScenePopupWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Consts.
    private static readonly Color disabledColor = new Color(1.0F, 1.0F, 1.0F, 0.0F);
    private static readonly Color enabledColor = new Color(1.0F, 1.0F, 1.0F, 1.0F);
    private const float DISPLAY_INTERVAL = 3.0F;

    // Private fields.
    private UserInterfaceWidgetStatus status;
    private float statusTimer;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.CurrentScenePopup;
    public UserInterfaceWidgetStatus Status => status;
    public GameObject WidgetGameObject => gameObject;
    public bool IsAutomatic => isAutomatic;

    // Public fields.
    [Header("Widget Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string widgetId;
    public bool isAutomatic;
    [Space]
    public TextMeshProUGUI popupTextbox;

    // Context methods.
    private void SetRandomWidgetId() => widgetId = Guid.NewGuid().ToString();

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    private void Update()
    {
        if (status == UserInterfaceWidgetStatus.BeginEnable)
        {
            float lerp = Mathf.InverseLerp(0.0F, WIDGET_ENABLE_INTERVAL, statusTimer);
            popupTextbox.color = Color.Lerp(disabledColor, enabledColor, lerp);
            statusTimer += Time.deltaTime;
            if (statusTimer > WIDGET_ENABLE_INTERVAL)
            {
                statusTimer = 0.0F;
                status = UserInterfaceWidgetStatus.Enabled;
            }
        }
        else if(status == UserInterfaceWidgetStatus.Enabled)
        {
            statusTimer += Time.deltaTime;
            if(statusTimer >= DISPLAY_INTERVAL)
            {
                statusTimer = 0.0F;
                status = UserInterfaceWidgetStatus.BeginDisable;
            }
        }
        else if (status == UserInterfaceWidgetStatus.BeginDisable)
        {
            float lerp = Mathf.InverseLerp(0.0F, WIDGET_ENABLE_INTERVAL, statusTimer);
            popupTextbox.color = Color.Lerp(enabledColor, disabledColor, lerp);
            statusTimer += Time.deltaTime;
            if (statusTimer > WIDGET_ENABLE_INTERVAL)
            {
                statusTimer = 0.0F;
                status = UserInterfaceWidgetStatus.Disabled;
                gameObject.SetActive(false);
            }
        }
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        popupTextbox.color = disabledColor;
        gameObject.SetActive(true);
        status = UserInterfaceWidgetStatus.BeginEnable;

        string sceneName = SceneManager.GetActiveScene().name;

        string sceneTitle = TextsHighLogic.G.GetText($"{sceneName}Title");
        string sceneSubtitle = TextsHighLogic.G.GetText($"{sceneName}Subtitle");

        popupTextbox.text = $"{sceneSubtitle} / {sceneTitle}";
    }

    public void EndWidget()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    public void RefreshWidget(Dictionary<string, object> args = null) { }
}
