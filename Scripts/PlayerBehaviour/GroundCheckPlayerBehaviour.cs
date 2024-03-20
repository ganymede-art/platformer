using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class GroundCheckPlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    // Private fields.
    private bool isCheckSphereHit;
    private bool isCheckSphereGrounded;
    private bool wasCheckSphereGrounded;
    private bool wasCheckSphereGroundedAfterBegin;

    private bool isRayCastHit;
    RaycastHit raycastHit;
    private Vector3 raycastGroundNormal;
    private float raycastGroundAngle;

    private bool isSpherecastHit;
    RaycastHit spherecastHit;
    private Vector3 spherecastGroundNormal;
    private float spherecastGroundAngle;
    private GameObject spherecastGroundObject;

    // Public properties.
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.GroundCheck;

    public bool IsCheckSphereHit => isCheckSphereHit;
    public bool IsCheckSphereGrounded => isCheckSphereGrounded;
    public bool WasCheckSphereGrounded => wasCheckSphereGrounded;
    public bool WasCheckSphereGroundedAfterBegin => wasCheckSphereGroundedAfterBegin;

    public Vector3 SpherecastGroundNormal => spherecastGroundNormal;
    public float SpherecastGroundAngle => spherecastGroundAngle;
    public GameObject SpherecastGroundObject => spherecastGroundObject;

    public void BeginBehaviour(Player controller, Dictionary<string, object> args = null) 
    {
        wasCheckSphereGroundedAfterBegin = false;
    }

    public void UpdateBehaviour(Player controller)
    {
        UpdateRayCast(controller);
        UpdateSphereCast(controller);
        UpdateCheckSphere(controller);
    }

    public void FixedUpdateBehaviour(Player controller) { }

    public void EndBehaviours(Player controller) { }

    private void UpdateRayCast(Player controller)
    {
        isRayCastHit = Physics.Raycast
            (controller.transform.position
            , Vector3.down
            , out raycastHit
            , 100.0F
            , LAYER_MASK_PLAYER_IGNORES
            , QueryTriggerInteraction.Ignore);

        raycastGroundNormal = (isRayCastHit)
            ? raycastHit.normal
            : Vector3.up;

        raycastGroundAngle = (isRayCastHit)
            ? Vector3.Angle(raycastHit.normal, Vector3.up)
            : 0.0F;
    }

    private void UpdateSphereCast(Player controller)
    {
        isSpherecastHit = Physics.SphereCast
            (controller.transform.position
            , GROUND_CHECK_SPHERECAST_RADIUS
            , Vector3.down
            , out spherecastHit
            , GROUND_CHECK_SPHERECAST_DISTANCE
            , LAYER_MASK_PLAYER_IGNORES
            , QueryTriggerInteraction.Ignore);

        if(isSpherecastHit)
        {
            spherecastGroundNormal = spherecastHit.normal;
            spherecastGroundAngle = 
                Vector3.Angle(spherecastHit.normal, Vector3.up);
            spherecastGroundObject = spherecastHit.transform.gameObject;
        }
        else
        {
            spherecastGroundNormal = Vector3.up;
            spherecastGroundAngle = 0.0F;
            spherecastGroundObject = null;
        }
    }

    private void UpdateCheckSphere(Player controller)
    {
        isCheckSphereHit = Physics.CheckSphere
            ( controller.transform.position + GROUND_CHECK_CHECK_SPHERE_OFFSET
            , GROUND_CHECK_CHECK_SPHERE_RADIUS
            , LAYER_MASK_PLAYER_CHECKSPHERE
            , QueryTriggerInteraction.Ignore);


        wasCheckSphereGrounded = isCheckSphereGrounded;
        isCheckSphereGrounded = isCheckSphereHit
            && spherecastGroundAngle < GROUND_CHECK_MAX_GROUNDED_ANGLE;

        if (!wasCheckSphereGroundedAfterBegin)
            wasCheckSphereGroundedAfterBegin = isCheckSphereGrounded;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.green;
        if (isCheckSphereGrounded)
        {
            Gizmos.DrawRay(transform.position, Vector3.left);
            Gizmos.DrawRay(transform.position, Vector3.right);
            Gizmos.DrawRay(transform.position, Vector3.forward);
            Gizmos.DrawRay(transform.position, Vector3.back);
            Gizmos.DrawWireCube(transform.position,new Vector3(0.1F,0.0F,0.1F));
        }
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + GROUND_CHECK_CHECK_SPHERE_OFFSET, GROUND_CHECK_CHECK_SPHERE_RADIUS);
    }
}
