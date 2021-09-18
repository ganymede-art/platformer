using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class PropBreakableController : MonoBehaviour
{
    private const float BASE_VOLUME = 1f;

    private GameMasterController master;

    public int prop_health = 1;

    public bool is_destroy_one_shot = false;
    public string destroy_one_shot_var_name = string.Empty;
    
    private GameObject prop_destroy_fx_object;
    private AudioSource prop_destroy_audio_source;
    private float item_pickup_audio_pitch;
    private float item_pickup_audio_volume;

    public GameObject prop_destroy_fx_prefab;
    public Transform prop_destroy_fx_origin;
    public AudioClip prop_destroy_audio_clip;

    void Start()
    {
        master = GameMasterController.GetMasterController();

        // destroy if already saved as destroyed.

        if(is_destroy_one_shot)
        {
            if (master.data_controller.GetGameVarBool(destroy_one_shot_var_name))
                Destroy(this.gameObject);
        }

        // otherwise set up.

        if (prop_destroy_fx_origin == null)
            prop_destroy_fx_origin = this.transform;

        item_pickup_audio_pitch = Random.Range(0.95f, 1.05f);
        item_pickup_audio_volume = BASE_VOLUME * master.audio_controller.volume_object;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameConstants.TAG_PLAYER_DAMAGE_OBJECT)
        {
            HandleDamageObject(other);
        }
    }

    private void HandleDamageObject(Collider other)
    {
        // if the object is the player itself, repel them.

        if (other.transform.root.gameObject.name == GameConstants.NAME_PLAYER)
            GameMasterController.GetPlayerController().HandleRepelObject(this.gameObject);

        // handle damage, destroy if out of health.

        var damage_type = other.gameObject.GetComponent<ActorAttributeDamageType>()?.damage_type;
        if (damage_type == null)
            damage_type = AttributeDamageTypeData.GetDefault();

        prop_health -= damage_type.damage_amount;

        if (prop_health <= 0)
        {
            if (is_destroy_one_shot)
                master.data_controller.UpdateGameVar(destroy_one_shot_var_name, true);

            prop_destroy_fx_object = Instantiate(
                prop_destroy_fx_prefab, 
                prop_destroy_fx_origin.position, 
                prop_destroy_fx_origin.rotation);

            prop_destroy_audio_source = prop_destroy_fx_object.AddComponent<AudioSource>();
            prop_destroy_audio_source.clip = prop_destroy_audio_clip;
            prop_destroy_audio_source.pitch = item_pickup_audio_pitch;
            prop_destroy_audio_source.volume = item_pickup_audio_volume;
            prop_destroy_audio_source.Play();

            Destroy(this.gameObject);
            Destroy(prop_destroy_fx_object, 5);
        }
    }
}
