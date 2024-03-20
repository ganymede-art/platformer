using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Private fields.
    private UserInterfaceWidgetStatus status;
    private float translateLerpTimer;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.Oxygen;
    public UserInterfaceWidgetStatus Status => status;
    public GameObject WidgetGameObject => gameObject;

    public bool IsAutomatic => isAutomatic;

    // Public fields.
    [Header("Widget Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string widgetId;
    public bool isAutomatic;
    [Space]
    public TranslateLerpWidget lerpWidget;
    public TextMeshProUGUI timerTextbox;
    

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
        TimerHighLogic.G.TimerAdded += OnTimerAdded;
        TimerHighLogic.G.TimerUpdated += OnTimerUpdated;
        TimerHighLogic.G.TimerCompleted += OnTimerCompleted;
    }

    private void OnDestroy()
    {
        if(TimerHighLogic.G != null)
        {
            TimerHighLogic.G.TimerAdded -= OnTimerAdded;
            TimerHighLogic.G.TimerUpdated -= OnTimerUpdated;
            TimerHighLogic.G.TimerCompleted -= OnTimerUpdated;
        }
    }

    private void OnTimerAdded(object sender, TimerArgs args)
    {
        if(lerpWidget.Status == UserInterfaceWidgetStatus.Disabled)
            lerpWidget.BeginWidget();
        timerTextbox.text = $"<mspace=0.5em>{args.timerValue.ToString("0.0")}";
    }

    private void OnTimerUpdated(object sender, TimerArgs args)
    {
        if (lerpWidget.Status == UserInterfaceWidgetStatus.Disabled)
            lerpWidget.BeginWidget();
        timerTextbox.text = $"<mspace=0.5em>{args.timerValue.ToString("0.0")}";
    }

    private void OnTimerCompleted(object sender, TimerArgs args)
    {
        lerpWidget.EndWidget();
        timerTextbox.text = $"0";
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        gameObject.SetActive(true);
    }

    public void EndWidget() { }

    public void RefreshWidget(Dictionary<string, object> args = null) { }
}
