using System;
using UnityEngine;
using static Constants;

public class Item : MonoBehaviour, INameable, IProp
{
    // Private fields.
    private ItemStatus status;

    // Public fields.
    [Header("Item Attributes")]
    public string itemId;
    public int itemCount;
    public ItemTypeConstant itemType;
    [Space]
    public GameObject addActionTriggerPrefab;
    public GameObject fxPrefab;

    // Public properties.
    public PropStatus ActiveStatus => PropStatus.Uncollected;
    public PropStatus PreviousStatus => PropStatus.Collected;
    public GameObject PropObject => gameObject;

    // Events.
    public event EventHandler<PropArgs> StatusChanged;

    private void Awake()
    {
        status = ItemStatus.None;
    }

    private void Start()
    {
        // Destroy if already collected.
        if(PlayerHighLogic.G.CollectedItemIds.Contains(itemId))
        {
            GameObject.Destroy(gameObject);
            return;
        }

        // Initialise as not collected.
        status = ItemStatus.NotCollected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != TRANSFORM_NAME_PLAYER_COLLIDER)
            return;

        CollectItem();
    }

    private void CollectItem()
    {
        if (status != ItemStatus.NotCollected)
            return;

        PlayerHighLogic.G.AddItem(itemType.ItemType, itemId, itemCount);
        if(addActionTriggerPrefab != null)
        {
            var addActionTriggerObject = Instantiate(addActionTriggerPrefab, transform.position, transform.rotation);
            var addActionTrigger = addActionTriggerObject.GetComponent<AddActionHighLogicTrigger>();
            if(addActionTrigger == null)
                goto addActionTriggerIsNull;
            addActionTrigger.AddAction();

        } addActionTriggerIsNull:

        if (fxPrefab != null)
            Instantiate(fxPrefab, transform.position, transform.rotation);

        // Invoke prop event hooks.
        var args = new PropArgs();
        args.activeStatus = PropStatus.Collected;
        args.previousStatus = PropStatus.Uncollected;
        StatusChanged?.Invoke(this, args);

        GameObject.Destroy(gameObject);
    }

    public string GetName() => $"Item{itemType.ItemType}";
}
