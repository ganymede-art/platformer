using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideSwitchStatusAction : MonoBehaviour, IAction
{
    // Private fields.
    private ISwitch overrideSwitch;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Switch Attributes")]
    public GameObject overrideSwitchObject;
    public SwitchStatusConstant switchStatus;

    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.OverrideSwitchStatus;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    public void BeginAction(ActionSource actionSource)
    {
        overrideSwitch = overrideSwitchObject.GetComponent<ISwitch>();
        if (overrideSwitch != null)
            overrideSwitch.OverrideStatus(switchStatus.SwitchStatus);
    }

    public void EndAction(ActionSource actionSource) { }
    public void UpdateAction(ActionSource actionSource) { }
}
