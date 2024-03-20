using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class KeyItem : MonoBehaviour
{
    // Consts.
    private float COLLECTING_INTERVAL = 0.5F;
    private static readonly Vector3 COLLECTING_START_OFFSET = new Vector3(0.0F, 0.0F, 0.0F);
    private static readonly Vector3 COLLECTING_FINISH_OFFSET = new Vector3(0.0F, 1.0F, 0.0F);

    // Private fields.
    private ItemStatus status;
    private float collectingTimer;
    private float collectingProgress;

    // Public fields.
    public KeyItemIdConstant keyItemId;

    [Header("Item Attributes")]
    public GameObject rendererContainerObject;
    [Space]
    public GameObject onCollectingFxPrefab;
    public GameObject onCollectedFxPrefab;

    private void Awake()
    {
        status = ItemStatus.None;
    }

    void Start()
    {
        // Destroy if already collected.
        if (PlayerHighLogic.G.CollectedKeyItemIds.Contains(keyItemId.KeyItemId))
        {
            Destroy(gameObject);
            return;
        }

        // Initialise as not collected.
        status = ItemStatus.NotCollected;
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if (status != ItemStatus.Collecting)
            return;

        collectingProgress = collectingTimer / COLLECTING_INTERVAL;
        float collectingLerp = Mathf.SmoothStep(0.0F, 1.0F, collectingProgress);
        rendererContainerObject.transform.position = Vector3.Lerp
            ( transform.position + COLLECTING_START_OFFSET
            , transform.position + COLLECTING_FINISH_OFFSET
            , collectingLerp);
        rendererContainerObject.transform.localScale = Vector3.Lerp
            ( Vector3.one
            , Vector3.zero
            , collectingLerp);

        if(collectingTimer >= COLLECTING_INTERVAL)
        {
            status = ItemStatus.Collected;

            if(onCollectedFxPrefab != null)
                Instantiate(onCollectedFxPrefab, transform.position + COLLECTING_FINISH_OFFSET, transform.rotation);

            GameObject.Destroy(gameObject);
        }

        collectingTimer += Time.deltaTime;
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

        PlayerHighLogic.G.AddKeyItem(keyItemId.KeyItemId);

        status = ItemStatus.Collecting;

        if (onCollectingFxPrefab != null)
            Instantiate(onCollectingFxPrefab, transform.position + COLLECTING_START_OFFSET, transform.rotation);
    }
}
