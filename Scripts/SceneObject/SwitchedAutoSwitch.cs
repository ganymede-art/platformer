using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchedAutoSwitch : MonoBehaviour, ISwitch, INameable
{
    // Private fields.
    private ISwitch parentSwitch;
    private SwitchArgs args;
    private float statusTimer;
    private SwitchStatus activeStatus;
    private SwitchStatus previousStatus;
    
    public SwitchStatus ActiveStatus => activeStatus;
    public SwitchStatus PreviousStatus => previousStatus;
    public GameObject SwitchObject => gameObject;

    // Public fields.
    public GameObject parentSwitchObject;
    [Space]
    public float switchOnInterval;
    public float switchOffInterval;
    public float startingTime;

    // Events.
    public event EventHandler<SwitchArgs> StatusChanged;

    private void Awake()
    {
        if (parentSwitchObject != null)
            parentSwitch = parentSwitchObject.GetComponent<ISwitch>();
    }

    void Start()
    {
        args = new SwitchArgs();
        previousStatus = SwitchStatus.Off;
        activeStatus = SwitchStatus.Off;
        ChangeStatus(SwitchStatus.Off);

        statusTimer = startingTime;
    }

    void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if (parentSwitch != null && parentSwitch.ActiveStatus != SwitchStatus.On)
            return;

        if(activeStatus == SwitchStatus.Off)
        {
            if (statusTimer >= switchOffInterval)
            {
                ChangeStatus(SwitchStatus.On);
                return;
            }
        }
        else if(activeStatus == SwitchStatus.On)
        {
            if (statusTimer >= switchOnInterval)
            {
                ChangeStatus(SwitchStatus.Off);
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
        ChangeStatus(newStatus);
    }

    private void BeginStatus() { }
    private void EndStatus() { }

    public string GetName() => $"SwitchedAutoSwitchOn{switchOnInterval}Off{switchOffInterval}";

    
}
