using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class UnstablePlatformProp : MonoBehaviour, IProp
{
    // Consts.
    private const float MIN_SFX_PITCH = 0.9F;
    private const float MAX_SFX_PITCH = 1.1F;

    // Private fields.
    private PropArgs args;
    private float statusTimer;
    private PropStatus activeStatus;
    private PropStatus previousStatus;
    private IRemoteTrigger remoteTrigger;
    private int pressure;

    // Public properties.
    public PropStatus ActiveStatus => activeStatus;
    public PropStatus PreviousStatus => previousStatus;
    public GameObject PropObject => gameObject;

    // Public fields.
    public GameObject remoteTriggerObject;
    public Rigidbody platformRigidBody;
    [Space]
    public float shakingInterval;
    public float fallingInterval;
    public float recoveryInterval;
    [Space]
    public float shakingDistance;
    public float shakingSpeed;
    [Space]
    public Transform fallingStartTransform;
    public Transform fallingFinishTransform;
    [Space]
    public AudioSource shakingAudioSource;
    public AudioSource fallingAudioSource;
    [Space]
    public bool isOneShot;

    // Events.
    public event EventHandler<PropArgs> StatusChanged;

    private void Awake()
    {
        args = new PropArgs();
    }

    private void Start()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEntered;
        remoteTrigger.RemoteTriggerExited += OnRemoteTriggerExited;

        activeStatus = PropStatus.Resting;
        previousStatus = PropStatus.Resting;
        ChangeStatus(PropStatus.Resting);
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if(activeStatus == PropStatus.Resting)
        {
            if(pressure > 0)
            {
                ChangeStatus(PropStatus.Shaking);
                return;
            }
        }
        else if(activeStatus == PropStatus.Shaking)
        {
            float xOffset = Mathf.Cos(statusTimer * shakingSpeed) * shakingDistance;
            float zOffset = Mathf.Sin(statusTimer * shakingSpeed) * shakingDistance;
            var newPos = new Vector3
                ( fallingStartTransform.position.x + xOffset
                , fallingStartTransform.position.y
                , fallingStartTransform.position.z + zOffset);

            platformRigidBody.MovePosition(newPos);

            if(statusTimer > shakingInterval)
            {
                ChangeStatus(PropStatus.Falling);
                return;
            }
        }
        else if(activeStatus == PropStatus.Falling)
        {
            float fallingProgress = statusTimer / fallingInterval;
            float fallingLerp = Mathf.SmoothStep(0.0F, 1.0F, fallingProgress);

            var fallingPosition = Vector3.Lerp(fallingStartTransform.position, fallingFinishTransform.position, fallingLerp);
            var fallingRotation = Quaternion.Lerp(fallingStartTransform.rotation, fallingFinishTransform.rotation, fallingLerp);

            platformRigidBody.MovePosition(fallingPosition);
            platformRigidBody.MoveRotation(fallingRotation);

            if (statusTimer > fallingInterval)
            {
                ChangeStatus(PropStatus.Recovering);
                return;
            }
        }
        else if(activeStatus == PropStatus.Recovering)
        {
            if (!isOneShot && statusTimer > recoveryInterval)
            {
                ChangeStatus(PropStatus.Resting);
                return;
            }
        }

        statusTimer += Time.deltaTime;
    }

    private void ChangeStatus(PropStatus newStatus)
    {
        EndStatus();
        previousStatus = activeStatus;
        activeStatus = newStatus;
        statusTimer = 0.0F;
        BeginStatus();
        args.activeStatus = activeStatus;
        args.previousStatus = previousStatus;
        StatusChanged?.Invoke(this, args);
    }

    private void BeginStatus()
    {
        if (activeStatus == PropStatus.Resting)
        {
            platformRigidBody.gameObject.transform.position = fallingStartTransform.position;
            platformRigidBody.gameObject.transform.rotation = fallingStartTransform.rotation;
        }
        else if (activeStatus == PropStatus.Shaking)
        {
            platformRigidBody.gameObject.transform.position = fallingStartTransform.position;
            platformRigidBody.gameObject.transform.rotation = fallingStartTransform.rotation;

            if(shakingAudioSource != null)
                shakingAudioSource.PlayPitchedOneShot
                    ( shakingAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);
        }
        else if (activeStatus == PropStatus.Falling)
        {
            platformRigidBody.gameObject.transform.position = fallingStartTransform.position;
            platformRigidBody.gameObject.transform.rotation = fallingStartTransform.rotation;

            if(fallingAudioSource != null)
                fallingAudioSource.PlayPitchedOneShot
                    ( fallingAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);
        }
        else if (activeStatus == PropStatus.Recovering)
        {
            platformRigidBody.gameObject.transform.position = fallingFinishTransform.position;
            platformRigidBody.gameObject.transform.rotation = fallingFinishTransform.rotation;
        }
    }

    private void EndStatus()
    {
        if (activeStatus == PropStatus.Resting)
        {

        }
        else if (activeStatus == PropStatus.Shaking)
        {
        }
        else if (activeStatus == PropStatus.Falling)
        {
        }
        else if (activeStatus == PropStatus.Recovering)
        {

        }
    }

    private void OnRemoteTriggerEntered(object sender, RemoteTriggerArgs args)
    {
        if (args.other.name == TRANSFORM_NAME_PLAYER_COLLIDER)
            pressure++;
    }

    private void OnRemoteTriggerExited(object sender, RemoteTriggerArgs args)
    {
        if (args.other.name == TRANSFORM_NAME_PLAYER_COLLIDER)
            pressure--;
    }
}
