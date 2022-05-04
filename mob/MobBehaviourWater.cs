using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Assets.Script.GameConstants;

public class MobBehaviourWater : MonoBehaviour, IMobBehaviour
{
    [NonSerialized] public List<GameObject> collidingWaterObjects;
    [NonSerialized] public bool isCollidingWaterObject;
    [NonSerialized] public float waterYLevel;
    [NonSerialized] public bool wasFullSubmerged;
    [NonSerialized] public bool isFullSubmerged;
    [NonSerialized] public bool wasPartialSubmerged;
    [NonSerialized] public bool isPartialSubmerged;

    public Vector3 fullSubmergedOffset;
    public Vector3 partialSubmergedOffset;

    public string GetBehaviourType()
    {
        return MOB_BEHAVIOUR_WATER;
    }

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

        isPartialSubmerged = (transform.position + partialSubmergedOffset).y <= waterYLevel;
        isFullSubmerged = (transform.position + fullSubmergedOffset).y <= waterYLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LAYER_WORLD_WATER)
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
}
