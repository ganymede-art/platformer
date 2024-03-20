using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class TripWireSwitch : MonoBehaviour, ISwitch
{
    // Private fields.
    private SwitchArgs args;
    private SwitchStatus activeStatus;
    private SwitchStatus previousStatus;
    private RemoteTrigger remoteTrigger;

    // Public properties.
    public SwitchStatus ActiveStatus => activeStatus;
    public SwitchStatus PreviousStatus => previousStatus;
    public GameObject SwitchObject => gameObject;

    // Public fields.
    public GameObject remoteTriggerObject;
    public GameObject wireObject;
    public AudioSource onAudioSource;

    // Events.
    public event EventHandler<SwitchArgs> StatusChanged;

    private void Awake()
    {
        activeStatus = SwitchStatus.Off;
        previousStatus = SwitchStatus.Off;
        remoteTrigger = remoteTriggerObject.GetComponent<RemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEntered;
    }

    private void OnDestroy()
    {
        if(remoteTrigger != null)
            remoteTrigger.RemoteTriggerEntered -= OnRemoteTriggerEntered;
    }

    private void OnRemoteTriggerEntered(object sender, RemoteTriggerArgs args)
    {
        if (activeStatus == SwitchStatus.Off && args.other.name == TRANSFORM_NAME_PLAYER_COLLIDER)
            Trip();
    }

    private void Trip()
    {
        activeStatus = SwitchStatus.On;

        if (wireObject != null)
            Destroy(wireObject);

        if (onAudioSource != null)
            onAudioSource.PlayOneShot(onAudioSource.clip, SettingsHighLogic.G.PropVolume);
    }

    public void OverrideStatus(SwitchStatus newStatus)
    {
        if (newStatus == SwitchStatus.On)
            Trip();
        else
            Debug.LogWarning($"[{GetType()}][{gameObject.name}] Invalid override status.");
    }
}
