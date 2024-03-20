using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Private fields.
    private UserInterfaceWidgetStatus status;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.Item;
    public UserInterfaceWidgetStatus Status => status;
    public GameObject WidgetGameObject => gameObject;

    public bool IsAutomatic => isAutomatic;

    // Public fields.
    [Header("Widget Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string widgetId;
    public bool isAutomatic;

    [Header("Stat Attributes")]
    public TextMeshProUGUI itemTextbox;
    public ItemTypeConstant itemType;

    // Context methods.
    private void SetRandomWidgetId() => widgetId = Guid.NewGuid().ToString();

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    private void Start()
    {
        PlayerHighLogic.G.ItemIdAdded += OnItemIdAdded;
    }

    private void OnDestroy()
    {
        if (PlayerHighLogic.G != null)
            PlayerHighLogic.G.ItemIdAdded -= OnItemIdAdded;
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        gameObject.SetActive(true);
        status = UserInterfaceWidgetStatus.Enabled;

        int itemCount = itemType.ItemType switch
        {
            ItemType.Primary => PlayerHighLogic.G.HeldPrimaryItemCount,
            ItemType.Secondary => PlayerHighLogic.G.HeldSecondaryItemCount,
            ItemType.Tertiary => PlayerHighLogic.G.HeldTertiaryItemCount,
            ItemType.Quaternary => PlayerHighLogic.G.HeldQuaternaryItemCount,
            _ => PlayerHighLogic.G.HeldPrimaryItemCount,
        };
        if (itemTextbox != null)
            itemTextbox.text = $"{itemCount}";
    }

    public void EndWidget()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    public void RefreshWidget(Dictionary<string, object> args = null) { }

    private void OnItemIdAdded(object sender, EventArgs e)
    {
        int itemCount = itemType.ItemType switch
        {
            ItemType.Primary => PlayerHighLogic.G.HeldPrimaryItemCount,
            ItemType.Secondary => PlayerHighLogic.G.HeldSecondaryItemCount,
            ItemType.Tertiary => PlayerHighLogic.G.HeldTertiaryItemCount,
            ItemType.Quaternary => PlayerHighLogic.G.HeldQuaternaryItemCount,
            _ => PlayerHighLogic.G.HeldPrimaryItemCount,
        };
        if (itemTextbox != null)
            itemTextbox.text = $"{itemCount}";
    }

}
