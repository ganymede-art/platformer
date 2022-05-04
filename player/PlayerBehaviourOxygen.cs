using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Script.GameConstants;
using System;

public class PlayerBehaviourOxygen : MonoBehaviour, IPlayerBehaviour
{
    const float OXYGEN_DRAIN_INTERVAL = 1.0F;
    
    private float oxygenDrainTimer = 0.0F;

    [NonSerialized] public bool wasOxygenDrainActive;
    [NonSerialized] public bool isOxygenDrainActive;

    public string GetBehaviourType()
    {
        return PLAYER_BEHAVIOUR_OXYGEN;
    }

    void Start()
    {
        wasOxygenDrainActive = false;
        isOxygenDrainActive = false;
    }

    void Update()
    {
        if (GameMasterController.Global.gameState != GAME_STATE_GAME)
            return;

        wasOxygenDrainActive = isOxygenDrainActive;
        isOxygenDrainActive =
            (GameMasterController.GlobalPlayerController.behaviourWater.isFullSubmerged);
            
        if(!wasOxygenDrainActive && isOxygenDrainActive)
            SetOxygenDrain();

        if (wasOxygenDrainActive && !isOxygenDrainActive)
            UnsetOxygenDrain();

        if (GameMasterController.GlobalPlayerController.behaviourWater.isFullSubmerged)
        {
            oxygenDrainTimer += Time.deltaTime;
            Debug.Log(oxygenDrainTimer.ToString() + " / " + GamePlayerController.Global.oxygen);

            if (oxygenDrainTimer > OXYGEN_DRAIN_INTERVAL)
            {
                GamePlayerController.Global.ModifyPlayerOxygen(-1);
                oxygenDrainTimer = 0.0F;
            }
        }
    }

    private void SetOxygenDrain()
    {

    }

    private void UnsetOxygenDrain()
    {
        GamePlayerController.Global.ModifyPlayerOxygen(1000);
        oxygenDrainTimer = 0.0F;
    }
}
