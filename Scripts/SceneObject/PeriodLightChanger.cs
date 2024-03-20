using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodLightChanger : MonoBehaviour, IPeriodObserver
{
    // Public fields.
    public Light earlyLight;
    public Light dawnLight;
    public Light morningLight;
    public Light afternoonLight;
    public Light duskLight;
    public Light lateLight;

    void Start()
    {
        OnPeriodChanged(TimeHighLogic.G.PeriodOfDay);
    }

    public void OnPeriodChanged(PeriodType periodType)
    {
        earlyLight.gameObject.SetActive(false);
        dawnLight.gameObject.SetActive(false);
        morningLight.gameObject.SetActive(false);
        afternoonLight.gameObject.SetActive(false);
        duskLight.gameObject.SetActive(false);
        lateLight.gameObject.SetActive(false);

        var light
            = (periodType == PeriodType.Early) ? earlyLight
            : (periodType == PeriodType.Dawn) ? dawnLight
            : (periodType == PeriodType.Morning) ? morningLight
            : (periodType == PeriodType.Afternoon) ? afternoonLight
            : (periodType == PeriodType.Dusk) ? duskLight
            : (periodType == PeriodType.Late) ? lateLight
            : lateLight;

        light.gameObject.SetActive(true);
    }
}
