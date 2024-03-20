using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAction : MonoBehaviour, IAction
{
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.Save;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;
    [Header("Save Attributes")]
    public string sceneName;
    public string sceneStartingTransformName;

    public void BeginAction(ActionSource actionSource)
    {
        PersistenceHighLogic.G.SavePersistence(sceneName, sceneStartingTransformName);
    }

    public void EndAction(ActionSource actionSource) { }
    public void UpdateAction(ActionSource actionSource) { }
}
