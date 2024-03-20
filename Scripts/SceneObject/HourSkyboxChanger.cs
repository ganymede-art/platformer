using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourSkyboxChanger : MonoBehaviour, IHourObserver
{
    // Public fields.
    public MeshRenderer meshRenderer;

    public Material earlyMaterial;
    public Material dawnMaterial;
    public Material morningMaterial;
    public Material afternoonMaterial;
    public Material duskMaterial;
    public Material lateMaterial;

    private void Start()
    {
        OnHourChanged(TimeHighLogic.G.Hour);
    }

    public void OnHourChanged(float hour)
    {
        PeriodType periodType = TimeHighLogic.GetPeriodFromHour(hour);

        Material startMaterial
            = (periodType == PeriodType.Early) ? earlyMaterial
            : (periodType == PeriodType.Dawn) ? dawnMaterial
            : (periodType == PeriodType.Morning) ? morningMaterial
            : (periodType == PeriodType.Afternoon) ? afternoonMaterial
            : (periodType == PeriodType.Dusk) ? duskMaterial
            : (periodType == PeriodType.Late) ? lateMaterial
            : dawnMaterial;

        Material finishMaterial
            = (periodType == PeriodType.Early) ? dawnMaterial
            : (periodType == PeriodType.Dawn) ? morningMaterial
            : (periodType == PeriodType.Morning) ? afternoonMaterial
            : (periodType == PeriodType.Afternoon) ? duskMaterial
            : (periodType == PeriodType.Dusk) ? lateMaterial
            : (periodType == PeriodType.Late) ? earlyMaterial
            : dawnMaterial;

        Color startNadir = startMaterial.GetColor("_NadirColor");
        Color startHorizon = startMaterial.GetColor("_HorizonColor");
        Color startZenith = startMaterial.GetColor("_ZenithColor");
        float startMiddle = startMaterial.GetFloat("_Middle");

        Color finishNadir = finishMaterial.GetColor("_NadirColor");
        Color finishHorizon = finishMaterial.GetColor("_HorizonColor");
        Color finishZenith = finishMaterial.GetColor("_ZenithColor");
        float finishMiddle = finishMaterial.GetFloat("_Middle");

        float periodProgress = TimeHighLogic.GetPeriodProgress(periodType, hour);

        Color lerpNadir = Color.Lerp(startNadir, finishNadir, periodProgress);
        Color lerpHorizon = Color.Lerp(startHorizon, finishHorizon, periodProgress);
        Color lerpZenith = Color.Lerp(startZenith, finishZenith, periodProgress);
        float lerpMiddle = Mathf.Lerp(startMiddle, finishMiddle, periodProgress);

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetColor("_NadirColor", lerpNadir);
        mpb.SetColor("_HorizonColor", lerpHorizon);
        mpb.SetColor("_ZenithColor", lerpZenith);
        mpb.SetFloat("_Middle", lerpMiddle);
        meshRenderer.SetPropertyBlock(mpb);
    }
}
