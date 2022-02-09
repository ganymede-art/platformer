using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using static Assets.script.GameConstants;
using UnityEngine.Serialization;
using static Assets.script.GameConstants;

public class CameraController : MonoBehaviour
{
    const float Y_MIN = -20;
    const float Y_MAX = 60;

    const float X_SPEED = 200F;
    const float Y_SPEED = 200F;
    const float ZOOM_SPEED = 1F;

    const float AUTO_ROTATION_DIFFERENCE_MIN = 30F;
    const float AUTO_X_SPEED = 100F;
    const float AUTO_X_SPEED_MANUAL = 600F;

    const float CAMERA_CLIPPING_RADIUS = 0.05F;

    const float MAX_DISTANCE_MIN = 1.0F;
    const float MAX_DISTANCE_MAX = 3.0F;

    const float EXIT_FIXED_TRANSITION_DURATION = 1F;

    private GameObject targetDirectionObject;

    private float xInput = 0.0F;
    private float yInput = 0.0F;

    private float xSensitivity = 0.0F;
    private float ySensitivity = 0.0F;

    private float x = 0.0F;
    private float y = 0.0F;

    private bool isAutoRotation = false;
    private bool isManualAutoRotation = false;
    private float manualAutoRotationTimer = 0.0F;
    private float manualAutoRotationInterval = 1.0F;
    private Quaternion manualAutoRotationTarget;

    private float autoX = 0.0F;
    private float autoXSpeed = 0.0F;

    private float autoRotationDifference = 0.0F;
    private Quaternion autoRotationStart = Quaternion.identity;
    private Quaternion autoRotationEnd = Quaternion.identity;

    // Fixed camera variables.

    private CameraMode cameraMode;

    private Transform fixedTransform = null;
    private float transitionTimer = 0F;
    private float transitionDuration = 2F;

    private Vector3 fixedStartPosition;
    private Quaternion fixedStartRotation;

    // game variables.

    GameMasterController master;

    // public fields.

    public Transform target;
    [FormerlySerializedAs("target_distance")]
    public float targetDistance;
    public float distance;
    [System.NonSerialized] public float transitionProgress = 0F;

    void Start()
    {
        master = GameObject.FindObjectOfType<GameMasterController>();

        cameraMode = CameraMode.camera_default;

        fixedStartPosition = this.transform.position;
        fixedStartRotation = this.transform.rotation;

        fixedTransform = this.transform;
        transitionTimer = 1.0F;

        x = transform.rotation.eulerAngles.y;
        y = transform.rotation.eulerAngles.x;

        // set to default target (player).

        target = GameObject.Find(GameConstants.NAME_PLAYER_CAMERA_TARGET).transform;
        targetDirectionObject = GameObject.Find(PlayerConstants.DIRECTION_OBJECT);

        Debug.Log("Camera Starting");
    }

    void Update()
    {
        if (master.gameState == GAME_STATE_GAME
            || master.gameState == GAME_STATE_CUTSCENE)
        {
            if (cameraMode == GameConstants.CameraMode.camera_default)
            {
                UpdateCameraInput();
                UpdateCameraDefault();
            }
            else if (cameraMode == GameConstants.CameraMode.camera_fixed)
            {
                UpdateCameraFixed();
            }
            else if (cameraMode == GameConstants.CameraMode.camera_fixed_tracking)
            {
                UpdateCameraFixedTracking();
            }
        }
    }

