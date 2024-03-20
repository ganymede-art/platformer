using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTrackPlayer : MonoBehaviour
{
    // Public fields.
    public Vector3 offset;

    void Update()
    {
        if(ActiveSceneHighLogic.G.CachedPlayer != null)
            transform.position = ActiveSceneHighLogic.G.CachedPlayer.gameObject.transform.position + offset;
    }
}
