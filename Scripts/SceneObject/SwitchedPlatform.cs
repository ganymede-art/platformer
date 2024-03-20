using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchedPlatform : MonoBehaviour
{
    // Consts.
    private const float MIN_SFX_PITCH = 0.9F;
    private const float MAX_SFX_PITCH = 1.1F;

    // Private fields.
    private ISwitch parentSwitch;
    private float statusTimer;
    private SwitchStatus previousStatus;
    private SwitchStatus activeStatus;

    // Public fields.
    public GameObject parentSwitchObject;
    [Space]
    public bool doUpdateInFilm;
    [Space]
    public float turningOffInterval;
    public float turningOnInterval;
    public float offGraceInterval;
    public float onGraceInterval;
    [Space]
    public Rigidbody platformRigidBody;
    public Transform offTransform;
    public Transform onTransform;
    [Space]
    public AudioSource offAudioSource;
    public AudioSource onAudioSource;
    public AudioSource turningOffAudioSource;
    public AudioSource turningOnAudioSource;
    

    private void Awake()
    {
        parentSwitch = parentSwitchObject?.GetComponent<ISwitch>();
        
        if(parentSwitch != null)
            parentSwitch.StatusChanged += OnParentSwitchStatusChanged;
    }

    private void Start()
    {
        previousStatus = SwitchStatus.Off;
        activeStatus = SwitchStatus.Off;
        ChangeStatus(SwitchStatus.Off);
    }

    private void Update()
    {
        if (!(StateHighLogic.G.ActiveState == HighLogicStateId.Play || (StateHighLogic.G.ActiveState == HighLogicStateId.Film && doUpdateInFilm)))
            return;

        if (activeStatus == SwitchStatus.Off)
        {
            if (parentSwitch.ActiveStatus == SwitchStatus.On 
                && statusTimer >= offGraceInterval)
                ChangeStatus(SwitchStatus.TurningOn);
        }
        else if (activeStatus == SwitchStatus.On)
        {
            if (parentSwitch.ActiveStatus == SwitchStatus.Off 
                && statusTimer >= onGraceInterval)
                ChangeStatus(SwitchStatus.TurningOff);
        }
        else if (activeStatus == SwitchStatus.TurningOff)
        {
            float moveProgress = Mathf.InverseLerp(0, turningOffInterval, statusTimer);
            float moveLerp = Mathf.SmoothStep(0.0F, 1.0F, moveProgress);
            platformRigidBody.MovePosition(Vector3.Lerp(onTransform.position, offTransform.position, moveLerp));
            platformRigidBody.MoveRotation(Quaternion.Lerp(onTransform.rotation, offTransform.rotation, moveLerp));
            if (moveLerp >= 1.0F)
            {
                ChangeStatus(SwitchStatus.Off);
                return;
            }
        }
        else if (activeStatus == SwitchStatus.TurningOn)
        {
            float moveProgress = Mathf.InverseLerp(0, turningOnInterval, statusTimer);
            float moveLerp = Mathf.SmoothStep(0.0F, 1.0F, moveProgress);
            platformRigidBody.MovePosition(Vector3.Lerp(offTransform.position, onTransform.position, moveLerp));
            platformRigidBody.MoveRotation(Quaternion.Lerp(offTransform.rotation, onTransform.rotation, moveLerp));
            if (moveLerp >= 1.0F)
            {
                ChangeStatus(SwitchStatus.On);
                return;
            }
        }

        statusTimer += Time.deltaTime;
    }

    private void ChangeStatus(SwitchStatus newStatus)
    {
        EndStatus();
        previousStatus = activeStatus;
        activeStatus = newStatus;
        statusTimer = 0.0F;
        BeginStatus();
    }

    private void BeginStatus()
    {
        if (activeStatus == SwitchStatus.Off)
        {
            if (offAudioSource != null && previousStatus != SwitchStatus.Off)
            {
                offAudioSource.PlayPitchedOneShot
                    (onAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);
            }

            platformRigidBody.gameObject.transform.position = offTransform.position;          
        }
        else if (activeStatus == SwitchStatus.On)
        {
            if (onAudioSource != null && previousStatus != SwitchStatus.On)
            {
                onAudioSource.PlayPitchedOneShot
                    (onAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);
            }

            platformRigidBody.gameObject.transform.position = onTransform.position;
        }
        else if (activeStatus == SwitchStatus.TurningOff)
        {
            if (turningOffAudioSource != null && previousStatus != SwitchStatus.TurningOff)
            {
                turningOffAudioSource.volume = SettingsHighLogic.G.PropVolume;
                turningOffAudioSource.Play();
            }
        }
        else if (activeStatus == SwitchStatus.TurningOn)
        {
            if (turningOnAudioSource != null && previousStatus != SwitchStatus.TurningOn)
            {
                turningOnAudioSource.volume = SettingsHighLogic.G.PropVolume;
                turningOnAudioSource.Play();
            }
        }
    }

    private void EndStatus()
    {
        if (activeStatus == SwitchStatus.TurningOff)
        {
            if (turningOffAudioSource != null)
                turningOffAudioSource.Stop();
        }
        else if (activeStatus == SwitchStatus.TurningOn)
        {
            if (turningOnAudioSource != null)
                turningOnAudioSource.Stop();
        }
    }

    private void  OnParentSwitchStatusChanged(object sender, SwitchArgs args)
    {
    }
}
