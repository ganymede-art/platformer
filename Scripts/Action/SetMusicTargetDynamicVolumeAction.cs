using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicTargetDynamicVolumeAction : MonoBehaviour, IAction
{
    // Public Properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.SetMusicTargetDynamicVolume;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    // Public fields
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Volume Attributes")]
    public float targetDynamicVolume;

    public void BeginAction(ActionSource actionSource)
    {
        MusicHighLogic.G.SetTargetDynamicVolume(targetDynamicVolume);
    }

    public void EndAction(ActionSource actionSource) { }
    public void UpdateAction(ActionSource actionSource) { }
}
