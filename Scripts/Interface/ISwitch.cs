using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitch
{ 
    event EventHandler<SwitchArgs> StatusChanged;
    SwitchStatus ActiveStatus { get; }
    SwitchStatus PreviousStatus { get; }
    public GameObject SwitchObject { get; }
    public void OverrideStatus(SwitchStatus newStatus);
}

public class SwitchArgs
{
    public SwitchStatus activeStatus;
    public SwitchStatus previousStatus;
}