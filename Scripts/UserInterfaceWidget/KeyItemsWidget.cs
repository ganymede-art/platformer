using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Constants;
using System.Linq;

public class KeyItemsWidget : MonoBehaviour, IUserInterfaceWidget
{
    // Consts.
    private float MIN_SFX_PITCH = 0.9F;
    private float MAX_SFX_PITCH = 1.1F;

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

    [Header("Key Items Attributes")]
    public VerticalLayoutGroup keyItemVerticalLayout;

    [Header("Button Attributes")]
    public GameObject keyItemButtonPrefab;
    public AudioSource navigateAudioSource;
    public AudioSource submitAudioSource;

    // Context methods.
    private void SetRandomWidgetId() => widgetId = Guid.NewGuid().ToString();

    private void Awake()
    {
        status = UserInterfaceWidgetStatus.Disabled;
        keyItemObjects = new List<GameObject>();
    }

    private GameObject GetNewKeyItemObject(string keyItemId)
    {
        var newKeyItemObject = Instantiate(keyItemButtonPrefab,keyItemVerticalLayout.transform);

        string iconId = $"KeyItem{keyItemId}Icon";
        string icon = TextsHighLogic.G.GetText(iconId) ?? keyItemId;

        var sprite = AssetsHighLogic.G.KeyItemSprites.FirstOrDefault(x => x.name == icon);

        string nameId = $"KeyItem{keyItemId}Name";
        string name = TextsHighLogic.G.GetText(nameId)
            ?? keyItemId;

        var newKeyItemWidget = newKeyItemObject.GetComponent<KeyItemWidget>();

        var newKeyItemTextbox = newKeyItemWidget.keyItemTextbox;
        var newKeyItemImage = newKeyItemWidget.keyItemImage;
        var newKeyItemButton = newKeyItemWidget.keyItemButton;
        var newKeyItemSelectable = newKeyItemWidget.keyItemSelectEvent;

        newKeyItemTextbox.text = name;
        newKeyItemImage.sprite = sprite;
        newKeyItemButton.onClick.AddListener(() => OnKeyItemButtonClick(keyItemId));
        newKeyItemSelectable.OnSelection += (s, e) => OnKeyItemButtonSelect(keyItemId);
        newKeyItemSelectable.OnDeselection += (s, e) => OnKeyItemButtonDeselect(keyItemId);

        return newKeyItemObject;
    }

    private void DestroyAllKeyItemObjects()
    {
        for(int i = 0; i < keyItemObjects.Count; i++)
        {
            GameObject.Destroy(keyItemObjects[i]);
        }
        keyItemObjects.Clear();
    }

    private void OnKeyItemButtonClick(string keyItemId)
    {
        if (PlayerHighLogic.G.SelectedKeyItemId == keyItemId)
            PlayerHighLogic.G.DeselectKeyItem();
        else
            PlayerHighLogic.G.SelectKeyItem(keyItemId);

        submitAudioSource.PlayPitchedOneShot
            ( submitAudioSource.clip
            , SettingsHighLogic.G.UserInterfaceVolume
            , MIN_SFX_PITCH
            , MAX_SFX_PITCH);
    }

    private void OnKeyItemButtonSelect(string keyItemId)
    {
        navigateAudioSource.PlayPitchedOneShot
            (navigateAudioSource.clip
            , SettingsHighLogic.G.UserInterfaceVolume
            , MIN_SFX_PITCH
            , MAX_SFX_PITCH);
    }

    private void OnKeyItemButtonDeselect(string keyItemId)
    {
    }

    public void BeginWidget(Dictionary<string, object> args = null)
    {
        status = UserInterfaceWidgetStatus.Enabled;

        for (int i = 0; i < PlayerHighLogic.G.HeldKeyItemIds.Count; i++)
        {
            string newKeyItemId = PlayerHighLogic.G.HeldKeyItemIds[i];
            var newKeyItemObject = GetNewKeyItemObject(newKeyItemId);
            keyItemObjects.Add(newKeyItemObject);
        }
    }

    public void RefreshWidget(Dictionary<string, object> args = null)
    {

    }

    public void EndWidget()
    {
        status = UserInterfaceWidgetStatus.Disabled;

        DestroyAllKeyItemObjects();
    }
}
