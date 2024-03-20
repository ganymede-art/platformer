using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourLightChanger : MonoBehaviour, IHourObserver
{
    // Public fields.
    public Light hourLight;
    [Space]
    public Light earlyLight;
    public Light dawnLight;
    public Light morningLight;
    public Light afternoonLight;
    public Light duskLight;
    public Light lateLight;

    

    private void Awake()
    {
        earlyLight.enabled = false;
        dawnLight.enabled = false;
        morningLight.enabled = false;
        afternoonLight.enabled = false;
        duskLight.enabled = false;
        lateLight.enabled = false;

        hourLight.enabled = true;
}

    private void Start()
    {
        OnHourChanged(TimeHighLogic.G.Hour);
    }

    public void OnHourChanged(float hour)
    {
        PeriodType periodType = TimeHighLogic.GetPeriodFromHour(hour);

        Light startLight = periodType switch
        {
            PeriodType.Early => earlyLight,
            PeriodType.Dawn => dawnLight,
            PeriodType.Morning => morningLight,
            PeriodType.Afternoon => afternoonLight,
            PeriodType.Dusk => duskLight,
            PeriodType.Late => lateLight,
            _ => earlyLight,
        };

        Light finishLight = periodType switch
        {
            PeriodType.Early => dawnLight,
            PeriodType.Dawn => morningLight,
            PeriodType.Morning => afternoonLight,
            PeriodType.Afternoon => duskLight,
            PeriodType.Dusk => lateLight,
            PeriodType.Late => earlyLight,
            _ => morningLight,
        };

        float periodProgress = TimeHighLogic.GetPeriodProgress(periodType, hour);

        hourLight.intensity = Mathf.Lerp(startLight.intensity, finishLight.intensity, periodProgress);
        hourLight.transform.rotation = Quaternion.Lerp(startLight.transform.rotation, finishLight.transform.rotation, periodProgress);
        hourLight.transform.position = Vector3.Lerp(startLight.transform.position, finishLight.transform.position, periodProgress);
        hourLight.color = Color.Lerp(startLight.color, finishLight.color, periodProgress);
    }
}
