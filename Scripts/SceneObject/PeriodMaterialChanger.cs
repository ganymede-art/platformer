using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodMaterialChanger : MonoBehaviour, IPeriodObserver
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
        OnPeriodChanged(TimeHighLogic.G.PeriodOfDay);
    }

    public void OnPeriodChanged(PeriodType periodType)
    {
        meshRenderer.material
            = (periodType == PeriodType.Early) ? earlyMaterial
            : (periodType == PeriodType.Dawn) ? dawnMaterial
            : (periodType == PeriodType.Morning) ? morningMaterial
            : (periodType == PeriodType.Afternoon) ? afternoonMaterial
            : (periodType == PeriodType.Dusk) ? duskMaterial
            : (periodType == PeriodType.Late) ? lateMaterial
            : meshRenderer.material;
    }
}