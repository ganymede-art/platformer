using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using static Assets.script.GameConstants;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    const float Y_MIN = -20;
    const float Y_MAX = 60;

    const float X_SPEED = 200f;
    const float Y_SPEED = 200f;
    const float ZOOM_SPEED = 1f;

    const float AUTO_ROTATION_DIFFERENCE_MIN = 30f;
    const float AUTO_X_SPEED = 100f;
    const float AUTO_X_SPEED_MANUAL = 600f;

    const float CAMERA_CLIPPING_RADIUS = 0.05f;

    const float MAX_DISTANCE_MIN = 1.0f;
    const float MAX_DISTANCE_MAX = 3.0f;

    const float EXIT_FIXED_TRANSITION_DURATION = 1f;

    private GameObject targetDirectionObject;

    private float xInput = 0.0f;
    private float yInput = 0.0f;

    private float xSensitivity = 0.0f;
    private float ySensitivity = 0.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    private bool isAutoRotation = false;
    private bool isManualAutoRotation = false;

    private float autoX = 0.0f;
    private float autoXSpeed = 0.0f;

    private float autoRotationDifference = 0.0f;
    private Quaternion autoRotationStart = Quaternion.identity;
    private Quaternion autoRotationEnd = Quaternion.identity;

    // Fixed camera variables.

    private CameraMode cameraMode;

    private Transform fixedTransform = null;
    private float transitionTimer = 0f;
    private float transitionDuration = 2f;

    private Vector3 fixedStartPosition;
    private Quaternion fixedStartRotation;

    // game variables.

    GameMasterController master;

    // public fields.

    public Transform target;
    [FormerlySerializedAs("target_distance")]
    public float targetDistance;
    public float distance;
    [System.NonSerialized] public float transitionProgress = 0f;

    void Start()
    {
        master = GameObject.FindObjectOfType<GameMasterController>();

        cameraMode = CameraMode.camera_default;

        fixedStartPosition = this.transform.position;
        fixedStartRotation = this.transform.rotation;

        fixedTransform = this.transform;
        transitionTimer = 1.0f;

        // set to default target (player).

        target = GameObject.FindGameObjectWithTag(GameConstants.TAG_PLAYER_CAMERA_TARGET).transform;
        targetDirectionObject = GameObject.Find(PlayerConstants.DIRECTION_OBJECT);

        Debug.Log("Camera Starting");
    }

    void Update()
    {
        if (master.gameState == GameState.Game 
            || master.gameState == GameState.Cutscene)
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

        xInput = master.inputController.actionAimHorizontal.ReadValue<float>();
        yInput = master.inputController.actionAimVertical.ReadValue<float>();

        xInput = Mathf.Clamp(xInput, -1, 1);
        yInput = Mathf.Clamp(yInput, -1, 1);

        xSensitivity = master.inputController.sensitivityCameraHorizontal;
        ySensitivity = master.inputController.sensitivityCameraVertical;

        // Get x and y offset from input.

        x += (xInput * (X_SPEED * xSensitivity)) * Time.deltaTime;
        y -= (yInput * (Y_SPEED * ySensitivity)) * Time.deltaTime;

        // if in auto rotation, gradually turn to be behind the target.

        isManualAutoRotation = master.inputController.isInputNegative2;
        if (isAutoRotation || isManualAutoRotation)
        {
            autoX = targetDirectionObject.transform.rotation.eulerAngles.y;

            autoRotationStart = Quaternion.Euler(y, autoX, 0);
            autoRotationEnd = Quaternion.Euler(y, x, 0.0f);

            autoRotationDifference = Quaternion.Angle(autoRotationStart, autoRotationEnd);

            if (isManualAutoRotation)
                autoXSpeed = (autoRotationDifference > AUTO_ROTATION_DIFFERENCE_MIN)
                    ? AUTO_X_SPEED_MANUAL
                    : Mathf.InverseLerp(0, AUTO_ROTATION_DIFFERENCE_MIN, autoRotationDifference) * AUTO_X_SPEED_MANUAL;
            else
                autoXSpeed = (autoRotationDifference > AUTO_ROTATION_DIFFERENCE_MIN) 
                    ? AUTO_X_SPEED 
                    : Mathf.InverseLerp(0, AUTO_ROTATION_DIFFERENCE_MIN, autoRotationDifference) * AUTO_X_SPEED;

            autoRotationEnd = Quaternion.RotateTowards(autoRotationEnd, autoRotationStart, autoXSpeed * Time.deltaTime);

            x = autoRotationEnd.eulerAngles.y;     // auto yaw.
            //y = auto_rotation_end.eulerAngles.x;   // do not auto pitch.
        }

        // Get max distance from input.

        targetDistance += master.inputController.actionAimZoom.ReadValue<float>()
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
            transitionProgress = 1.0f;
            transform.position = fixedTransform.position;
            transform.rotation = fixedTransform.rotation;
        }

        // Set the X and Y rotation to the current rotation.
        // So the dynamic camera is in the same position when
        // exiting the fixed camera zone.

        x = transform.rotation.eulerAngles.y;
        y = transform.rotation.eulerAngles.x;
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
            transitionProgress = 1.0f;
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
            out hit, targetDistance, GameConstants.MASK_PLAYER_IGNORES);

        if (isHit)
        {
            distance = hit.distance;
        }
        else
        {
            if (distance < targetDistance)
            {
                distance += 0.1f;
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

            float lerpSpeed = Vector3.Distance(transform.position, position) * 16f;
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
            : 0.0f;
        transitionProgress = 0.0f;

        this.fixedTransform = fixedTransform;
        this.transitionDuration = transitionDuration;
    }


    public void UnsetCamera()
    {
        cameraMode = CameraMode.camera_default;

        // Reset the transition.

        fixedStartPosition = this.transform.position;
        fixedStartRotation = this.transform.rotation;
        transitionTimer = 0.0f;
        transitionProgress = 0.0f;
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

