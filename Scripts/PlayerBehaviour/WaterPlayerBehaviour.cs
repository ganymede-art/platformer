using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class WaterPlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    // Consts.
    private float LOWEST_WATER_HEIGHT = -100.0F;

    // Private fields.
    private IRemoteTrigger remoteTrigger;
    private Dictionary<GameObject, Collider> waterColliders;
    private bool isWaterCollision;
    private float waterHeight;
    private bool isPartialSubmerged;
    private bool wasPartialSubmerged;
    private bool isFullSubmerged;
    private bool wasFullSubmerged;

    private bool didBeginBehaviourPartialSubmerged;
    private bool didBeginBehaviourFullSubmerged;
    private bool didEmergeSinceBehaviourBegan;

    // Public properties.
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.Water;

    public bool IsWaterCollision => isWaterCollision;
    public float WaterHeight => waterHeight;
    public bool IsPartialSubmerged => isPartialSubmerged;
    public bool WasPartialSubmerged => wasPartialSubmerged;
    public bool IsFullSubmerged => isFullSubmerged;
    public bool WasFullSubmerged => wasFullSubmerged;

    public bool DidBeginBeheaviourPartialSubmerged => didBeginBehaviourPartialSubmerged;
    public bool DidBeginBehaviourFullSubmerged => didBeginBehaviourFullSubmerged;
    public bool DidEmergeSinceBehaviourBegan => didEmergeSinceBehaviourBegan;

    // Public fields.
    public GameObject remoteTriggerObject;

    private void Awake()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEnter;
        remoteTrigger.RemoteTriggerExited += OnRemoteTriggerExit;
        waterColliders = new Dictionary<GameObject, Collider>();
    }

    public void BeginBehaviour(Player controller, Dictionary<string, object> args = null) 
    {
        didBeginBehaviourPartialSubmerged = isPartialSubmerged;
        didBeginBehaviourFullSubmerged = isFullSubmerged;
        didEmergeSinceBehaviourBegan = (IsPartialSubmerged && !IsFullSubmerged);
    }

    public void FixedUpdateBehaviour(Player controller) { }

    public void UpdateBehaviour(Player c)
    {
        if (!isWaterCollision)
            return;

        wasPartialSubmerged = isPartialSubmerged;
        wasFullSubmerged = isFullSubmerged;

        // Update water height.
        float highestWaterHeight = LOWEST_WATER_HEIGHT;
        foreach(var waterCollider in waterColliders.Values)
        {
            var ray = new Ray(transform.position + WATER_HEIGHT_RAY_OFFSET, Vector3.down);
            RaycastHit hitInfo;

            if (waterCollider.Raycast(ray, out hitInfo, 100.0F) 
                && hitInfo.point.y > highestWaterHeight)
                    highestWaterHeight = hitInfo.point.y;
        }
        waterHeight = highestWaterHeight;

        // Update submerged status.
        isPartialSubmerged = (transform.position + WATER_PARTIAL_SUBMERGED_OFFSET).y <= waterHeight;
        isFullSubmerged = (transform.position + WATER_FULL_SUBMERGED_OFFSET).y <= waterHeight;

        if (!didEmergeSinceBehaviourBegan && !isFullSubmerged)
            didEmergeSinceBehaviourBegan = true;
    }

    public void EndBehaviours(Player controller) { }

    public void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        if (args.other.gameObject.layer != LAYER_WATER)
            return;
        waterColliders.Add(args.other.gameObject, args.other);
        isWaterCollision = true;
    }

    public void OnRemoteTriggerExit(object sender, RemoteTriggerArgs args)
    {
        if (args.other.gameObject.layer != LAYER_WATER)
            return;
        waterColliders.Remove(args.other.gameObject);
        if(waterColliders.Count == 0)
        {
            isWaterCollision = false;
            waterHeight = 0.0F;
            isPartialSubmerged = false;
            isFullSubmerged = false;
        }
    }
}