    private void UpdateCameraInput()
    {
        // get input.

        xInput = master.inputController.axisAimHorizontal.ReadValue<float>();
        yInput = master.inputController.axisAimVertical.ReadValue<float>();

        xInput = Mathf.Clamp(xInput, -1, 1);
        yInput = Mathf.Clamp(yInput, -1, 1);

        xSensitivity = master.inputController.sensitivityCameraHorizontal;
        ySensitivity = master.inputController.sensitivityCameraVertical;

        // Get x and y offset from input.

        x += (xInput * (X_SPEED * xSensitivity)) * Time.deltaTime;
        y -= (yInput * (Y_SPEED * ySensitivity)) * Time.deltaTime;

        // if manual auto rotation, gradually turn to the manual target.

        if(!GameInputController.Global.wasInputWestExtra 
            && GameInputController.Global.isInputWestExtra)
        {
            isManualAutoRotation = true;
            manualAutoRotationTimer = 0.0F;
            manualAutoRotationInterval = 1.0F;
            manualAutoRotationTarget = targetDirectionObject.transform.rotation;
        }
        
        if(isManualAutoRotation)
        {
            manualAutoRotationTimer += Time.deltaTime;

            autoX = manualAutoRotationTarget.eulerAngles.y;

            autoRotationStart = Quaternion.Euler(y, autoX, 0);
            autoRotationEnd = Quaternion.Euler(y, x, 0.0f);

            autoRotationDifference = Quaternion.Angle(autoRotationStart, autoRotationEnd);

            autoXSpeed = (autoRotationDifference > AUTO_ROTATION_DIFFERENCE_MIN)
                    ? AUTO_X_SPEED_MANUAL
                    : Mathf.InverseLerp(0, AUTO_ROTATION_DIFFERENCE_MIN, autoRotationDifference) * AUTO_X_SPEED_MANUAL;

            autoRotationEnd = Quaternion.RotateTowards(autoRotationEnd, autoRotationStart, autoXSpeed * Time.deltaTime);

            x = autoRotationEnd.eulerAngles.y;     // auto yaw.

            if (manualAutoRotationTimer > manualAutoRotationInterval)
                isManualAutoRotation = false;
        }


        // if in auto rotation and NOT manual, gradually turn to be behind the target.

        if (isAutoRotation && !isManualAutoRotation)
        {
            autoX = targetDirectionObject.transform.rotation.eulerAngles.y;

            autoRotationStart = Quaternion.Euler(y, autoX, 0);
            autoRotationEnd = Quaternion.Euler(y, x, 0.0f);

            autoRotationDifference = Quaternion.Angle(autoRotationStart, autoRotationEnd);

            autoXSpeed = (autoRotationDifference > AUTO_ROTATION_DIFFERENCE_MIN) 
                ? AUTO_X_SPEED 
                : Mathf.InverseLerp(0, AUTO_ROTATION_DIFFERENCE_MIN, autoRotationDifference) * AUTO_X_SPEED;

            autoRotationEnd = Quaternion.RotateTowards(autoRotationEnd, autoRotationStart, autoXSpeed * Time.deltaTime);

            x = autoRotationEnd.eulerAngles.y;     // auto yaw.
            //y = auto_rotation_end.eulerAngles.x;   // do not auto pitch.
        }

        // Get max distance from input.

        targetDistance += master.inputController.axisZoom.ReadValue<float>()
            * (ZOOM_SPEED * master.inputController.sensitivityCameraZoom);

        if (targetDistance > MAX_DISTANCE_MAX) targetDistance = MAX_DISTANCE_MAX;
        if (targetDistance < MAX_DISTANCE_MIN) targetDistance = MAX_DISTANCE_MIN;

        // Limit how low or high y can go.

        y = Mathf.Clamp(y, Y_MIN, Y_MAX);
    }

    private void UpdateCameraFixed()
    {
        if (transitionProgress < 1)
        {
            // Increment the transition amount (0 is none, 1 is complete).

            transitionTimer += Time.deltaTime;
            transitionProgress = Mathf.InverseLerp(0, transitionDuration, transitionTimer);

            float t = transitionTimer / transitionDuration;
            t = t * t * (3f - 2f * t);
            t = Mathf.Clamp(t, 0, 1);

            // Lerp between the start and end points.

            transform.position = Vector3.Lerp(fixedStartPosition, fixedTransform.position, t);
            transform.rotation = Quaternion.Lerp(fixedStartRotation, fixedTransform.rotation, t);
        }
        else
        {
            transitionProgress = 1.0F;
            transform.position = fixedTransform.position;
            transform.rotation = fixedTransform.rotation;
        }

        // Set the X and Y rotation to the current rotation.
        // So the dynamic camera is in the same position when
        // exiting the fixed camera zone.

        x = transform.rotation.eulerAngles.y;
        y = 0;
    }

