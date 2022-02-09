using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class ItemController : MonoBehaviour
{
    private const float ITEM_PICKUP_RANGE = 0.375F;
    private const float BASE_VOLUME = 0.5F;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private GameMasterController master;
    private GameObject playerObject;
    private float distanceToPlayer = 0.0F;
    private AudioSource pickupAudioSource;
    private GameObject pickupFxObject;
    private float pickupAudioPitch;
    private float pickupAudioVolume;
    private GameObject itemPickupEventSource;

    [Header("Item Attributes")]
    [FormerlySerializedAs("item_data")]
    public GameItemData itemData;
    [FormerlySerializedAs("item_pickup_fx_prefab")]
    public GameObject pickupFxPrefab;
    [FormerlySerializedAs("item_pickup_fx_origin")]
    public Transform pickupFxOrigin;
    [FormerlySerializedAs("item_pickup_audio_clip")]
    public AudioClip sfxItemPickup;

    [Header("Event Attributes")]
    public GameObject pickupEventPrefab;
    

    void Start()
    {
        master = GameMasterController.Global;
        playerObject = GameMasterController.GlobalPlayerObject;

        // give item data default values if not set.

        if (itemData.group == string.Empty)
            itemData.group = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (itemData.code == string.Empty)
            itemData.code = this.gameObject.transform.position.ToString();

        // check if item is already collected,
        // destroy self if this is the case.

        if (master.dataController.GetIsItemCollected(itemData))
            Destroy(this.gameObject);

        // store original orientation

        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;

        // setup item pickup.

        if (pickupFxOrigin == null)
                pickupFxOrigin = this.transform;

        pickupAudioPitch = UnityEngine.Random.Range(0.95f, 1.05f);
        pickupAudioVolume = BASE_VOLUME * master.audioController.volumeItem;

        // start coroutine.

        StartCoroutine(UpdateStatus());
    }

    
    void Update()
    {
    }

    IEnumerator UpdateStatus()
    {
        while (true)
        {
            distanceToPlayer = Vector3.Distance(this.transform.position, playerObject.transform.position);

            if (distanceToPlayer < ITEM_PICKUP_RANGE)
            {
                pickupFxObject = GameObject.Instantiate(
                    pickupFxPrefab,
                    pickupFxOrigin.position,
                    pickupFxOrigin.rotation);

                pickupAudioSource = pickupFxObject.AddComponent<AudioSource>();
                pickupAudioSource.clip = sfxItemPickup;
                pickupAudioSource.pitch = pickupAudioPitch;
                pickupAudioSource.volume = pickupAudioVolume;
                pickupAudioSource.Play();

                master.dataController.UpdateItem(itemData);

                // if a game event is present, start the cutscene.

                if (pickupEventPrefab != null)
                {
                    itemPickupEventSource = Instantiate(
                        pickupEventPrefab,
                        this.transform.position,
                        originalRotation);

                    var gameEventTrigger = itemPickupEventSource.GetComponent<GameEventTrigger>();

                    if (gameEventTrigger == null)
                        yield return null;

                    gameEventTrigger.StartGameEvent();
                }

                // destroy the item.

                GameObject.Destroy(this.gameObject);
                GameObject.Destroy(pickupFxObject, 5);
            }

            yield return new WaitForSeconds(0.1F);
        }
    }
}
