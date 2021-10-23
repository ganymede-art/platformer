using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;
using static Assets.script.prop.PropConstants;
using static Assets.script.AttributeDataClasses;

public class PropBreakableController : MonoBehaviour
{
    private const float BASE_VOLUME = 1f;

    private GameMasterController master;

    private GameObject destroyFxObject;
    private AudioSource destroyAudioSource;
    private float pickupAudioPitch;
    private float pickupAudioVolume;

    [Header("Prop Attributes")]
    public int health;

    [Header("Destroy Attributes")]
    public bool isDestroyOneShot;
    public string destroyOneShotVarName;

    public GameObject destroyFxPrefab;
    public Transform destroyFxOrigin;

    public float destroyRepelForceMultiplier;

    [Header("Damage Attributes")]
    public GameObject damageFxPrefab;
    public Transform damageFxOrigin;

    public float damageRepelForceMultiplier;

    [Header("Sound Effects")]
    public AudioClip sfxDestroy;
    public AudioClip sfxDamage;

    void Start()
    {
        master = GameMasterController.GlobalMasterController;

        // destroy if already saved as destroyed.

        if(isDestroyOneShot)
        {
            if (master.dataController.GetGameVarBool(destroyOneShotVarName))
                Destroy(this.gameObject);
        }

        // otherwise set up.

        if (destroyFxOrigin == null)
            destroyFxOrigin = this.transform;

        if (destroyRepelForceMultiplier < PROP_REPEL_FORCE_MIN)
            destroyRepelForceMultiplier = PROP_REPEL_FORCE_MIN;

        pickupAudioPitch = Random.Range(0.95f, 1.05f);
        pickupAudioVolume = BASE_VOLUME * master.audioController.volumeObject;

        // validation.

        if (health < PROP_HEALTH_MIN)
            health = PROP_HEALTH_MIN;

        if (damageFxPrefab == null)
            damageFxPrefab = destroyFxPrefab;

        if (damageFxOrigin == null)
            damageFxOrigin = destroyFxOrigin;

        if (damageRepelForceMultiplier < PROP_REPEL_FORCE_MIN)
            damageRepelForceMultiplier = PROP_REPEL_FORCE_MIN;

        if (sfxDamage == null)
            sfxDamage = sfxDestroy;
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
        // handle damage, destroy if out of health.

        var damageType = other.gameObject.GetComponent<AttributeDamageController>()?.data;
        if (damageType == null)
            damageType = AttributeDamageData.GetDefault();

        health -= damageType.damageAmount;

        if (health <= 0)
        {
            if (isDestroyOneShot)
                master.dataController.UpdateGameVar(destroyOneShotVarName, true);

            // create destroy fx.

            destroyFxObject = Instantiate(
                destroyFxPrefab, 
                destroyFxOrigin.position, 
                destroyFxOrigin.rotation);

            destroyAudioSource = destroyFxObject.AddComponent<AudioSource>();
            destroyAudioSource.clip = sfxDestroy;
            destroyAudioSource.pitch = pickupAudioPitch;
            destroyAudioSource.volume = pickupAudioVolume;
            destroyAudioSource.Play();

            Destroy(this.gameObject);
            Destroy(destroyFxObject, 5);

            // if the collidng object is the player itself, repel them.

            if (other.transform.root.gameObject.name == GameConstants.NAME_PLAYER)
                GameMasterController.GlobalPlayerController.SimpleRepel(this.gameObject, destroyRepelForceMultiplier);

        }
        else
        {
            // create damage fx.

            destroyFxObject = Instantiate(
                damageFxPrefab, 
                damageFxOrigin.position,
                damageFxOrigin.rotation);

            destroyAudioSource = destroyFxObject.AddComponent<AudioSource>();
            destroyAudioSource.clip = sfxDamage;
            destroyAudioSource.pitch = pickupAudioPitch;
            destroyAudioSource.volume = pickupAudioVolume;
            destroyAudioSource.Play();

            Destroy(destroyFxObject, 5);

            // if the colliding object is the player itself, repel them.

            if (other.transform.root.gameObject.name == GameConstants.NAME_PLAYER)
                GameMasterController.GlobalPlayerController.SimpleRepel(this.gameObject, damageRepelForceMultiplier);

        }
    }
}
