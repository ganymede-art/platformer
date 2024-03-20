using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDataContainer : MonoBehaviour
{
    public HitboxData hitboxData;

    private void Start()
    {
        ActiveSceneHighLogic.G.HitboxDatas[gameObject] = hitboxData;
    }

    private void OnDestroy()
    {
        if (ActiveSceneHighLogic.G == null)
            return;
        ActiveSceneHighLogic.G.HitboxDatas.Remove(gameObject);
    }
}
