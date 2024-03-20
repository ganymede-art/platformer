using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSource
{
    // Public fields.
    public string actionId;
    public HighLogicStateId actionHighLogicStateId;

    public ActionStatus actionStatus;

    public float actionTimer;
    public float actionUpdateTimer;

    public GameObject activeActionObject;
    public IAction activeAction;
    public GameObject previousActionObject;
    public IAction previousAction;
}
