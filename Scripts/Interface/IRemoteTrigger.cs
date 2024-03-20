using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IRemoteTrigger
{
    event EventHandler<RemoteTriggerArgs> RemoteTriggerEntered;
    event EventHandler<RemoteTriggerArgs> RemoteTriggerStayed;
    event EventHandler<RemoteTriggerArgs> RemoteTriggerExited;
}

public class RemoteTriggerArgs
{
    public GameObject remoteTriggerObject;
    public Collider other;
}
