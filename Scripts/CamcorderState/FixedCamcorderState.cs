using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class FixedCamcorderState : MonoBehaviour, IState<Camcorder, CamcorderStateId>
{
    // Consts.
    private const float MIN_INTERVAL = 0.001F;

    // Private fields.
    private GameObject fixedPositionObject;
    private float fixedTransitionInterval;
    private float fixedTransitionTimer;
    private float fixedTransitionProgress;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    // Public properties.
    public CamcorderStateId StateId => CamcorderStateId.Fixed;

    public void BeginState(Camcorder c, Dictionary<string, object> args = null) 
    {
        fixedPositionObject = (GameObject)args[CAMCORDER_STATE_ARG_FIXED_POSITION_OBJECT];
        fixedTransitionInterval = (float)args[CAMCORDER_STATE_ARG_FIXED_TRANSITION_INTERVAL];
        fixedTransitionInterval = Mathf.Max(fixedTransitionInterval, MIN_INTERVAL);
        fixedTransitionTimer = 0.0F;
        fixedTransitionProgress = 0.0F;
        startingPosition = c.transform.position;
        startingRotation = c.transform.rotation;
    }

    public void UpdateState(Camcorder c)
    {
        if (fixedTransitionProgress < 1)
        { 
            fixedTransitionProgress = fixedTransitionTimer / fixedTransitionInterval;
            fixedTransitionProgress = Mathf.Clamp(fixedTransitionProgress, 0.0F, 1.0F);
            float lerp = Mathf.SmoothStep(0.0F, 1.0F, fixedTransitionProgress);

            c.transform.position = Vector3.Lerp
                (startingPosition
                , fixedPositionObject.transform.position
                , lerp);
            c.transform.rotation = Quaternion.Lerp
                (startingRotation
                , fixedPositionObject.transform.rotation
                , lerp);

            fixedTransitionTimer += Time.deltaTime;
        }

        c.x = 0.0F;
        c.y = c.transform.rotation.eulerAngles.y;
    }

    public void FixedUpdateState(Camcorder c) { }

    public void EndState(Camcorder c) { }
}
