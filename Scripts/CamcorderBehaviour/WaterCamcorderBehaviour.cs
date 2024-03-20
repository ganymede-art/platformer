using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class WaterCamcorderBehaviour : MonoBehaviour, IBehaviour<Camcorder, CamcorderBehaviourId>
{
    // Constants.
    private const float LENS_WOBBLE_SPEED_MULT = 5.0F;
    private const float LENS_WOBBLE_MAX_RANDOM_SPEED_MULT = 0.0F;
    private const float LENS_WOBBLE_X_MULT = 0.2F;
    private const float LENS_WOBBLE_Y_MULT = 0.2F;

    // Private fields.
    private bool isEffectActive;
    private int waterTriggerCount;

    // Public properties.
    public CamcorderBehaviourId BehaviourId => CamcorderBehaviourId.Water;
    private IRemoteTrigger remoteTrigger;

    // Public fields.
    public Camcorder controller;
    public GameObject remoteTriggerObject;

    private void Awake()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEnter;
        remoteTrigger.RemoteTriggerExited += OnRemoteTriggerExit;
    }

    private void OnDestroy()
    {
        if (remoteTrigger != null)
        {
            remoteTrigger.RemoteTriggerEntered -= OnRemoteTriggerEnter;
            remoteTrigger.RemoteTriggerExited -= OnRemoteTriggerExit;
        }

    }

    public void BeginBehaviour(Camcorder controller, Dictionary<string, object> args = null) { }
    public void UpdateBehaviour(Camcorder controller) 
    {
        if (!isEffectActive)
            return;

        float jitter = Random.Range(0.0F, LENS_WOBBLE_MAX_RANDOM_SPEED_MULT);

        float xWobble = Mathf.Sin(Time.time * (LENS_WOBBLE_SPEED_MULT + jitter)) * LENS_WOBBLE_X_MULT;
        float yWobble = Mathf.Sin(Time.time * (LENS_WOBBLE_SPEED_MULT - jitter)) * LENS_WOBBLE_Y_MULT;
        controller.PostLensDistortion.centerX.Override(xWobble);
        controller.PostLensDistortion.centerY.Override(yWobble);
    }
    public void FixedUpdateBehaviour(Camcorder controller) { }
    public void EndBehaviours(Camcorder controller) { }

    private void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        if (args.other.gameObject.layer != LAYER_WATER)
            return;
        waterTriggerCount++;
        RecalculateEffectVisibility();
    }

    private void OnRemoteTriggerExit(object sender, RemoteTriggerArgs args)
    {
        if (args.other.gameObject.layer != LAYER_WATER)
            return;
        waterTriggerCount--;
        RecalculateEffectVisibility();
    }

    private void RecalculateEffectVisibility()
    {
        isEffectActive = waterTriggerCount > 0;
        controller.PostColourGrading.enabled.Override(isEffectActive);
        controller.PostLensDistortion.enabled.Override(isEffectActive);
        controller.AudioLowPassFilter.enabled = isEffectActive;
    }
}
