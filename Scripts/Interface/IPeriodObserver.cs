using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPeriodObserver
{
    public void OnPeriodChanged(PeriodType periodType);
}
