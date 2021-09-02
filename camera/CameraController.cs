using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class CameraController : MonoBehaviour
{
    const float Y_MIN = -20;
    const float Y_MAX = 60;

    const float X_SPEED = 1f;
    const float Y_SPEED = 1f;
    const float ZOOM_SPEED = 1f;

    const float AUTO_ROTATION_DIFFERENCE_MIN = 30f;
    const float AUTO_X_SPEED = 0.1f;
    const float AUTO_X_SPEED_MANUAL = 0.5f;

    const float CAMERA_CLIPPING_RADIUS = 0.05f;

    const float MAX_DISTANCE_MIN = 1.0f;
    const float MAX_DISTANCE_MAX = 5.0f;

    public Transform target;
    public float target_distance;
    private GameObject target_direction_object;

    public float distance;

    private float x = 0.0f;
    private float y = 0.0f;

    private bool is_auto_rotation = false;
    private bool is_manual_auto_rotation = false;

    private float auto_x = 0.0f;
    private float auto_x_speed = 0.0f;

    private float auto_rotation_difference = 0.0f;
    private Quaternion auto_rotation_start = Quaternion.identity;
    private Quaternion auto_rotation_end = Quaternion.identity;

    // Fixed camera variables.

    private GameConstants.CameraMode camera_mode;

    private Transform fixed_transform = null;
    private float fixed_transition = 0f;

    private Vector3 fixed_start_position;
    private Quaternion fixed_start_rotation;

    const float FIXED_TRANSITION_STEP = 2f;

    // game variables.

    GameMasterController master;

    // properties.

    public float Fixed_Transition
    {
        get { return fixed_transition; }
    }


    void Start()
    {
        master = GameObject.FindObjectOfType<GameMasterController>();

        camera_mode = GameConstants.CameraMode.camera_default;

        fixed_start_position = this.transform.position;
        fixed_start_rotation = this.transform.rotation;

        fixed_transform = this.transform;
        fixed_transition = 1.0f;

        // set to default target (player).

        target = GameObject.FindGameObjectWithTag(GameConstants.TAG_PLAYER_CAMERA_TARGET).transform;
        target_direction_object = GameObject.Find(PlayerConstants.PLAYER_DIRECTION_GAME_OBJECT_NAME);

        Debug.Log("Camera Starting");
    }

    void Update()
    {
        if (master.game_state == GameState.Game)
        {
            if (camera_mode == GameConstants.CameraMode.camera_default)
            {
                UpdateCameraInput();
                UpdateCameraDefault();
            }
            else if (camera_mode == GameConstants.CameraMode.camera_fixed)
            {
                UpdateCameraFixed();
            }
            else if (camera_mode == GameConstants.CameraMode.camera_fixed_tracking)
            {
                UpdateCameraFixedTracking();
            }
        }
        else if (master.game_state == GameState.Cutscene)
        {
            if (camera_mode == GameConstants.CameraMode.camera_default)
            {
                UpdateCameraDefault();
            }
            else if (camera_mode == GameConstants.CameraMode.camera_fixed)
            {
                UpdateCameraFixed();
            }
            else if (camera_mode == GameConstants.CameraMode.camera_fixed_tracking)
            {
                UpdateCameraFixedTracking();
            }
        }
    }

    private void UpdateCameraInput()
    {
        // Get x and y offset from input.

        x += master.input_controller.action_aim_horizontal.ReadValue<float>() * (X_SPEED
            * master.input_controller.sensitivity_camera_horizontal);

        y -= master.input_controller.action_aim_vertical.ReadValue<float>() * (Y_SPEED
            * master.input_controller.sensitivity_camera_vertical);

        // if in auto rotation, gradually turn to be behind the target.

        is_manual_auto_rotation = master.input_controller.is_input_extra_positive;
        if (is_auto_rotation || is_manual_auto_rotation)
        {
            auto_x = target_direction_object.transform.rotation.eulerAngles.y;

            auto_rotation_start = Quaternion.Euler(y, auto_x, 0);
            auto_rotation_end = Quaternion.Euler(y, x, 0.0f);

            auto_rotation_difference = Quaternion.Angle(auto_rotation_start, auto_rotation_end);

            if (is_manual_auto_rotation)
                auto_x_speed = (auto_rotation_difference > AUTO_ROTATION_DIFFERENCE_MIN)
                    ? AUTO_X_SPEED_MANUAL
                    : Mathf.InverseLerp(0, AUTO_ROTATION_DIFFERENCE_MIN, auto_rotation_difference) * AUTO_X_SPEED_MANUAL;
            else
                auto_x_speed = (auto_rotation_difference > AUTO_ROTATION_DIFFERENCE_MIN) 
                    ? AUTO_X_SPEED 
                    : Mathf.InverseLerp(0, AUTO_ROTATION_DIFFERENCE_MIN, auto_rotation_difference) * AUTO_X_SPEED;

            auto_rotation_end = Quaternion.RotateTowards(auto_rotation_end, auto_rotation_start, auto_x_speed);

            x = auto_rotation_end.eulerAngles.y;     // auto yaw.
            //y = auto_rotation_end.eulerAngles.x;   // do not auto pitch.
        }

        // Get max distance from input.

        target_distance += master.input_controller.action_aim_zoom.ReadValue<float>()
            * (ZOOM_SPEED * master.input_controller.sensitivity_camera_zoom);

        if (target_distance > MAX_DISTANCE_MAX) target_distance = MAX_DISTANCE_MAX;
        if (target_distance < MAX_DISTANCE_MIN) target_distance = MAX_DISTANCE_MIN;

        // Limit how low or high y can go.

        y = Mathf.Clamp(y, Y_MIN, Y_MAX);
    }

    private void UpdateCameraFixed()
    {
        // Get the transition speed.

        float tran_speed = FIXED_TRANSITION_STEP * Time.deltaTime;

        // Increment the transition amount (0 is none, 1 is complete).

        fixed_transition += tran_speed;
        fixed_transition = Mathf.Clamp(fixed_transition, 0, 1);

        // Lerp between the start and end points.

        transform.position = Vector3.Lerp(fixed_start_position, fixed_transform.position, fixed_transition);
        transform.rotation = Quaternion.Lerp(fixed_start_rotation, fixed_transform.rotation, fixed_transition);

        // Set the X and Y rotation to the current rotation.
        // So the dynamic camera is in the same position when
        // exiting the fixed camera zone.

        x = transform.rotation.eulerAngles.y;
        y = transform.rotation.eulerAngles.x;
    }

    private void UpdateCameraFixedTracking()
    {
        // Get the transition speed.

        float tran_speed = FIXED_TRANSITION_STEP * Time.deltaTime;

        // Increment the transition amount (0 is none, 1 is complete).

        fixed_transition += tran_speed;
        fixed_transition = Mathf.Clamp(fixed_transition, 0, 1);

        // Lerp between the start and end points.

        transform.position = Vector3.Lerp(fixed_start_position, fixed_transform.position, fixed_transition);
        transform.LookAt(target.position);

        // Set the X and Y rotation to the current rotation.
        // So the dynamic camera is in the same position when
        // exiting the fixed camera zone.

        x = transform.rotation.eulerAngles.y;
        y = transform.rotation.eulerAngles.x;
    }

    private void UpdateCameraDefault()
    {
        // Get the rotation from x and y.

        var rotation = Quaternion.Euler(y, x, 0.0f);

        // Get the camera's distance, checking for collision.

        RaycastHit hit;
        if (Physics.SphereCast(target.position, CAMERA_CLIPPING_RADIUS, transform.forward * -1, out hit, target_distance))
        {
            distance = hit.distance;
        }
        else
        {
            if (distance < target_distance)
            {
                distance += 0.1f;
            }
            if (distance > target_distance)
            {
                distance = target_distance;
            }
        }

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        // Apply the position and rotation to the camera.

        if (fixed_transition < 1.0f)
        {
            // Get the transition speed.

            float tran_speed = FIXED_TRANSITION_STEP * Time.deltaTime;

            // Increment the transition amount (0 is none, 1 is complete).

            fixed_transition += tran_speed;
            fixed_transition = Mathf.Clamp(fixed_transition, 0, 1);

            // Lerp between the start and end points.

            transform.position = Vector3.Lerp(fixed_start_position, position, fixed_transition);
            transform.rotation = Quaternion.Lerp(fixed_start_rotation, rotation, fixed_transition);
        }
        else
        {
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public void SetCamera(GameConstants.CameraMode new_camera_mode, CameraModeChangeData new_data)
    {
        this.camera_mode = new_camera_mode;

        if (camera_mode == GameConstants.CameraMode.camera_default)
        {
            // reset the transition.

            fixed_start_position = this.transform.position;
            fixed_start_rotation = this.transform.rotation;
            fixed_transition = 0.0f;
        }
        else if (camera_mode == GameConstants.CameraMode.camera_fixed)
        {
            // set the transition.

            fixed_start_position = this.transform.position;
            fixed_start_rotation = this.transform.rotation;
            fixed_transition = 0.0f;

            this.fixed_transform = new_data.fixed_transform;
        }
        else if (camera_mode == GameConstants.CameraMode.camera_fixed_tracking)
        {
            // set the transition.

            fixed_start_position = this.transform.position;
            fixed_start_rotation = this.transform.rotation;
            fixed_transition = 0.0f;

            this.fixed_transform = new_data.fixed_transform;
        }
    }

    public void UnsetCamera()
    {
        camera_mode = GameConstants.CameraMode.camera_default;

        // Reset the transition.

        fixed_start_position = this.transform.position;
        fixed_start_rotation = this.transform.rotation;
        fixed_transition = 0.0f;
    }

    public void SetAutoRotation()
    {
        is_auto_rotation = true;
    }

    public void UnsetAutoRotation()
    {
        is_auto_rotation = false;
    }
}

