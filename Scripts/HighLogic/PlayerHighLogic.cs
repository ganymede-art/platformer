using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class PlayerHighLogic : MonoBehaviour, IPersistenceLoadable
{
    // Private fields.
    private int health;
    private int maxHealth;
    private int oxygen;
    private int maxOxygen;
    private int ammo;
    private int maxAmmo;
    private int money;
    private int maxMoney;

    private int heldPrimaryItemCount;
    private int heldSecondaryItemCount;
    private int heldTertiaryItemCount;
    private int heldQuaternaryItemCount;

    private bool canDoubleJump;
    private bool canAttack;
    private bool canDiveUnderwater;
    private bool canAttackUnderwater;
    private bool canLunge;
    private bool canSlam;
    private bool canHighJump;
    private bool canShoot;

    private List<string> collectedItemIds;
    private List<string> collectedKeyItemIds;
    private List<string> heldKeyItemIds;
    private string selectedKeyItemId;

    // Public properties.
    public static PlayerHighLogic G => GameHighLogic.G?.PlayerHighLogic;

    public int Health => health;
    public int MaxHealth => maxHealth;
    public int Oxygen => oxygen;
    public int MaxOxygen => maxOxygen;
    public int Ammo => ammo;
    public int MaxAmmo => maxAmmo;
    public int Money => money;
    public int MaxMoney => maxMoney;

    public int HeldPrimaryItemCount => heldPrimaryItemCount;
    public int HeldSecondaryItemCount => heldSecondaryItemCount;
    public int HeldTertiaryItemCount => heldTertiaryItemCount;
    public int HeldQuaternaryItemCount => heldQuaternaryItemCount;

    public bool CanDoubleJump => canDoubleJump;
    public bool CanAttack => canAttack;
    public bool CanDiveUnderwater => canDiveUnderwater;
    public bool CanAttackUnderwater => canAttackUnderwater;
    public bool CanLunge => canLunge;
    public bool CanSlam => canSlam;
    public bool CanHighJump => canHighJump;
    public bool CanShoot => canShoot;

    public List<string> CollectedItemIds => collectedItemIds;
    public List<string> CollectedKeyItemIds => collectedKeyItemIds;
    public List<string> HeldKeyItemIds => heldKeyItemIds;
    public string SelectedKeyItemId => selectedKeyItemId;

    // Events.
    public event EventHandler ItemIdAdded;
    public event EventHandler KeyItemIdAdded;
    public event EventHandler KeyItemIdRemoved;
    public event EventHandler KeyItemSelected;
    public event EventHandler KeyItemDeselected;
    public event EventHandler StatChanged;

    private void Awake()
    {
        health = PLAYER_DEFAULT_HEALTH;
        maxHealth = PLAYER_DEFAULT_MAX_HEALTH;
        oxygen = PLAYER_DEFAULT_OXYGEN;
        maxOxygen = PLAYER_DEFAULT_MAX_OXYGEN;
        ammo = PLAYER_DEFAULT_AMMO;
        maxAmmo = PLAYER_DEFAULT_MAX_AMMO;
        money = PLAYER_DEFAULT_MONEY;
        maxMoney = PLAYER_DEFAULT_MAX_MONEY;

        collectedItemIds = new List<string>();
        collectedKeyItemIds = new List<string>();
        heldKeyItemIds = new List<string>();
        selectedKeyItemId = null;
    }

    public void ModifyHealth(int changeAmount)
    {
        health += changeAmount;
        health = Mathf.Clamp(health, 0, maxHealth);

        EventHandler handler = StatChanged;
        if (handler != null) handler(this, null);
    }

    public void ModifyAmmo(int changeAmount)
    {
        ammo += changeAmount;
        ammo = Mathf.Clamp(ammo, 0, maxAmmo);

        EventHandler handler = StatChanged;
        if (handler != null) handler(this, null);
    }

    public void ModifyMoney(int changeAmount)
    {
        money += changeAmount;
        money = Mathf.Clamp(money, 0, maxMoney);

        EventHandler handler = StatChanged;
        if (handler != null) handler(this, null);
    }

    public void ModifyOxygen(int changeAmount)
    {
        oxygen += changeAmount;
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);

        EventHandler handler = StatChanged;
        if (handler != null) handler(this, null);
    }

    public void ModifyMaxHealth(int changeAmount)
    {
        maxHealth += changeAmount;

        EventHandler handler = StatChanged;
        if (handler != null) handler(this, null);
    }

    public void ModifyMaxAmmo(int changeAmount)
    {
        maxAmmo += changeAmount;

        EventHandler handler = StatChanged;
        if (handler != null) handler(this, null);
    }
    public void ModifyMaxMoney(int changeAmount)
    {
        maxMoney += changeAmount;

        EventHandler handler = StatChanged;
        if (handler != null) handler(this, null);
    }
    public void ModifyMaxOxygen(int changeAmount)
    {
        maxOxygen += changeAmount;

        EventHandler handler = StatChanged;
        if (handler != null) handler(this, null);
    }

    public void ModifyCanAttack(bool canNowAttack) => canAttack = canNowAttack;
    public void ModifyCanDoubleJump(bool canNowDoubleJump) => canDoubleJump = canNowDoubleJump;
    public void ModifyCanDiveUnderwater(bool canNowDiveUnderwater) => canDiveUnderwater = canNowDiveUnderwater;
    public void ModifyCanAttackUnderwater(bool canNowAttackUnderwater) => canAttackUnderwater = canNowAttackUnderwater;
    public void ModifyCanLunge(bool canNowLunge) => canLunge = canNowLunge;
    public void ModifyCanSlam(bool canNowSlam) => canSlam = canNowSlam;
    public void ModifyCanHighJump(bool canNowHighJump) => canHighJump = canNowHighJump;
    public void ModifyCanShoot(bool canNowShoot) => canShoot = canNowShoot;

    public void AddItem(ItemType itemType, string itemId, int count)
    {
        Action<int> action = itemType switch
        {
            ItemType.Primary    => (x) => heldPrimaryItemCount    += x,
            ItemType.Secondary  => (x) => heldSecondaryItemCount  += x,
            ItemType.Tertiary   => (x) => heldTertiaryItemCount   += x,
            ItemType.Quaternary => (x) => heldQuaternaryItemCount += x,
            _ => (x) => heldQuaternaryItemCount += x,
        };
        action.Invoke(count);

        collectedItemIds.Add(itemId);
        EventHandler handler = ItemIdAdded;
        if (handler != null) handler(this, null);
    }

    public void AddKeyItem(string keyItemId)
    {
        collectedKeyItemIds.Add(keyItemId);
        heldKeyItemIds.Add(keyItemId);
        EventHandler handler = KeyItemIdAdded;
        if (handler != null) handler(this, null);
    }

    public void RemoveKeyItem(string keyItemId)
    {
        if (selectedKeyItemId == keyItemId)
            DeselectKeyItem();
        heldKeyItemIds.Remove(keyItemId);
        EventHandler handler = KeyItemIdRemoved;
        if (handler != null) handler(this, null);
    }

    public void SelectKeyItem(string keyItemId)
    {
        if(!heldKeyItemIds.Contains(keyItemId))
        {
            DeselectKeyItem();
            return;
        }
        selectedKeyItemId = keyItemId;
        KeyItemSelected?.Invoke(this, null);
    }

    public void DeselectKeyItem()
    {
        selectedKeyItemId = null;
        KeyItemDeselected?.Invoke(this, null);
    }

    public void LoadFromPersistence(PersistenceHighLogic.PersistenceInfo pi)
    {
        health = pi.health;
        maxHealth = pi.maxHealth;
        oxygen = pi.oxygen;
        maxOxygen = pi.maxOxygen;
        ammo = pi.ammo;
        maxAmmo = pi.maxAmmo;
        money = pi.money;
        maxMoney = pi.maxMoney;

        heldPrimaryItemCount = pi.heldPrimaryItemCount;
        heldSecondaryItemCount = pi.heldSecondaryItemCount;
        heldTertiaryItemCount = pi.heldTertiaryItemCount;
        heldQuaternaryItemCount = pi.heldQuaternaryItemCount;

        canDoubleJump = pi.canDoubleJump;
        canAttack = pi.canAttack;
        canDiveUnderwater = pi.canDiveUnderwater;
        canAttackUnderwater = pi.canAttackUnderwater;
        canLunge = pi.canLunge;
        canSlam = pi.canSlam;
        canHighJump = pi.canHighJump;
        canShoot = pi.canShoot;

        collectedItemIds = pi.collectedItemIds;
        collectedKeyItemIds = pi.collectedKeyItemIds;
        heldKeyItemIds = pi.heldKeyItemIds;
    }
}
