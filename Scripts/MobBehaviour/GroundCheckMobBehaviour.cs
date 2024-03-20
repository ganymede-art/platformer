using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class GroundCheckMobBehaviour : MonoBehaviour, IBehaviour<Mob, MobBehaviourId>
{
    public MobBehaviourId BehaviourId => MobBehaviourId.GroundCheck;

    // Private fields.
    private bool isSphereCastHit;
    private bool wasSphereCastGrounded;
    private bool isSphereCastGrounded;
    private GameObject sphereCastGroundObject;
    private Vector3 sphereCastGroundNormal;
    private float sphereCastGroundAngle;
    private RaycastHit hitInfo;

    // Public properties.
    public bool IsSphereCastHit => isSphereCastHit;
    public bool WasSphereCastGrounded => wasSphereCastGrounded;
    public bool IsSphereCastGrounded => isSphereCastGrounded;

    // Public fields.
    public float sphereCastRadius;
    public Vector3 sphereCastOffset;
    public float sphereCastDistance;

    public void BeginBehaviour(Mob controller, Dictionary<string, object> args = null) { }
    public void EndBehaviours(Mob controller) { }
    public void FixedUpdateBehaviour(Mob controller) 
    {
        isSphereCastHit = Physics.SphereCast
            ( controller.transform.position + sphereCastOffset
            , sphereCastRadius
            , Vector3.down
            , out hitInfo
            , sphereCastDistance
            , LAYER_MASK_MOB_CHECKSPHERE
            , QueryTriggerInteraction.Ignore);

        if (isSphereCastHit)
        {
            sphereCastGroundNormal = hitInfo.normal;
            sphereCastGroundAngle =
                Vector3.Angle(hitInfo.normal, Vector3.up);
            sphereCastGroundObject = hitInfo.transform.gameObject;
        }
        else
        {
            sphereCastGroundNormal = Vector3.up;
            sphereCastGroundAngle = 0.0F;
            sphereCastGroundObject = null;
        }

        wasSphereCastGrounded = isSphereCastGrounded;
        isSphereCastGrounded = isSphereCastHit
            && sphereCastGroundAngle < GROUND_CHECK_MAX_GROUNDED_ANGLE;
    }
    public void UpdateBehaviour(Mob controller) { }
}
