using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProppedAddActionTrigger : MonoBehaviour
{
    // Private fields.
    private IProp parentProp;

    // Public fields.
    public GameObject parentPropGameObject;
    public Pair<PropStatusConstant, AddActionHighLogicTrigger>[] interactionPairs;

    private void Awake()
    {
        parentProp = parentPropGameObject?.GetComponent<IProp>();

        if (parentProp != null)
            parentProp.StatusChanged += OnParentSwitchStatusChanged;
    }

    private void OnDestroy()
    {
        if (parentProp != null)
            parentProp.StatusChanged -= OnParentSwitchStatusChanged;
    }

    private void OnParentSwitchStatusChanged(object sender, PropArgs args)
    {
        for (int i = 0; i < interactionPairs.Length; i++)
        {
            if (interactionPairs[i].pairKey.PropStatus == args.activeStatus)
                interactionPairs[i].pairValue.AddAction();
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (parentPropGameObject != null)
            Gizmos.DrawLine(transform.position, parentPropGameObject.transform.position);
    }
    #endif
}
