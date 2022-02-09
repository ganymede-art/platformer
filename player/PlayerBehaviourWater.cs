using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.script.GameConstants;

public class PlayerBehaviourWater : MonoBehaviour, IPlayerBehaviour
{
    // water constants.

    public static readonly Vector3 WATER_PARTIAL_SUBMERGED_OFFSET = new Vector3(0, 0, 0);
    public static readonly Vector3 WATER_FULL_SUBMERGED_OFFSET = new Vector3(0, 0.1625f, 0);

    void Start()
    {
        
    }

    void Update()
    {
        if (!GameMasterController.GlobalPlayerController.isCollidingWaterObject)
            return;

        GameMasterController.GlobalPlayerController.isPartialSubmerged = (this.transform.position + WATER_PARTIAL_SUBMERGED_OFFSET).y <= GameMasterController.GlobalPlayerController.waterYLevel;
        GameMasterController.GlobalPlayerController.isFullSubmerged = (this.transform.position + WATER_FULL_SUBMERGED_OFFSET).y <= GameMasterController.GlobalPlayerController.waterYLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameConstants.LAYER_WORLD_WATER)
        {
            GameMasterController.GlobalPlayerController.collidingWaterObjects.Add(other.gameObject);

            GameMasterController.GlobalPlayerController.isCollidingWaterObject = true;
            GameMasterController.GlobalPlayerController.waterYLevel = other.bounds.center.y + (other.bounds.size.y / 2);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameMasterController.GlobalPlayerController.collidingWaterObjects.Contains(other.gameObject))
        {
            GameMasterController.GlobalPlayerController.collidingWaterObjects.Remove(other.gameObject);

            if (GameMasterController.GlobalPlayerController.collidingWaterObjects.Count == 0)
            {
                GameMasterController.GlobalPlayerController.isCollidingWaterObject = false;
                GameMasterController.GlobalPlayerController.waterYLevel = 0.0F;
                GameMasterController.GlobalPlayerController.isFullSubmerged = false;
                GameMasterController.GlobalPlayerController.isPartialSubmerged = false;
            }
        }
    }

    public string GetBehaviourType()
    {
        return PLAYER_BEHAVIOUR_WATER;
    }
}
