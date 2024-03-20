using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static MobConstants;

public class WallCheckMobBehaviour : MonoBehaviour, IBehaviour<Mob, MobBehaviourId>
{
    // Private fields.
    private bool isHit;
    private float hitHorizontalAngle;
    private float hitVerticalAngle;
    private bool isWallHit;
    private RaycastHit hitInfo;

    // Public properties.
    public MobBehaviourId BehaviourId => MobBehaviourId.WallCheck;
    public bool IsWallHit => isWallHit;
    public float HitHorizontalAngle => hitHorizontalAngle;
    public float HitVerticalAngle => hitVerticalAngle;
    public RaycastHit HitInfo => hitInfo;

    // Public fields.
    public float spherecastDistance;
    public float spherecastRadius;

    public void BeginBehaviour(Mob c, Dictionary<string, object> args = null) { }
    public void FixedUpdateBehaviour(Mob c) { }

    public void UpdateBehaviour(Mob c) 
    {
        hitVerticalAngle = 0.0F;
        isWallHit = false;

        isHit = Physics.SphereCast
            ( c.mobDirectionObject.transform.position
            , spherecastRadius
            , c.mobDirectionObject.transform.forward
            , out hitInfo
            , spherecastDistance
            , LAYER_MASK_MOB_IGNORES
            , QueryTriggerInteraction.Ignore);

        if(isHit)
        {
            hitHorizontalAngle = Vector3.Angle(HitInfo.normal, -c.mobDirectionObject.transform.forward);
            hitVerticalAngle = Vector3.Angle(hitInfo.normal, Vector3.up);
            isWallHit = hitVerticalAngle >= WALL_CHECK_WALL_ANGLE;
        }
    }

    public void EndBehaviours(Mob c) { }
}
