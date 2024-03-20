using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CollisionLoadNewSceneTrigger : MonoBehaviour
{
    public BoxCollider triggerCollider;
    public LoadNewSceneHighLogicTrigger highLogicTrigger;

    private void Start()
    {
        if(highLogicTrigger == null)
            highLogicTrigger = gameObject.GetComponent<LoadNewSceneHighLogicTrigger>();

        if (highLogicTrigger == null)
            enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if(other.gameObject == ActiveSceneHighLogic.G.CachedPlayerObject)
        {
            highLogicTrigger.LoadNewScene();
        }
    }

    private void OnDrawGizmos()
    {
        if (triggerCollider == null)
            return;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS
            ( triggerCollider.transform.position
            , triggerCollider.transform.rotation
            , triggerCollider.transform.lossyScale);

        Gizmos.matrix = rotationMatrix;
        Gizmos.color = GIZMO_COLOUR;
        Gizmos.DrawCube(triggerCollider.center, triggerCollider.size);
    }
}
