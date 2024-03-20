using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleSwitch : MonoBehaviour, ISwitch
{
    private SwitchArgs args;
    private SwitchStatus activeStatus;
    private SwitchStatus previousStatus;

    public SwitchStatus ActiveStatus => activeStatus;
    public SwitchStatus PreviousStatus => previousStatus;
    public GameObject SwitchObject => gameObject;
    public event EventHandler<SwitchArgs> StatusChanged;

    private void Start()
    {
        args = new SwitchArgs();
        previousStatus = SwitchStatus.Off;
        activeStatus = SwitchStatus.Off;
        ChangeStatus(SwitchStatus.Off);
    }

    private void ChangeStatus(SwitchStatus newStatus)
    {
        EndStatus();
        previousStatus = activeStatus;
        activeStatus = newStatus;
        BeginStatus();
        args.activeStatus = activeStatus;
        args.previousStatus = previousStatus;
        StatusChanged?.Invoke(this, args);
    }

    private void BeginStatus() { }
    private void EndStatus() { }

    public string GetName() => $"InvisibleSwitch";

    public void OverrideStatus(SwitchStatus newStatus)
    {
        ChangeStatus(newStatus);
    }
}
