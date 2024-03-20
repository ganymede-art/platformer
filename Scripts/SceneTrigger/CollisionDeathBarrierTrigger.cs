using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CollisionDeathBarrierTrigger : MonoBehaviour
{
    // Consts.
    private const int DAMAGE_AMOUNT = 1;

    public BoxCollider triggerCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if (other.gameObject == ActiveSceneHighLogic.G.CachedPlayerObject)
        {
            ActiveSceneHighLogic.G.CachedPlayer.Damage.OnSimpleDamage(DAMAGE_AMOUNT);
            ActiveSceneHighLogic.G.CachedPlayer.Bounds.ResetPlayerToLastInBoundsPosition();
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
}
