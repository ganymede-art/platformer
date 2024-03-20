using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class SelectedKeyItemWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Private fields.
    private UserInterfaceWidgetStatus status;
    private List<GameObject> keyItemObjects;

    // Public properties.
    public string WidgetId => widgetId;
    public UserInterfaceWidgetType WidgetType => UserInterfaceWidgetType.KeyItems;
    public UserInterfaceWidgetStatus Status => status;
    public GameObject WidgetGameObject => gameObject;
    public bool IsAutomatic => isAutomatic;

    // Public fields.
    [Header("Widget Attributes")]
    [ContextMenuItem("Set Random Id", "SetRandomWidgetId")]
    public string widgetId;
    public bool isAutomatic;

    [Header("Selected Key Item Attributes")]
    public Image selectedKeyItemImage;
    public Sprite noneSprite;
    public TextMeshProUGUI selectedKeyItemTextbox;

    // Context methods.
    private void SetRandomWidgetId() => widgetId = Guid.NewGuid().ToString();

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    private void Start()
    {
        PlayerHighLogic.G.KeyItemSelected += OnKeyItemSelected;
        PlayerHighLogic.G.KeyItemDeselected += OnKeyItemDeselected;
    }

    private void OnDestroy()
    {
        if(PlayerHighLogic.G != null)
        {
            PlayerHighLogic.G.KeyItemDeselected -= OnKeyItemDeselected;
            PlayerHighLogic.G.KeyItemSelected -= OnKeyItemSelected;
        }
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        status = UserInterfaceWidgetStatus.Enabled;
        UpdateSelectedKeyItemImage();
        UpdateSelectedKeyItemTextbox();
    }

    public void RefreshWidget(Dictionary<string, object> args = null) { }

    public void EndWidget()
    {
        status = UserInterfaceWidgetStatus.Disabled;
    }

    private void OnKeyItemSelected(object sender, EventArgs e)
    {
        UpdateSelectedKeyItemImage();
        UpdateSelectedKeyItemTextbox();
    }

    private void OnKeyItemDeselected(object sender, EventArgs e)
    {
        UpdateSelectedKeyItemImage();
        UpdateSelectedKeyItemTextbox();
    }

    private void UpdateSelectedKeyItemImage()
    {
        if (selectedKeyItemImage == null)
            return;

        string keyItemId = PlayerHighLogic.G.SelectedKeyItemId;

        if (keyItemId == null)
        {
            selectedKeyItemImage.sprite = noneSprite;
        }
        else
        {
            string iconId = $"KeyItem{keyItemId}Icon";
            string icon = TextsHighLogic.G.GetText(iconId) ?? keyItemId;

            var sprite = AssetsHighLogic.G.KeyItemSprites.FirstOrDefault(x => x.name == icon);
            selectedKeyItemImage.sprite = sprite;
        }
    }

    private void UpdateSelectedKeyItemTextbox()
    {
        if (selectedKeyItemTextbox == null)
            return;

        string keyItemId = PlayerHighLogic.G.SelectedKeyItemId;

        string name = string.Empty;
        string description = string.Empty;

        if (keyItemId == null)
        {
            name = TextsHighLogic.G.GetText(LOC_MISSING_KEY_ITEM_NAME);
            description = TextsHighLogic.G.GetText(LOC_MISSING_KEY_ITEM_DESCRIPTION);
        }
        else
        {
            string nameId = $"KeyItem{keyItemId}Name";
            name = TextsHighLogic.G.GetText(nameId)
                ?? keyItemId;

            string descriptionId = $"KeyItem{keyItemId}Description";
            description = TextsHighLogic.G.GetText(descriptionId)
                ?? keyItemId;
        }

        selectedKeyItemTextbox.text = $"<color=yellow>{name}</color>: {description}";
    }
}
