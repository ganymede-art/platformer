using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TimeWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Private fields.
    private UserInterfaceWidgetStatus status;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.Time;
    public UserInterfaceWidgetStatus Status => status;
    public GameObject WidgetGameObject => gameObject;
    public bool IsAutomatic => isAutomatic;

    // Public fields.
    [Header("Widget Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string widgetId;
    public bool isAutomatic;

    [Header("Time Attributes")]
    public TextMeshProUGUI timeTextbox;

    // Context methods.
    private void SetRandomWidgetId() => widgetId = Guid.NewGuid().ToString();

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        gameObject.SetActive(true);
        status = UserInterfaceWidgetStatus.Enabled;
        var timespan = TimeSpan.FromHours(TimeHighLogic.G.Hour);
        timeTextbox.text 
            = $"Hour {timespan.Hours:00}:{timespan.Minutes:00}, "
            + $"Day {TimeHighLogic.G.Day}, "
            + $"{TimeHighLogic.G.DayOfWeek}";
    }

    public void EndWidget()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    public void RefreshWidget(Dictionary<string, object> args = null) { }
}
