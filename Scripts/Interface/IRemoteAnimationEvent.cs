using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IRemoteAnimationEvent
{
    event EventHandler<RemoteAnimationEventArgs> AnimationEventTriggered;
}

public class RemoteAnimationEventArgs
{
    public string value;
}
