using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserInterfaceWidget
{
    public string WidgetId { get; }
    public UserInterfaceWidgetType WidgetType { get; }
    public UserInterfaceWidgetStatus Status { get; }
    public GameObject WidgetGameObject { get; }
    public bool IsAutomatic { get; }
    public void BeginWidget(Dictionary<string, object> args = null);
    public void RefreshWidget(Dictionary<string, object> args = null);
    public void EndWidget();
}
