using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static CamcorderConstants;
using UnityEngine.InputSystem;

public class OrbitCamcorderState : MonoBehaviour, IState<Camcorder, CamcorderStateId>
{
    public CamcorderStateId StateId => CamcorderStateId.Orbit;

    public void BeginState(Camcorder c, Dictionary<string, object> args = null) { }

    public void UpdateState(Camcorder c)
    {
        if(!InputHighLogic.G.WasFarPressed
            && InputHighLogic.G.IsFarPressed
            && InputHighLogic.G.IsInputActive)
        {
            c.ChangeState(CamcorderStateId.Reorient);
            return;
        }

        // Update input.    
        c.zInput = InputHighLogic.G.Zoom;

        if (InputHighLogic.G.IsUsingMouse)
        {
            c.xInput = InputHighLogic.G.MouseY;
            c.yInput = InputHighLogic.G.MouseX;

            c.x += (c.xInput * MOUSE_LOOK_X_SPEED * c.xSensitivity);
            c.y += (c.yInput * MOUSE_LOOK_Y_SPEED * c.ySensitivity);
        }
        else
        {
            c.xInput = InputHighLogic.G.Look2d.y;
            c.yInput = InputHighLogic.G.Look2d.x;
            c.xInput = Mathf.Clamp(c.xInput, -1.0F, 1.0F);
            c.yInput = Mathf.Clamp(c.yInput, -1.0F, 1.0F);

            c.x += (c.xInput * LOOK_X_SPEED * c.xSensitivity) * Time.deltaTime;
            c.y += (c.yInput * LOOK_Y_SPEED * c.ySensitivity) * Time.deltaTime;
        }

        c.targetDistance += (c.zInput * LOOK_Z_SPEED * c.zSensitivity) * Time.deltaTime;

        c.targetDistance = Mathf.Clamp(c.targetDistance, TARGET_DISTANCE_MIN, TARGET_DISTANCE_MAX);
        c.x = Mathf.Clamp(c.x, LOOK_X_MIN, LOOK_X_MAX);

        // Update movement.
        var targetRotation = Quaternion.Euler(c.x, c.y, 0.0f);

        RaycastHit hit;
        bool isHit = Physics.SphereCast(c.target.position, CAMCORDER_SPHERECAST_RADIUS, -c.transform.forward,
            out hit, c.targetDistance, LAYER_MASK_CAMCORDER_IGNORES,QueryTriggerInteraction.Ignore);

        if(isHit)
        {
            c.distance = hit.distance;
        }
        else
        {
            if (c.distance < c.targetDistance)
                c.distance += LOOK_Z_SPEED * Time.deltaTime;
            else
                c.distance = c.targetDistance;
        }

        var targetPositionOffset = new Vector3(0.0f, 0.0f, -c.distance);
        var targetPosition = targetRotation * targetPositionOffset + c.target.position;

        float rotateSpeed = Quaternion.Angle(transform.rotation, targetRotation) * CAMCORDER_ROTATE_SPEED;
        c.transform.rotation = Quaternion.RotateTowards(c.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        float moveSpeed = Vector3.Distance(transform.position, targetPosition) * CAMCORDER_MOVE_SPEED;
        c.transform.position = Vector3.MoveTowards(c.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void FixedUpdateState(Camcorder c) { }

    public void EndState(Camcorder c) { }
}
