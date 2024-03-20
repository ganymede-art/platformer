using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLookAtPlayerObject : MonoBehaviour
{
    void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        transform.LookAt(ActiveSceneHighLogic.G.CachedPlayer.gameObject.transform.transform);
    }
}
