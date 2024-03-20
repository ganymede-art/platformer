using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchedAddActionTrigger : MonoBehaviour, INameable
{
    // Private fields.
    private ISwitch parentSwitch;

    // Public fields.
    public GameObject parentSwitchObject;
    public Pair<SwitchStatusConstant, AddActionHighLogicTrigger>[] interactionPairs;

    private void Awake()
    {
        parentSwitch = parentSwitchObject?.GetComponent<ISwitch>();

        if (parentSwitch != null)
            parentSwitch.StatusChanged += OnParentSwitchStatusChanged;
    }

    private void OnDestroy()
    {
        if (parentSwitch != null)
            parentSwitch.StatusChanged -= OnParentSwitchStatusChanged;
    }

    private void OnParentSwitchStatusChanged(object sender, SwitchArgs args)
    {
        for(int i = 0; i<interactionPairs.Length; i++)
        {
            if (interactionPairs[i].pairKey.SwitchStatus == args.activeStatus)
                interactionPairs[i].pairValue.AddAction();
        }
    }

    public string GetName()
    {
        return $"SwitchedAddActionTrigger";
    }
}
