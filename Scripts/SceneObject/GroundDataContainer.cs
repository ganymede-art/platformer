using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDataContainer : MonoBehaviour
{
    public GroundData groundData;

    private void Start()
    {
        ActiveSceneHighLogic.G.GroundDatas[gameObject] = groundData;
    }

    private void OnDestroy()
    {
        if (ActiveSceneHighLogic.G == null)
            return;
        ActiveSceneHighLogic.G.GroundDatas.Remove(gameObject);
    }
}
