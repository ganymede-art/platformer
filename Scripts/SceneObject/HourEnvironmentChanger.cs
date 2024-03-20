using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourEnvironmentChanger : MonoBehaviour, IHourObserver
{
    // Public fields.
    public EnvironmentData earlyEnvironmentData;
    public EnvironmentData dawnEnvironmentData;
    public EnvironmentData morningEnvironmentData;
    public EnvironmentData afternoonEnvironmentData;
    public EnvironmentData duskEnvironmentData;
    public EnvironmentData lateEnvironmentData;

    void Start()
    {
        OnHourChanged(TimeHighLogic.G.Hour);
    }

    public void OnHourChanged(float hour)
    {
        PeriodType periodType = TimeHighLogic.GetPeriodFromHour(hour);

        var startEnvironmentData
            = (periodType == PeriodType.Early) ? earlyEnvironmentData
            : (periodType == PeriodType.Dawn) ? dawnEnvironmentData
            : (periodType == PeriodType.Morning) ? morningEnvironmentData
            : (periodType == PeriodType.Afternoon) ? afternoonEnvironmentData
            : (periodType == PeriodType.Dusk) ? duskEnvironmentData
            : (periodType == PeriodType.Late) ? lateEnvironmentData
            : lateEnvironmentData;

        var finishEnvironmentData
            = (periodType == PeriodType.Early) ? dawnEnvironmentData
            : (periodType == PeriodType.Dawn) ? morningEnvironmentData
            : (periodType == PeriodType.Morning) ? afternoonEnvironmentData
            : (periodType == PeriodType.Afternoon) ? duskEnvironmentData
            : (periodType == PeriodType.Dusk) ? lateEnvironmentData
            : (periodType == PeriodType.Late) ? earlyEnvironmentData
            : lateEnvironmentData;

        float periodProgress = TimeHighLogic.GetPeriodProgress(periodType, hour);

        var lerpAmbientLight = Color.Lerp(startEnvironmentData.ambientLightColour, finishEnvironmentData.ambientLightColour, periodProgress);
        var lerpFogColour = Color.Lerp(startEnvironmentData.fogColour, finishEnvironmentData.fogColour, periodProgress);
        float lerpFogDensity = Mathf.Lerp(startEnvironmentData.fogDensity, finishEnvironmentData.fogDensity, periodProgress);

        RenderSettings.ambientLight = lerpAmbientLight;

        RenderSettings.fog = startEnvironmentData.isFogEnabled;
        RenderSettings.fogColor = lerpFogColour;
        RenderSettings.fogDensity = lerpFogDensity;
        RenderSettings.fogMode = FogMode.Exponential;
    }
}
