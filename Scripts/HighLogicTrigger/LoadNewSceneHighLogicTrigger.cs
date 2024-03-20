using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class LoadNewSceneHighLogicTrigger : MonoBehaviour
{
    // Private fields.
    private Dictionary<string, object> args;

    // Public fields.
    public string newSceneName;
    public bool doOverrideStartingObject;
    public string startingObjectName;

    private void Start()
    {
        args = new Dictionary<string, object>();

        if (doOverrideStartingObject)
            args.Add(LOAD_NEW_SCENE_ARG_STARTING_OBJECT_NAME, startingObjectName);
    }

    public void LoadNewScene()
    {
        LoadSceneHighLogic.G.LoadNewScene(newSceneName, HighLogicStateId.Play, args);
    }
}
