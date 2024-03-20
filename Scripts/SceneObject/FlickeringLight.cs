using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    // Consts.
    private enum FlickerStatus
    {
        Stable,
        Flickering,
    }

    // Private fields.
    private FlickerStatus status;
    private float timer;
    private float interval;
    private float stableIntensity;

    // Public fields.
    public Light flickeringLight;

    public float minStableInterval;
    public float maxStableInterval;

    public float minFlickerInterval;
    public float maxFlickerInterval;

    public float minFlickerIntensity;
    public float maxFlickerIntensity;

    public float flickerSpeedMult;

    private void Awake()
    {
        status = FlickerStatus.Stable;
        timer = 0.0F;
        interval = Random.Range(minStableInterval, maxStableInterval);
        stableIntensity = flickeringLight.intensity;
    }

    void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        if(timer > interval)
        {
            timer = 0.0F;

            if(status == FlickerStatus.Stable)
            {
                status = FlickerStatus.Flickering;
                interval = Random.Range(minFlickerInterval, maxFlickerInterval);
                flickeringLight.intensity = stableIntensity;
            }
            else if(status == FlickerStatus.Flickering)
            {
                status = FlickerStatus.Stable;
                interval = Random.Range(minStableInterval, maxStableInterval);
            }
        }

        if(status == FlickerStatus.Flickering)
        {
            float flickerMovement = Random.Range(-flickerSpeedMult, flickerSpeedMult) * Time.deltaTime;
            float newIntensity = flickeringLight.intensity + flickerMovement;
            newIntensity = Mathf.Clamp(newIntensity, minFlickerIntensity, maxFlickerIntensity);
            flickeringLight.intensity = newIntensity;
        }

        timer += Time.deltaTime;
    }
}
