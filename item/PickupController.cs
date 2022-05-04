using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;
using static Assets.Script.GameConstants;

public class PickupController : MonoBehaviour
{
    private const float ITEM_PICKUP_RANGE = 0.375F;
    private const float BASE_VOLUME = 0.5F;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private GameObject playerObject;
    private float distanceToPlayer = 0.0F;
    private AudioSource pickupAudioSource;
    private GameObject pickupFxObject;
    private float pickupAudioPitch;
    private float pickupAudioVolume;
    private GameObject itemPickupEventSource;

    private Rigidbody propRigidBody;

    [Header("Pickup Attributes")]
    public GameObject pickupFxPrefab;
    public Transform pickupFxOrigin;
    public AudioClip pickupSound;

    [Header("Pickup Effect Attributes")]
    public bool doAffectPlayerHealth;
    public int playerHealthChange;
    public bool doAffectPlayerAmmo;
    public int playerAmmoChange;
    public bool doAffectPlayerMoney;
    public int playerMoneyChange;

    [Header("Physics Attributes")]
    public bool hasPhysics;
    public GameObject rigidBodyObject;
    public float xForceMin;
    public float xForceMax;
    public float yForceMin;
    public float yForceMax;
    public float zForceMin;
    public float zForceMax;

    void Start()
    {
        playerObject = GameMasterController.GlobalPlayerObject;

        // setup item pickup.

        if (pickupFxOrigin == null)
            pickupFxOrigin = this.transform;

        pickupAudioPitch = UnityEngine.Random.Range(0.95f, 1.05f);
        pickupAudioVolume = BASE_VOLUME * GameSettingsController.Global.volumeProp;

        // start coroutine.

        StartCoroutine(UpdateStatus());

        // physics.

        if (hasPhysics)
            StartPhysiscs();
    }

    private void StartPhysiscs()
    {
        propRigidBody = rigidBodyObject.GetComponent<Rigidbody>();
        float xForce = UnityEngine.Random.Range(xForceMin, xForceMax);
        float yForce = UnityEngine.Random.Range(yForceMin, yForceMax);
        float zForce = UnityEngine.Random.Range(zForceMin, zForceMax);
        Vector3 force = new Vector3(xForce, yForce, zForce);
        propRigidBody.AddForce(force, ForceMode.VelocityChange);
    }

    IEnumerator UpdateStatus()
    {
        while(true)
        {
            if(GameMasterController.Global.gameState != GAME_STATE_GAME)
                yield return new WaitForSeconds(0.1F);

            distanceToPlayer = Vector3.Distance(this.transform.position, playerObject.transform.position);

            if (distanceToPlayer < ITEM_PICKUP_RANGE)
            {
                // store original orientation

                originalPosition = this.transform.position;
                originalRotation = this.transform.rotation;

                // inst fx.

                pickupFxObject = GameObject.Instantiate(
                    pickupFxPrefab,
                    pickupFxOrigin.position,
                    pickupFxOrigin.rotation);

                pickupAudioSource = pickupFxObject.AddComponent<AudioSource>();
                pickupAudioSource.clip = pickupSound;
                pickupAudioSource.pitch = pickupAudioPitch;
                pickupAudioSource.volume = pickupAudioVolume;
                pickupAudioSource.Play();

                // apply pickup effects.

                if (doAffectPlayerHealth)
                    GamePlayerController.Global.ModifyPlayerHealth(playerHealthChange);

                if (doAffectPlayerAmmo)
                    GamePlayerController.Global.ModifyPlayerAmmo(playerAmmoChange);

                if (doAffectPlayerMoney)
                    GamePlayerController.Global.ModifyPlayerMoney(playerMoneyChange);

                // destroy the item.

                GameObject.Destroy(this.gameObject);
                GameObject.Destroy(pickupFxObject, 5);
            }

            yield return new WaitForSeconds(0.1F);
        }
    }
}
