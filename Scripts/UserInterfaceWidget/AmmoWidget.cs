using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Private fields.
    private UserInterfaceWidgetStatus status;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.Ammo;
    public UserInterfaceWidgetStatus Status => status;
    public GameObject WidgetGameObject => gameObject;

    public bool IsAutomatic => isAutomatic;

    // Public fields.
    [Header("Widget Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string widgetId;
    public bool isAutomatic;

    [Header("Stat Attributes")]
    public TextMeshProUGUI statTextbox;
    public TextMeshProUGUI maxStatTextbox;

    // Context methods.
    private void SetRandomWidgetId() => widgetId = Guid.NewGuid().ToString();

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    private void Start()
    {
        PlayerHighLogic.G.StatChanged += OnStatChanged;
    }

    private void OnDestroy()
    {
        if (PlayerHighLogic.G != null)
            PlayerHighLogic.G.StatChanged -= OnStatChanged;
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        gameObject.SetActive(true);
        status = UserInterfaceWidgetStatus.Enabled;

        if (statTextbox != null)
            statTextbox.text = $"{PlayerHighLogic.G.Ammo}";

        if (maxStatTextbox != null)
            maxStatTextbox.text = $"/{PlayerHighLogic.G.MaxAmmo}";
    }

    public void EndWidget()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    public void RefreshWidget(Dictionary<string, object> args = null) { }

    private void OnStatChanged(object sender, EventArgs e)
    {
        if (statTextbox != null)
            statTextbox.text = $"{PlayerHighLogic.G.Ammo}";

        if (maxStatTextbox != null)
            maxStatTextbox.text = $"/{PlayerHighLogic.G.MaxAmmo}";
    }

}
