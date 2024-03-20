using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSwitch : MonoBehaviour, ISwitch, IInteractable
{

    // Consts.
    private const float MIN_SFX_PITCH = 0.9F;
    private const float MAX_SFX_PITCH = 1.1F;

    // Private fields.
    private SwitchArgs args;
    private float statusTimer;
    private SwitchStatus activeStatus;
    private SwitchStatus previousStatus;

    // Public properties.
    public SwitchStatus ActiveStatus => activeStatus;
    public SwitchStatus PreviousStatus => previousStatus;
    public GameObject SwitchObject => gameObject;

    public bool IsInteractable => IsInteractableCheck();
    public float InteractableRange => interactableRange;
    public GameObject InteractableGameObject => gameObject;
    public Transform InteractableTransform => transform;
    public Vector3 InteractablePromptOffset => interactablePromptOffset;

    // Public fields.
    [Header("Interaction Attributes")]
    public float interactableRange;
    public Vector3 interactablePromptOffset;
    [Header("Lever Attributes")]
    public GameObject leverObject;
    public Transform offTransform;
    public Transform onTransform;
    public float turningInterval;
    [Header("One Shot Attributes")]
    public bool isOneShot;
    public VariableIdConstant oneShotVariableId;
    public bool doSetOneShotVariable;
    [Space]
    public AudioSource offAudioSource;
    public AudioSource onAudioSource;
    public AudioSource turningOffAudioSource;
    public AudioSource turningOnAudioSource;

    // Events.
    public event EventHandler<SwitchArgs> StatusChanged;

    private void Awake()
    {
        args = new SwitchArgs();
    }

    private void Start()
    {
        ActiveSceneHighLogic.G.Interactables[gameObject] = this;

        previousStatus = SwitchStatus.Off;
        activeStatus = SwitchStatus.Off;
        ChangeStatus(SwitchStatus.Off);

        if (isOneShot && oneShotVariableId != null)
        {
            bool isOneShotSet = PersistenceHighLogic.G.GetBoolVariable(oneShotVariableId.VariableId);

            if (isOneShotSet)
                ChangeStatus(SwitchStatus.On);
        }
    }

    private void OnDestroy()
    {
        if (ActiveSceneHighLogic.G == null)
            return;
        ActiveSceneHighLogic.G.Interactables.Remove(gameObject);
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if (activeStatus == SwitchStatus.Off) { }
        else if (activeStatus == SwitchStatus.On) { }
        else if (activeStatus == SwitchStatus.TurningOff)
        {
            float moveProgress = Mathf.InverseLerp(0, turningInterval, statusTimer);
            float moveLerp = Mathf.SmoothStep(0.0F, 1.0F, moveProgress);
            leverObject.transform.position = Vector3.Lerp(onTransform.position, offTransform.position, moveLerp);
            leverObject.transform.rotation = Quaternion.Lerp(onTransform.rotation, offTransform.rotation, moveLerp);
            if (moveLerp >= 1.0F)
            {
                ChangeStatus(SwitchStatus.Off);
                return;
            }
        }
        else if (activeStatus == SwitchStatus.TurningOn)
        {
            float moveProgress = Mathf.InverseLerp(0, turningInterval, statusTimer);
            float moveLerp = Mathf.SmoothStep(0.0F, 1.0F, moveProgress);
            leverObject.transform.position = Vector3.Lerp(offTransform.position, onTransform.position, moveLerp);
            leverObject.transform.rotation = Quaternion.Lerp(offTransform.rotation, onTransform.rotation, moveLerp);
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
        args.activeStatus = activeStatus;
        args.previousStatus = previousStatus;
        StatusChanged?.Invoke(this, args);
    }

    public void OverrideStatus(SwitchStatus newStatus)
    {
        if (newStatus == SwitchStatus.Off)
        {
            if(activeStatus == SwitchStatus.On || activeStatus == SwitchStatus.TurningOn)
            {
                ChangeStatus(SwitchStatus.TurningOff);
                return;
            }
        }
        else if(newStatus == SwitchStatus.On)
        {
            if(activeStatus == SwitchStatus.Off || activeStatus == SwitchStatus.TurningOff)
            {
                ChangeStatus(SwitchStatus.TurningOn);
                return;
            }
        }
        else
        {
            Debug.LogWarning($"[{GetType()}][{gameObject.name}] Invalid override status.");
            return;
        }
    }

    private void BeginStatus()
    {
        if (activeStatus == SwitchStatus.Off)
        {
            leverObject.transform.position = offTransform.position;

            if (previousStatus == SwitchStatus.TurningOff
                && offAudioSource != null)
                offAudioSource.PlayPitchedOneShot
                    (offAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);
        }
        else if (activeStatus == SwitchStatus.On)
        {
            leverObject.transform.position = onTransform.position;

            if (isOneShot
                && doSetOneShotVariable
                && oneShotVariableId != null)
                PersistenceHighLogic.G.SetBoolVariable(oneShotVariableId.VariableId, true);

            if (previousStatus == SwitchStatus.TurningOn
                && onAudioSource != null)
                onAudioSource.PlayPitchedOneShot
                    (onAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);
        }
        else if (activeStatus == SwitchStatus.TurningOff)
        {
            if (turningOffAudioSource != null)
                turningOffAudioSource.PlayPitchedOneShot
                    (turningOffAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);
        }
        else if (activeStatus == SwitchStatus.TurningOn)
        {
            if (turningOnAudioSource != null)
                turningOnAudioSource.PlayPitchedOneShot
                    (turningOnAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);
        }
    }

    private void EndStatus()
    {
        if (activeStatus == SwitchStatus.TurningOff)
        {
            if (turningOffAudioSource != null
                && turningOffAudioSource.isPlaying)
                turningOffAudioSource.Stop();
        }
        else if (activeStatus == SwitchStatus.TurningOn)
        {
            if (turningOnAudioSource != null
                && turningOnAudioSource.isPlaying)
                turningOnAudioSource.Stop();
        }
    }

    public void OnInteract()
    {
        if(activeStatus == SwitchStatus.Off)
        {
            ChangeStatus(SwitchStatus.TurningOn);
            return;
        }
        else if(activeStatus == SwitchStatus.On && !isOneShot)
        {
            ChangeStatus(SwitchStatus.TurningOff);
            return;
        }
    }

    private bool IsInteractableCheck()
    {
        if (activeStatus != SwitchStatus.On && activeStatus != SwitchStatus.Off)
            return false;

        if (activeStatus == SwitchStatus.On && isOneShot)
            return false;

        if (!gameObject.activeInHierarchy)
            return false;

        return true;
    }
}
