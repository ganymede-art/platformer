using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProp
{
    event EventHandler<PropArgs> StatusChanged;
    PropStatus ActiveStatus { get; }
    PropStatus PreviousStatus { get; }
    public GameObject PropObject { get; }
}

public class PropArgs
{
    public PropStatus activeStatus;
    public PropStatus previousStatus;
}