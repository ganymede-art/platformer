using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class SquashableProp : MonoBehaviour
{
    // Private fields.
    private IRemoteTrigger remoteTrigger;

    // Public fields.
    [Header("Breakable Attributes")]
    public bool canPlayerBreak;
    public bool canMobBreak;
    public bool canStaticBreak;
    [Space]
    public GameObject remoteTriggerObject;
    public GameObject onDiePrefab;
    public Vector3 onDiePrefabSpawnOffset;
    public GameObject fxPrefab;
    public Vector3 fxPrefabSpawnOffset;

    private void Awake()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEnter;
    }

    private void Break()
    {
        if (onDiePrefab != null)
            Instantiate(onDiePrefab, transform.position + onDiePrefabSpawnOffset, transform.rotation);

        Instantiate(fxPrefab, transform.position + fxPrefabSpawnOffset, transform.rotation);
        Destroy(gameObject);
    }

    public void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        if(args.other.name == TRANSFORM_NAME_PLAYER_COLLIDER && canPlayerBreak)
        {
            Break();
            return;
        }
    }
}
