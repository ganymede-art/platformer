using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodEnvironmentChanger : MonoBehaviour, IPeriodObserver
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
        OnPeriodChanged(TimeHighLogic.G.PeriodOfDay);
    }

    public void OnPeriodChanged(PeriodType periodType)
    {
        var environmentData
            = (periodType == PeriodType.Early) ? earlyEnvironmentData
            : (periodType == PeriodType.Dawn) ? dawnEnvironmentData
            : (periodType == PeriodType.Morning) ? morningEnvironmentData
            : (periodType == PeriodType.Afternoon) ? afternoonEnvironmentData
            : (periodType == PeriodType.Dusk) ? duskEnvironmentData
            : (periodType == PeriodType.Late) ? lateEnvironmentData
            : lateEnvironmentData;

        RenderSettings.ambientLight = environmentData.ambientLightColour;

        RenderSettings.fog = environmentData.isFogEnabled;
        RenderSettings.fogColor = environmentData.fogColour;
        RenderSettings.fogDensity = environmentData.fogDensity;
        RenderSettings.fogMode = FogMode.Exponential;
    }
}
