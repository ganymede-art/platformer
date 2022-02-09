using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.script.GameConstants;

public class PlayerBehaviourMovingObject : MonoBehaviour, IPlayerBehaviour
{
    // collision.

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == GameConstants.LAYER_WORLD_DYNAMIC
            || collision.gameObject.layer == GameConstants.LAYER_WORLD_DYNAMIC_IGNORE_CAMERA)
        {
            GameMasterController.GlobalPlayerController.collidingMovingObjects.Add(collision.gameObject);

            GameMasterController.GlobalPlayerController.isCollidingMovingObject = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (GameMasterController.GlobalPlayerController.collidingMovingObjects.Contains(collision.gameObject))
        {
            GameMasterController.GlobalPlayerController.collidingMovingObjects.Remove(collision.gameObject);

            if (GameMasterController.GlobalPlayerController.collidingMovingObjects.Count == 0)
            {
                GameMasterController.GlobalPlayerController.isCollidingMovingObject = false;
            }
        }
    }

    public string GetBehaviourType()
    {
        return PLAYER_BEHAVIOUR_MOVING_OBJECT;
    }
}
