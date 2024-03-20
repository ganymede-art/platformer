using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class PressurePlateSwitch : MonoBehaviour, ISwitch
{
    // Consts.
    private const float MIN_SFX_PITCH = 0.9F;
    private const float MAX_SFX_PITCH = 1.1F;

    // Private fields.
    private SwitchArgs args;
    private float statusTimer;
    private SwitchStatus activeStatus;
    private SwitchStatus previousStatus;
    private IRemoteTrigger remoteTrigger;
    private int pressure;

    // Public fields.
    public GameObject remoteTriggerObject;
    public bool canPlayerActivate;
    public float turningInterval;
    public float graceInterval;
    [Space]
    public GameObject plateObject;
    public Transform offTransform;
    public Transform onTransform;
    [Space]
    public bool isOneShot;
    public VariableIdConstant oneShotVariableId;
    public bool doSetOneShotVariable;
    [Space]
    public bool isTimed;
    public TimerIdConstant timerId;
    public float timerSeconds;
    [Space]
    public AudioSource offAudioSource;
    public AudioSource onAudioSource;
    public AudioSource turningOffAudioSource;
    public AudioSource turningOnAudioSource;
    
    // Public properties.
    public SwitchStatus ActiveStatus => activeStatus;
    public SwitchStatus PreviousStatus => previousStatus;
    public GameObject SwitchObject => gameObject;

    // Events.
    public event EventHandler<SwitchArgs> StatusChanged;

    private void Awake()
    {
        args = new SwitchArgs();
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEntered;
        remoteTrigger.RemoteTriggerExited += OnRemoteTriggerExited;
    }

    private void Start()
    {
        previousStatus = SwitchStatus.Off;
        activeStatus = SwitchStatus.Off;
        ChangeStatus(SwitchStatus.Off);

        if(isOneShot && oneShotVariableId != null)
        {
            bool isOneShotSet = PersistenceHighLogic.G.GetBoolVariable(oneShotVariableId.VariableId);

            if (isOneShotSet)
                ChangeStatus(SwitchStatus.On);
        }

        if (isTimed)
            TimerHighLogic.G.TimerCompleted += OnTimerCompleted;
    }

    private void OnDestroy()
    {
        if (isTimed && TimerHighLogic.G != null)
            TimerHighLogic.G.TimerCompleted -= OnTimerCompleted;
    }

    private void OnTimerCompleted(object sender, TimerArgs args)
    {
        if (args.timerId == timerId.TimerId && !isOneShot)
            ChangeStatus(SwitchStatus.TurningOff);
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if(activeStatus == SwitchStatus.Off)
        {
            if (pressure > 0)
            {
                ChangeStatus(SwitchStatus.TurningOn);
                return;
            }
        }
        else if(activeStatus == SwitchStatus.On)
        {
            if(pressure <= 0 
                && !isOneShot 
                && !isTimed
                && statusTimer > graceInterval)
            {
                ChangeStatus(SwitchStatus.TurningOff);
                return;
            }
        }
        else if(activeStatus == SwitchStatus.TurningOff)
        {
            float moveProgress = Mathf.InverseLerp(0, turningInterval, statusTimer);
            float moveLerp = Mathf.SmoothStep(0.0F, 1.0F, moveProgress);
            plateObject.transform.position = Vector3.Lerp(onTransform.position, offTransform.position, moveLerp);
            plateObject.transform.rotation = Quaternion.Lerp(onTransform.rotation, offTransform.rotation, moveLerp);
            plateObject.transform.localScale = Vector3.Lerp(onTransform.localScale, offTransform.localScale,moveLerp);
            if (moveLerp >= 1.0F)
            {
                ChangeStatus(SwitchStatus.Off);
                return;
            }
        }
        else if(activeStatus == SwitchStatus.TurningOn)
        {
            float moveProgress = Mathf.InverseLerp(0, turningInterval, statusTimer);
            float moveLerp = Mathf.SmoothStep(0.0F, 1.0F, moveProgress);
            plateObject.transform.position = Vector3.Lerp(offTransform.position, onTransform.position, moveLerp);
            plateObject.transform.rotation = Quaternion.Lerp(offTransform.rotation, onTransform.rotation, moveLerp);
            plateObject.transform.localScale = Vector3.Lerp(offTransform.localScale, onTransform.localScale, moveLerp);
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
            if (activeStatus == SwitchStatus.On || activeStatus == SwitchStatus.TurningOn)
            {
                ChangeStatus(SwitchStatus.TurningOff);
                return;
            }
        }
        else if (newStatus == SwitchStatus.On)
        {
            if (activeStatus == SwitchStatus.Off || activeStatus == SwitchStatus.TurningOff)
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
        if(activeStatus == SwitchStatus.Off)
        {
            plateObject.transform.position = offTransform.position;

            if (previousStatus == SwitchStatus.TurningOff
                && offAudioSource != null)
                offAudioSource.PlayPitchedOneShot
                    ( offAudioSource.clip
                    , SettingsHighLogic.G.PropVolume
                    , MIN_SFX_PITCH
                    , MAX_SFX_PITCH);

        }
        else if(activeStatus == SwitchStatus.On)
        {
            if (isOneShot 
                && doSetOneShotVariable
                && oneShotVariableId != null)
                PersistenceHighLogic.G.SetBoolVariable(oneShotVariableId.VariableId, true);

            if (isTimed)
                TimerHighLogic.G.AddTimer(timerId.TimerId, timerSeconds);

            plateObject.transform.position = onTransform.position;

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
        if(activeStatus == SwitchStatus.TurningOff)
        {
            if (turningOffAudioSource != null 
                && turningOffAudioSource.isPlaying)
                turningOffAudioSource.Stop();
        }
        else if(activeStatus == SwitchStatus.TurningOn)
        {
            if (turningOnAudioSource != null 
                && turningOnAudioSource.isPlaying)
                turningOnAudioSource.Stop();
        }
    }

    private void OnRemoteTriggerEntered(object sender, RemoteTriggerArgs args)
    {
        if(canPlayerActivate
            && args.other.name == TRANSFORM_NAME_PLAYER_COLLIDER)
            pressure++;
    }

    private void OnRemoteTriggerExited(object sender, RemoteTriggerArgs args)
    {
        if (canPlayerActivate 
            && args.other.name == TRANSFORM_NAME_PLAYER_COLLIDER)
            pressure--;
    }
}
