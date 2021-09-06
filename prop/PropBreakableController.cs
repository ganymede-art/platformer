using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class PropBreakableController : MonoBehaviour
{
    private GameMasterController master;

    public int prop_health = 1;

    public string damage_audio_clip_name = string.Empty;
    public string damage_particle_prefab_name = string.Empty;
    private GameObject damage_particle_prefab = null;

    public bool is_destroy_one_shot = false;
    public string destroy_one_shot_var_name = string.Empty;

    private AudioSource audio_source;

    private MeshRenderer renderer;
    private Collider collider;
    
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

        renderer = this.GetComponent<MeshRenderer>();
        collider = this.GetComponent<Collider>();

        damage_particle_prefab = master.resource_controller.GetParticlePrefab(damage_particle_prefab_name);

        audio_source = this.gameObject.AddComponent<AudioSource>();
        audio_source.spatialBlend = 1;
        audio_source.clip = master.audio_controller.GetAudioClip(damage_audio_clip_name);
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
        // play the breaking sound.

        audio_source.Play();

        // if the object is the player itself, repel them.

        if (other.transform.root.gameObject.name == GameConstants.NAME_PLAYER)
            GameMasterController.GetPlayerController().HandleRepelObject(this.gameObject);

        // create the particle prefab.

        if (damage_particle_prefab != null)
            damage_particle_prefab = Instantiate(damage_particle_prefab, this.transform);

        // handle damage, destroy if out of health.

        var damage_type = other.gameObject.GetComponent<ActorAttributeDamageType>()?.damage_type;
        if (damage_type == null)
            damage_type = AttributeDamageTypeData.GetDefault();

        prop_health -= damage_type.damage_amount;

        if (prop_health <= 0)
        {
            if (is_destroy_one_shot)
                master.data_controller.UpdateGameVar(destroy_one_shot_var_name, true);

            renderer.enabled = false;
            collider.enabled = false;

            Destroy(this.gameObject, 5);
        }
    }
}