    private void UpdateCameraFixedTracking()
    {
        if (transitionProgress < 1)
        {
            // Increment the transition amount (0 is none, 1 is complete).

            transitionTimer += Time.deltaTime;
            transitionProgress = Mathf.InverseLerp(0, transitionDuration, transitionTimer);

            float t = transitionTimer / transitionDuration;
            t = t * t * (3f - 2f * t);
            t = Mathf.Clamp(t, 0, 1);
            
            // Lerp between the start and end points.

            transform.position = Vector3.Lerp(fixedStartPosition, fixedTransform.position, t);
            transform.LookAt(target.position);
        }
        else
        {
            transitionProgress = 1.0F;
            transform.position = fixedTransform.position;
            transform.LookAt(target.position);
        }

        // Set the X and Y rotation to the current rotation.
        // So the dynamic camera is in the same position when
        // exiting the fixed camera zone.

        x = transform.rotation.eulerAngles.y;
        y = 0; // flatten the tilt when exiting tracking camera.
        //y = transform.rotation.eulerAngles.x;
    }

    private void UpdateCameraDefault()
    {
        // Get the rotation from x and y.

        var rotation = Quaternion.Euler(y, x, 0.0f);

        // Get the camera's distance, checking for collision.

        RaycastHit hit;
        bool isHit = Physics.SphereCast(target.position, CAMERA_CLIPPING_RADIUS, transform.forward * -1, 
            out hit, targetDistance, GameConstants.MASK_CAMERA_IGNORES);

        if (isHit)
        {
            distance = hit.distance;
        }
        else
        {
            if (distance < targetDistance)
            {
                distance += 0.1F;
            }
            if (distance > targetDistance)
            {
                distance = targetDistance;
            }
        }

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        // Apply the position and rotation to the camera.

        if (transitionProgress < 1)
        {
            // Increment the transition amount (0 is none, 1 is complete).

            transitionTimer += Time.deltaTime;
            transitionProgress = Mathf.InverseLerp(0, transitionDuration, transitionTimer);

            float t = transitionTimer / transitionDuration;
            t = t * t * (3f - 2f * t);
            t = Mathf.Clamp(t, 0, 1);

            // Lerp between the start and end points.

            transform.position = Vector3.Lerp(fixedStartPosition, position, t);
            transform.rotation = Quaternion.Lerp(fixedStartRotation, rotation, t);
        }
        else
        {
            //transform.rotation = rotation;
            //transform.position = position;

            float rotateSpeed = Quaternion.Angle(transform.rotation, rotation) * 16;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);

            float lerpSpeed = Vector3.Distance(transform.position, position) * 16F;
            transform.position = Vector3.MoveTowards(transform.position, position, lerpSpeed * Time.deltaTime); 
        }
    }

    public void SetFixedCamera(Transform fixedTransform, bool isTracking, bool isInstant, float transitionDuration)
    {
        cameraMode = (isTracking) 
            ? CameraMode.camera_fixed_tracking 
            : CameraMode.camera_fixed;

        // set the transition.

        fixedStartPosition = this.transform.position;
        fixedStartRotation = this.transform.rotation;
        transitionTimer = (isInstant)
            ? transitionDuration
            : 0.0F;
        transitionProgress = 0.0F;

        this.fixedTransform = fixedTransform;
        this.transitionDuration = transitionDuration;
    }


    public void UnsetCamera()
    {
        cameraMode = CameraMode.camera_default;

        // Reset the transition.

        fixedStartPosition = this.transform.position;
        fixedStartRotation = this.transform.rotation;
        transitionTimer = 0.0F;
        transitionProgress = 0.0F;
        this.transitionDuration = EXIT_FIXED_TRANSITION_DURATION;
    }

    public void SetAutoRotation()
    {
        isAutoRotation = true;
    }

    public void UnsetAutoRotation()
    {
        isAutoRotation = false;
    }
}

