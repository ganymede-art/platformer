using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using static Assets.Script.GameConstants;

public class PlayerBehaviourWater : MonoBehaviour, IPlayerBehaviour
{
    // water constants.

    public static readonly Vector3 WATER_PARTIAL_SUBMERGED_OFFSET = new Vector3(0, 0, 0);
    public static readonly Vector3 WATER_FULL_SUBMERGED_OFFSET = new Vector3(0, 0.1625f, 0);

    // public water fields.

    [NonSerialized] public List<GameObject> collidingWaterObjects;
    [NonSerialized] public bool isCollidingWaterObject;
    [NonSerialized] public float waterYLevel;
    [NonSerialized] public bool wasFullSubmerged;
    [NonSerialized] public bool isFullSubmerged;
    [NonSerialized] public bool wasPartialSubmerged;
    [NonSerialized] public bool isPartialSubmerged;

    void Start()
    {
        collidingWaterObjects = new List<GameObject>();

        isCollidingWaterObject = false;
        waterYLevel = 0.0F;

        wasFullSubmerged = false;
        isFullSubmerged = false;

        wasPartialSubmerged = false;
        isPartialSubmerged = false;
    }

    void Update()
    {
        if (!isCollidingWaterObject)
            return;

        wasFullSubmerged = isFullSubmerged;
        wasPartialSubmerged = isPartialSubmerged;

        isPartialSubmerged = (this.transform.position + WATER_PARTIAL_SUBMERGED_OFFSET).y <= waterYLevel;
        isFullSubmerged = (this.transform.position + WATER_FULL_SUBMERGED_OFFSET).y <= waterYLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameConstants.LAYER_WORLD_WATER)
        {
            collidingWaterObjects.Add(other.gameObject);

            isCollidingWaterObject = true;
            waterYLevel = other.bounds.center.y + (other.bounds.size.y / 2);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collidingWaterObjects.Contains(other.gameObject))
        {
            collidingWaterObjects.Remove(other.gameObject);

            if (collidingWaterObjects.Count == 0)
            {
                isCollidingWaterObject = false;
                waterYLevel = 0.0F;
                isFullSubmerged = false;
                isPartialSubmerged = false;
            }
        }
    }

    public string GetBehaviourType()
    {
        return PLAYER_BEHAVIOUR_WATER;
    }
}
