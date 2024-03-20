using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class PushableProp : MonoBehaviour
{
    // Consts.
    private const float STILL_FRICTION = 1.0F;

    // Private fields.
    private IRemoteTrigger remoteTrigger;
    private float statusTimer;
    private PropStatus previousStatus;
    private PropStatus activeStatus;

    // Public fields.
    [Header("Pushable Attributes")]
    public Rigidbody propRigidBody;
    public Collider propCollider;
    public GameObject remoteTriggerObject;
    [Space]
    public bool canPlayerPush;
    [Space]
    public float coastInterval;
    public float coastForce;
    [Header("Sound Attributes")]
    public AudioSource coastingAudioSource;

    private void Awake()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEnter;
    }

    void Start()
    {
        activeStatus = PropStatus.Resting;
        previousStatus = PropStatus.Resting;
        ChangeStatus(PropStatus.Resting);
    }

    void Update()
    {
        if (activeStatus == PropStatus.Resting)
        {
        }
        else if (activeStatus == PropStatus.Coasting)
        {
            float coastingProgress = Mathf.InverseLerp(0, coastInterval, statusTimer);
            float coastingLerp = Mathf.SmoothStep(0.0F, STILL_FRICTION, coastingProgress);
            propCollider.material.dynamicFriction = coastingLerp;
            propCollider.material.staticFriction = coastingLerp;
        }

        statusTimer += Time.deltaTime;
    }

    public void Push()
    {
        propRigidBody.AddForce
            ( ActiveSceneHighLogic.G.CachedPlayer.playerDirectionObject.transform.forward * coastForce
            , ForceMode.VelocityChange);
        ChangeStatus(PropStatus.Coasting);
    }

    public void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        if (args.other.gameObject.layer != LAYER_HITBOX)
            return;

        var hitboxData = ActiveSceneHighLogic.G.HitboxDatas[args.other.gameObject];

        if (hitboxData.damageType.DamageType == DamageType.Player
            && canPlayerPush)
        {
            Push();
        }
    }

    private void ChangeStatus(PropStatus newStatus)
    {
        EndStatus();
        previousStatus = activeStatus;
        activeStatus = newStatus;
        statusTimer = 0.0F;
        BeginStatus();
    }

    private void BeginStatus()
    {
        if (activeStatus == PropStatus.Resting)
        {
            propCollider.material.frictionCombine = PhysicMaterialCombine.Average;
            propCollider.material.dynamicFriction = STILL_FRICTION;
            propCollider.material.staticFriction = STILL_FRICTION;
        }
        else if (activeStatus == PropStatus.Coasting)
        {
            propCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
            propCollider.material.dynamicFriction = 0.0F;
            propCollider.material.staticFriction = 0.0F;

            if (coastingAudioSource != null)
                coastingAudioSource.PlayPitchedOneShot
                    ( coastingAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);
        }
    }

    private void EndStatus()
    {
        if (activeStatus == PropStatus.Resting)
        {
        }
        else if (activeStatus == PropStatus.Coasting)
        {
        }
    }
}
