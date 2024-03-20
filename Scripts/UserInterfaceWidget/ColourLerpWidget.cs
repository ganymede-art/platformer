using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class ColourLerpWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Private fields.
    private UserInterfaceWidgetStatus status;
    private float colourLerpTimer;
    private Color lerpColor;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.ColourLerp;
    public UserInterfaceWidgetStatus Status => status;
    public GameObject WidgetGameObject => gameObject;
    public bool IsAutomatic => isAutomatic;

    // Public fields.
    [Header("Widget Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string widgetId;
    public bool isAutomatic;
    public Image overlayImage;
    public Color disabledColor;
    public Color enabledColor;

    // Context methods.
    private void SetRandomWidgetId() => widgetId = Guid.NewGuid().ToString();

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    private void Update()
    {
        if(status == UserInterfaceWidgetStatus.BeginEnable)
        {
            float lerp = Mathf.InverseLerp(0.0F,WIDGET_ENABLE_INTERVAL,colourLerpTimer);
            overlayImage.color = Color.Lerp(lerpColor, enabledColor, lerp);
            colourLerpTimer += Time.deltaTime;
            if (colourLerpTimer > WIDGET_ENABLE_INTERVAL)
                status = UserInterfaceWidgetStatus.Enabled;
        }
        else if(status == UserInterfaceWidgetStatus.BeginDisable)
        {
            float lerp = Mathf.InverseLerp(0.0F, WIDGET_ENABLE_INTERVAL, colourLerpTimer);
            overlayImage.color = Color.Lerp(lerpColor, disabledColor, lerp);
            colourLerpTimer += Time.deltaTime;
            if (colourLerpTimer > WIDGET_ENABLE_INTERVAL)
            {
                status = UserInterfaceWidgetStatus.Disabled;
                gameObject.SetActive(false);
            }
        }
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        gameObject.SetActive(true);
        status = UserInterfaceWidgetStatus.BeginEnable;
        lerpColor = overlayImage.color;
        colourLerpTimer = 0.0F;
    }

    public void EndWidget()
    {
        status = UserInterfaceWidgetStatus.BeginDisable;
        lerpColor = overlayImage.color;
        colourLerpTimer = 0.0F;
    }

    public void RefreshWidget(Dictionary<string, object> args = null)
    {
        
    }
}
