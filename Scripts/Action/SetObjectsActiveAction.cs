using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectsActiveAction : MonoBehaviour, IAction
{
    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.SetObjectsActive;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Delay Attributes")]
    public GameObject setObject;
    public GameObject[] setObjects;
    public bool isActive;

    public void BeginAction(ActionSource actionSource)
    {
        setObject.SetActive(isActive);
        for (int i = 0; i < setObjects.Length; i++)
            setObjects[i].SetActive(isActive);
    }

    public void UpdateAction(ActionSource actionSource) { }

    public void EndAction(ActionSource actionSource) { }
}
