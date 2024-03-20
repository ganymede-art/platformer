using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAnimationEvent : MonoBehaviour, IRemoteAnimationEvent
{
    // Private fields.
    private RemoteAnimationEventArgs args;

    // Public events.
    public event EventHandler<RemoteAnimationEventArgs> AnimationEventTriggered;

    private void Awake()
    {
        args = new RemoteAnimationEventArgs();
    }

    public void OnAnimationEvent(string value)
    {
        args.value = value;
        AnimationEventTriggered?.Invoke(this, args);
    }
}
