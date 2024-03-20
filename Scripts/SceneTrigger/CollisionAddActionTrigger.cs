using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CollisionAddActionTrigger : MonoBehaviour, INameable
{
    public BoxCollider triggerCollider;
    public AddActionHighLogicTrigger highLogicTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if (other.name == TRANSFORM_NAME_PLAYER_COLLIDER)
        {
            highLogicTrigger.AddAction();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (triggerCollider == null)
            return;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS
            (triggerCollider.transform.position
            , triggerCollider.transform.rotation
            , triggerCollider.transform.lossyScale);

        Gizmos.matrix = rotationMatrix;
        Gizmos.color = GIZMO_COLOUR;
        Gizmos.DrawCube(triggerCollider.center, triggerCollider.size);
    }
#endif

    public string GetName()
    {
        if (highLogicTrigger == null)
            return $"AddActionTrigger";
        else
            return $"AddActionTrigger{highLogicTrigger.actionId}";
    }
}
