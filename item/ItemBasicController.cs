using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class ItemBasicController : MonoBehaviour
{
    private const float ITEM_PICKUP_RANGE = 0.375f;
    private const float BASE_VOLUME = 0.5f;

    private GameMasterController master;
    private GameObject player_object;
    private float distance_to_player = 0.0f;
    private AudioSource item_pickup_audio_source;
    private GameObject item_pickup_fx_object;
    private float item_pickup_audio_pitch;
    private float item_pickup_audio_volume;

    public GameItemData item_data;
    public GameObject item_pickup_fx_prefab;
    public Transform item_pickup_fx_origin;
    public AudioClip item_pickup_audio_clip;
    

    void Start()
    {
        master = GameMasterController.GetMasterController();
        player_object = GameMasterController.GetPlayerObject();

        if (item_pickup_fx_origin == null)
                item_pickup_fx_origin = this.transform;

        item_pickup_audio_pitch = Random.Range(0.95f, 1.05f);
        item_pickup_audio_volume = BASE_VOLUME * master.audio_controller.volume_item;
    }

    
    void Update()
    {
        distance_to_player = Vector3.Distance(this.transform.position, player_object.transform.position);

        if (distance_to_player < ITEM_PICKUP_RANGE)
        {
            item_pickup_fx_object = GameObject.Instantiate(
                item_pickup_fx_prefab,
                item_pickup_fx_origin.position,
                item_pickup_fx_origin.rotation);

            item_pickup_audio_source = item_pickup_fx_object.AddComponent<AudioSource>();
            item_pickup_audio_source.clip = item_pickup_audio_clip;
            item_pickup_audio_source.pitch = item_pickup_audio_pitch;
            item_pickup_audio_source.volume = item_pickup_audio_volume;
            item_pickup_audio_source.Play();

            GameObject.Destroy(this.gameObject);
            GameObject.Destroy(item_pickup_fx_object, 5);
        }
    }
}
