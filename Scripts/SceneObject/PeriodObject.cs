using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PeriodObject : MonoBehaviour, IPeriodObserver
{
    public GameObject periodObject;
    public PeriodTypeConstant[] activePeriodTypes;

    void Start()
    {
        OnPeriodChanged(TimeHighLogic.G.PeriodOfDay);
    }

    public void OnPeriodChanged(PeriodType periodType)
    {
        bool isActive = activePeriodTypes.Any(x => x.PeriodType == periodType);
        periodObject.SetActive(isActive);
    }
}
