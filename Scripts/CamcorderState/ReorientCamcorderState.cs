using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static CamcorderConstants;

public class ReorientCamcorderState : MonoBehaviour, IState<Camcorder, CamcorderStateId>
{
    // Private fields.
    private Vector3 position;
    private Quaternion rotation;

    // Public properties.
    public CamcorderStateId StateId => CamcorderStateId.Reorient;

    public void BeginState(Camcorder c, Dictionary<string, object> args = null)
    {
        position = ActiveSceneHighLogic.G.CachedPlayer.playerDirectionObject.transform.position;
        rotation = ActiveSceneHighLogic.G.CachedPlayer.playerDirectionObject.transform.rotation;
    }

    public void FixedUpdateState(Camcorder c)
    {

    }

    public void UpdateState(Camcorder c)
    {
        if(c.StateTimer > REORIENT_MAX_INTERVAL)
        {
            c.ChangeState(CamcorderStateId.Orbit);
            return;
        }

        float rotateSpeed = REORIENT_ROTATE_SPEED;
        c.transform.rotation = Quaternion.RotateTowards(c.transform.rotation, rotation, rotateSpeed * Time.deltaTime);

        float moveSpeed = REORIENT_MOVE_SPEED;
        c.transform.position = Vector3.MoveTowards(c.transform.position, position, moveSpeed * Time.deltaTime);

        c.x = 15.0F;
        c.y = c.transform.rotation.eulerAngles.y;
        c.targetDistance = TARGET_DISTANCE_MIN;
    }

    public void EndState(Camcorder c)
    {
        
    }

    
}
