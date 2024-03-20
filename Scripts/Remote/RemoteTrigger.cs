using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteTrigger : MonoBehaviour, IRemoteTrigger
{
    // Private fields.
    private RemoteTriggerArgs args;

    // Public events.
    public event EventHandler<RemoteTriggerArgs> RemoteTriggerEntered;
    public event EventHandler<RemoteTriggerArgs> RemoteTriggerStayed;
    public event EventHandler<RemoteTriggerArgs> RemoteTriggerExited;

    private void Awake()
    {
        args = new RemoteTriggerArgs();
    }

    private void OnTriggerEnter(Collider other)
    {
        args.remoteTriggerObject = gameObject;
        args.other = other;
        RemoteTriggerEntered?.Invoke(this, args);
    }

    private void OnTriggerStay(Collider other)
    {
        args.remoteTriggerObject = gameObject;
        args.other = other;
        RemoteTriggerStayed?.Invoke(this, args);
    }

    private void OnTriggerExit(Collider other)
    {
        args.remoteTriggerObject = gameObject;
        args.other = other;
        RemoteTriggerExited?.Invoke(this, args);
    }
}
