using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class StartupHighLogic : MonoBehaviour
{
    private float startupTimer;
    private bool isStartupComplete;

    void Start()
    {
        startupTimer = 0.0F;
        isStartupComplete = false;
    }

    private void Update()
    {
        if(startupTimer >= STARTUP_SCENE_INTERVAL && !isStartupComplete)
        {
            isStartupComplete = true;
            GameHighLogic.G.LoadSceneHighLogic.LoadNewScene("MenuMain1", HighLogicStateId.Menu);
            return;
        }
            
        startupTimer += Time.deltaTime;
    }
}
